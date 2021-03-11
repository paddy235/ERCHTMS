using BSFramework.Util.WebControl;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Code;
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
    /// <summary>
    /// 安全风险管控（风险四色）
    /// </summary>
    public class VRRiskController : BaseApiController
    {
        private RiskAssessBLL riskassessbll = new RiskAssessBLL();

        /// <summary>
        /// 获取安全风险清单列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getRiskIdentifyList(string json)
        {
            try
            {
                string res = string.Empty;// json.Value<string>("json");//[FromBody]JObject
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                string userId = dy.userid;
                string riskType = dy.data.riskType;//风险类别
                string grade = dy.data.grade;//风险等级 
                string areaCode = dy.data.areaCode;//关键字查询
                string keyWord = dy.data.keyWord;//关键字查询

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    curUser = GetOperator(dy.userid);
                    if (curUser == null)
                    {
                        return new { code = -1, count = 0, info = "当前用户不存在,请核对用户信息!" };
                    }
                }

                Pagination pagination = new Pagination();
                pagination.p_kid = "id";
                pagination.p_fields = @"areaid,risktype,dangersource,riskdesc,itemr,grade,
accidentname,result,createuserid,createuserdeptcode,jobname,toolordanger,dangersourcetype,hjsystem,hjequpment,
createuserorgcode,deptname,postname,districtname,
deptcode,planid,createdate,MajorName,Description,
worktask,process,equipmentname,parts,levelname,1 as faultordanger,faulttype,project,dutyperson,dutypersonid,element,faultcategory,majornametype,
                                        packuntil,packnum,storagespace,postdept,postdeptid,postperson,postpersonid,postdeptcode,'' f1,'' f2,'' f3,'' f4,'' f5 ";
                pagination.p_tablename = "bis_riskassess";
                pagination.conditionJson = "status=1 and deletemark=0 and enabledmark=0";
                //pagination.sidx = pagination.sidx + " " + pagination.sord + ",id";

                //部门查询
                if (!curUser.IsSystem)
                {
                    pagination.conditionJson += " and deptcode like '" + curUser.OrganizeCode + "%'";
                }
                //风险类别
                if (!string.IsNullOrEmpty(riskType))
                {
                    pagination.conditionJson += string.Format(" and risktype ='{0}'", riskType);
                }
                //风险等级
                if (!string.IsNullOrEmpty(grade))
                {
                    string strgrade = string.Empty;
                    switch (grade)
                    {
                        case "1":
                            strgrade = "重大风险";
                            break;
                        case "2":
                            strgrade = "较大风险";
                            break;
                        case "3":
                            strgrade = "一般风险";
                            break;
                        case "4":
                            strgrade = "低风险";
                            break;
                        default:
                            break;
                    }
                   // pagination.conditionJson += string.Format(" and grade ='{0}'", strgrade);
                }

                //风险区域
                if (!string.IsNullOrEmpty(areaCode))
                {
                    pagination.conditionJson += string.Format(" and areacode like '{0}%'", areaCode);
                }

                //查询关键字
                if (!string.IsNullOrEmpty(keyWord))
                {
                    pagination.conditionJson += string.Format(" and (Description like '%{0}%' or riskdesc like '%{0}%' or majorname like '%{0}%' or worktask like '%{0}%' or equipmentname like '%{0}%' or dangersource like '%{0}%') ", keyWord.Trim());
                }

                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pagenum), rows = Convert.ToInt32(dy.data.pagesize);
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "createdate desc,id desc,id";//排序字段
                pagination.sord = "desc";//排序方式
                DataTable data = riskassessbll.GetPageList(pagination, null);
                GrandEntity entity = GetGradeCount(curUser, areaCode);
                return new { code = 0, info = "获取数据成功", count = pagination.records, Grade1 = entity.Grade1, Grade2 = entity.Grade2, Grade3 = entity.Grade3, Grade4 = entity.Grade4, data = data.ToJson() };

            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }

        /// <summary>
        /// 获取按照风险等级统计数据
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public GrandEntity GetGradeCount(Operator user,string areaCode)
        {
            string sql = string.Format(@"select rownum,
            (select count(1) from bis_riskassess d where d.grade='重大风险' and  status=1 and deletemark=0 and enabledmark=0 and (risktype='设备' or risktype='管理' or risktype='区域') and deptcode like '{0}%' and areacode like '{1}%'),
            (select count(1) from bis_riskassess d where d.grade='较大风险'and  status=1 and deletemark=0 and enabledmark=0 and (risktype='设备' or risktype='管理' or risktype='区域') and deptcode like '{0}%' and areacode like '{1}%'),
            (select count(1) from bis_riskassess d where d.grade='一般风险'and  status=1 and deletemark=0 and enabledmark=0 and (risktype='设备' or risktype='管理' or risktype='区域') and deptcode like '{0}%' and areacode like '{1}%') ,
            (select count(1) from bis_riskassess d where d.grade='低风险' and  status=1 and deletemark=0 and enabledmark=0 and (risktype='设备' or risktype='管理' or risktype='区域') and deptcode like '{0}%' and areacode like '{1}%') 
            from bis_riskassess t  where rownum=1 ", user.OrganizeCode,areaCode);
            DataTable dt = new OperticketmanagerBLL().GetDataTable(sql);
             GrandEntity entity = new GrandEntity();
             if (dt.Rows.Count > 0)
             {
                 entity.Grade1 = dt.Rows[0][1].ToString();
                 entity.Grade2 = dt.Rows[0][2].ToString();
                 entity.Grade3 = dt.Rows[0][3].ToString();
                 entity.Grade4 = dt.Rows[0][4].ToString();
             }
             dt.Dispose();
            return entity;
        }

        /// <summary>
        /// 风险等级序列化实体
        /// </summary>
        public class GrandEntity
        {
            /// <summary>
            /// 重大风险
            /// </summary>
            public string Grade1 { get; set; }
            /// <summary>
            /// 较大风险
            /// </summary>
            public string Grade2 { get; set; }
            /// <summary>
            ///一般风险 
            /// </summary>
            public string Grade3 { get; set; }
            /// <summary>
            /// 低风险
            /// </summary>
            public string Grade4 { get; set; }
        }



    


    }
}
