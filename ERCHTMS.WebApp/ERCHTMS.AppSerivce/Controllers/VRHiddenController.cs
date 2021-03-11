using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.PublicInfoManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BSFramework.Util;
using ERCHTMS.Busines.MatterManage;

namespace ERCHTMS.AppSerivce.Controllers
{
    [HandlerLogin(LoginMode.Enforce)]
    public class VRHiddenController : BaseApiController
    {
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //隐患基本信息
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL(); //隐患验收信息
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private WfControlBLL wfcontrolbll = new WfControlBLL();//自动化流程服务
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //隐患流程


        #region 获取所有隐患列表接口
        /// <summary>
        /// 获取所有隐患列表接口
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetAllProblems(string json)
        {

            string res = string.Empty;// json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
            if (null == curUser)
            {
                curUser = GetOperator(dy.userid);
                if (curUser == null)
                {
                    return new { code = -1, count = 0, info = "当前用户不存在,请核对用户信息!" };
                }
            }

            string tokenId = dy.tokenid; //设备唯一标识

            int pageSize = int.Parse(dy.pagesize.ToString()); //每页的记录数

            int pageIndex = int.Parse(dy.pageindex.ToString());  //当前页索引

            string action = res.Contains("action") ? dy.data.action : ""; //请求类型

            string deptid = res.Contains("deptid") ? dy.data.deptid : "";  //所属单位

            string qdeptcode = res.Contains("qdeptcode") ? dy.data.qdeptcode : "";  //所属单位编码

            string registerdeptcode = res.Contains("registerdeptcode") ? dy.data.registerdeptcode : "";  //登记编码

            string checkdepart = res.Contains("checkdepart") ? dy.data.checkdepart : ""; //排查单位

            string problemareaid = res.Contains("problemareaid") ? dy.data.problemareaid : ""; //隐患区域

            string safetydetailid = res.Contains("safetydetailid") ? dy.data.safetydetailid : ""; //检查项ID

            string DeviceId = res.Contains("deviceid") ? dy.data.deviceid : ""; //检查项ID

            string districtid = res.Contains("districtid") ? dy.data.districtid : "";  //区域code

            string relevanceid = res.Contains("relevanceid") ? dy.data.relevanceid : "";  //关联应用

            string relevancetype = res.Contains("relevancetype") ? dy.data.relevancetype : "";  //关联应用类型

            string workstream = res.Contains("workstream") ? dy.data.workstream : "";  //流程状态

            string standingtype = res.Contains("standingtype") ? dy.data.standingtype : "";  //台账类型

            string standingmark = res.Contains("standingmark") ? dy.data.standingmark : "";  //台账标记

            string hiddenstatus = res.Contains("hiddenstatus") ? dy.data.hiddenstatus : "";  //隐患状态

            Pagination pagination = new Pagination();
            pagination.p_tablename = "v_basehiddeninfo";
            pagination.p_fields = @"hidrankname,CHANGEDUTYDEPARTNAME,CHANGEPERSONNAME,HIDPOINTNAME";
            pagination.page = pageIndex;
            pagination.rows = pageSize;
            pagination.sidx = "createdate desc";
            pagination.sord = "";
            pagination.conditionJson = " 1=1 ";
            pagination.p_kid = "id";

            //组织机构
            if (curUser != null && !string.IsNullOrEmpty(curUser.OrganizeCode))
            {
                //省级单位
                if (curUser.RoleName.Contains("省级用户"))
                {
                    pagination.conditionJson += string.Format(@" and  deptcode  like '{0}%' ", curUser.NewDeptCode);
                }
                else
                {
                    pagination.conditionJson += string.Format(@" and  hiddepart = '{0}' ", curUser.OrganizeId);
                }
            }

            //区域
            if (!string.IsNullOrEmpty(districtid))
            {
                pagination.conditionJson += string.Format(@" and  hidpoint = '{0}' ", districtid.ToString());
            }
            //流程状态
            if (!string.IsNullOrEmpty(workstream))
            {
                pagination.conditionJson += string.Format(@" and workstream = '{0}'", workstream.ToString());
            }

            #region 隐患状态
            if (!string.IsNullOrEmpty(hiddenstatus))
            {
                switch (hiddenstatus)
                {
                    case "制定整改计划":
                        pagination.conditionJson += @" and workstream = '制定整改计划' ";
                        break;
                    case "逾期未评估":
                        pagination.conditionJson += string.Format(@" and workstream = '隐患评估'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > afterapprovedate", DateTime.Now);
                        break;
                    case "即将到期未评估":
                        pagination.conditionJson += string.Format(@" and workstream = '隐患评估'  and   to_date('{0}','yyyy-mm-dd hh24:mi:ss') >= beforeapprovedate  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= afterapprovedate  ", DateTime.Now);
                        break;
                    case "未整改":
                        pagination.conditionJson += @" and workstream = '隐患整改' ";
                        break;
                    case "逾期未整改":
                        pagination.conditionJson += string.Format(@" and workstream = '隐患整改'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > changedeadine + 1", DateTime.Now);
                        break;
                    case "延期整改":
                        pagination.conditionJson += @" and  problemid in (select distinct hidcode from bis_htextension where handlesign ='1')";
                        break;
                    case "即将到期未整改":
                        pagination.conditionJson += @"and workstream = '隐患整改' and ((hidrankname like  '%一般隐患%'  and changedeadine - 3 <= 
                                                         sysdate  and sysdate <= changedeadine + 1 )  or (hidrankname like '%重大隐患%' and changedeadine - 5 <= sysdate and  sysdate <= changedeadine + 1 ) )";
                        break;
                    case "逾期未验收":
                        pagination.conditionJson += string.Format(@" and workstream = '隐患验收'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > afteracceptdate", DateTime.Now);
                        break;
                    case "即将到期未验收":
                        pagination.conditionJson += string.Format(@" and workstream = '隐患验收'  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') >= beforeacceptdate   and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= afteracceptdate ", DateTime.Now);
                        break;
                    case "本人登记":
                        pagination.conditionJson += string.Format(@" and  createuserid ='{0}'", curUser.UserId);
                        break;
                    case "已整改":
                        pagination.conditionJson += @" and   workstream in ('隐患验收','复查验证','整改效果评估','整改结束')"; //
                        break;
                    case "挂牌督办":
                        pagination.conditionJson += @" and  isgetafter ='1'";
                        break;
                    case "未整改结束":
                        pagination.conditionJson += @" and  workstream !='整改结束'";
                        break;
                    case "未闭环":
                        pagination.conditionJson += @" and  workstream !='整改结束' and  workstream !='隐患评估' ";
                        break;
                    case "已闭环":
                        pagination.conditionJson += @" and  workstream ='整改结束'";
                        break;
                }
            }
            #endregion

            var dt = htbaseinfobll.GetBaseInfoForApp(pagination);
            return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt.ToJson() };

        }

        /// <summary>
        /// 获取预警信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEarlyWarning([FromBody]JObject json)
        {

            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            var type = dy.type;
            if (null == curUser)
            {
                curUser = GetOperator(dy.userid);
                if (curUser == null)
                {
                    return new { code = -1, count = 0, info = "当前用户不存在,请核对用户信息!" };
                }
            }
            string sql = string.Format("select d.changedeadine,d.hidpointname,changemeasure,d.workstream from v_basehiddeninfo d where 1=1 ");

            //组织机构
            if (curUser != null && !string.IsNullOrEmpty(curUser.OrganizeCode))
            {
                //省级单位
                if (curUser.RoleName.Contains("省级用户"))
                {
                    sql += string.Format(@" and  deptcode  like '{0}%' ", curUser.NewDeptCode);
                }
                else
                {

                    sql += string.Format(@" and  hiddepart = '{0}' ", curUser.OrganizeId);
                }
            }
            //时间筛选
            if (!string.IsNullOrEmpty(type) && type == "1")
            {//当天预警信息
                sql += string.Format(" and Changedeadine > to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') and Changedeadine < to_date('{1}', 'yyyy-MM-dd HH24:mi:ss') ", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1));
            }

            //流程状态
            if (!string.IsNullOrEmpty(res))
            {
                sql += string.Format(@" and workstream != '整改结束'");
            }

            DataTable dt = new OperticketmanagerBLL().GetDataTable(sql);
            return new { code = 0, info = "获取数据成功", count = 1, data = dt.ToJson() };
        }

        #endregion

        #region 获取隐患详情信息
        /// <summary>
        /// 获取隐患详情信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetProblemInfo(string json)
        {
            string res = string.Empty; //json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
            if (null == curUser)
            {
                curUser = GetOperator(dy.userid);
                if (curUser == null)
                {
                    return new { code = -1, count = 0, info = "当前用户不存在,请核对用户信息!" };
                }
            }
            string hiddenid = dy.data.hiddenid; //隐患主键
            //var baseInfo = htbaseinfobll.GetHiddenByKeyValue(hiddenid);

            string sql = string.Format(@"select hidname,hiddescribe,changemeasure,changedeadine,changepersonname  from v_basehiddeninfo where id='{0}'", hiddenid);
            DataTable dt = new OperticketmanagerBLL().GetDataTable(sql);
            //HiddenEntity entity = new HiddenEntity();
            //if (dt.Rows.Count > 0)
            //{
            //    entity.hidname = dt.Rows[0][0].ToString();
            //    entity.hiddescribe = dt.Rows[0][1].ToString();
            //    entity.changemeasure = dt.Rows[0][2].ToString();
            //    entity.Changedeadine = dt.Rows[0][3].ToString();
            //    entity.changepersonname = dt.Rows[0][4].ToString();
            //}

            return new { code = 0, count = 0, info = "获取成功", data = dt.ToJson() };
        }

        #endregion

        /// <summary>
        /// 隐患详情序列化实体
        /// </summary>
        public class HiddenEntity
        {
            public string hidname { get; set; }//隐患名称
            public string hiddescribe { get; set; }//隐患描述
            public string changemeasure { get; set; }//整改要求
            public string Changedeadine { get; set; }//整改时限
            public string changepersonname { get; set; }//整改责任人

            public string Committee { get; set; }//摄像头
            public string Email { get; set; }//邮件

        }
     

    }
}
