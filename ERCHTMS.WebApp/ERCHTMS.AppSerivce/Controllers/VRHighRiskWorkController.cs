using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BSFramework.Util;
using ERCHTMS.Code;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BSFramework.Util.WebControl;
using System.Data;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System.IO;
using System.Web;
using System.Text;
using WebApi.Controllers;



namespace ERCHTMS.AppSerivce.Controllers
{
    /// <summary>
    /// 高风险作业（三维）
    /// </summary>
    [HandlerLogin(LoginMode.Enforce)]
    public class VRHighRiskWorkController : BaseApiController
    {

        private HighRiskCommonApplyBLL highriskcommonapplybll = new HighRiskCommonApplyBLL();
        private ScaffoldBLL scaffoldbll = new ScaffoldBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private HighProjectSetBLL highprojectsetbll = new HighProjectSetBLL();
        private HighImportTypeBLL highimporttypebll = new HighImportTypeBLL();
        private ScaffoldprojectBLL scaffoldprojectbll = new ScaffoldprojectBLL();
        private ScaffoldauditrecordBLL scaffoldauditrecordbll = new ScaffoldauditrecordBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private HighRiskRecordBLL highriskrecordbll = new HighRiskRecordBLL();
        private UserBLL userBLL = new UserBLL();

        
        #region 获取通用作业列表
        /// <summary>
        /// 5.获取通用作业列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetWorkCommonList(string json)
        {
            try
            {
                string res = string.Empty;// json.Value<string>("json");[FromBody]JObject 
                //Stream s = System.Web.HttpContext.Current.Request.InputStream;//字符流
                //StreamReader reader = new StreamReader(s, Encoding.UTF8);
                //string json = reader.ReadToEnd();
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                //获取用户Id
                string userId = dy.userid;
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

              

                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pagenum), rows = Convert.ToInt32(dy.data.pagesize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "a.Id as applyid";
                pagination.p_fields = " RiskType,WorkDutyUserName,c.itemname as applystatename,a.applystate,a.workdepttype,case when a.workdepttype=0 then '单位内部'  when  a.workdepttype=1 then '外包单位' end workdepttypename,a.workdeptname,b.itemname as worktype,workplace,workcontent,to_char(workstarttime,'yyyy-mm-dd hh24:mi') as workstarttime,to_char(workendtime,'yyyy-mm-dd hh24:mi') as workendtime,applyusername,'' as approveusername,a.flowdept,a.flowrolename,a.specialtytype,a.flowremark";
                pagination.p_tablename = " bis_highriskcommonapply a left join base_dataitemdetail b on a.worktype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='CommonType') left join base_dataitemdetail c on a.applystate=c.itemvalue and c.itemid =(select itemid from base_dataitem where itemcode='CommonStatus')";
                pagination.conditionJson = " a.applystate='5' and RealityWorkStartTime is not null and RealityWorkEndTime is null";
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                DataTable dt = highriskcommonapplybll.GetPageDataTable(pagination, null);
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt.ToJson() };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }

        #endregion

        #region 获取高风险通用作业详情
        /// <summary>
        /// 7.获取高风险通用作业详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object WorkCommonDetail(string json)
        {
            try
            {
                string res = string.Empty;// json.Value<string>("json");[FromBody]JObject
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
                //获取用户Id
                string userId = dy.userid;
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
                string applyid = dy.data.applyid;//通用申请id
                var commonentity = new HighRiskCommonApplyBLL().GetEntity(applyid);
                List<HighRiskEntity> list = new List<HighRiskEntity>();
                //风险等级、作业类型
                var data = dataitemdetailbll.GetDataItemListByItemCode("'CommonRiskType'").Where(a => a.ItemValue == commonentity.RiskType).FirstOrDefault();
                var data2 = dataitemdetailbll.GetDataItemListByItemCode("'CommonType'").Where(a => a.ItemValue == commonentity.WorkType).FirstOrDefault();

                if (commonentity != null && !string.IsNullOrEmpty(commonentity.WorkUserIds))
                {//作业人员
                    string[] users = commonentity.WorkUserIds.Split(',');
                    foreach (string rows in users)
                    {
                        if (string.IsNullOrEmpty(rows)) continue;
                        UserEntity user = userBLL.GetEntity(rows);
                        if (user == null) continue;
                        HighRiskEntity entity = new HighRiskEntity();
                        entity.RiskType = data.ItemName;
                        entity.EngineeringName = commonentity.EngineeringName;
                        entity.WorkPlace = commonentity.WorkPlace;
                        entity.WorkDeptName = commonentity.WorkDeptName;
                        entity.WorkContent = commonentity.WorkContent;
                        entity.ElectronPen = "正常";
                        entity.WorkDutyUserName = commonentity.WorkDutyUserName;
                        entity.EnCode = user.EnCode;
                        entity.RealName = user.RealName;
                        entity.Gender = user.Gender;
                        entity.ApplyDeptName = commonentity.ApplyDeptName;
                        entity.WorkType = data2.ItemName;
                        entity.Mobile = user.Mobile;
                        entity.Place = commonentity.WorkPlace;
                        entity.HeadIcon = webUrl + user.HeadIcon;
                        list.Add(entity);
                    }
                }

                return new
                {
                    code = 0,
                    count = 1,
                    info = "获取数据成功",
                    data = list.ToJson()
                };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }

        #endregion


        /// <summary>
        /// 高风险详情序列化实体
        /// </summary>
        public class HighRiskEntity
        {
            public string RiskType { get; set; }//风险等级
            public string EngineeringName { get; set; }//作业名称
            public string WorkPlace { get; set; }//作业位置
            public string WorkDeptName { get; set; }//作业部门
            public string WorkContent { get; set; }//作业要求
            public string ElectronPen { get; set; }//电子围栏
            public string WorkDutyUserName { get; set; }//作业负责人

            public string HeadIcon { get; set; }//人员头像
            public string EnCode { get; set; }//工号
            public string RealName { get; set; }//姓名
            public string Gender { get; set; }//性别
            public string ApplyDeptName { get; set; }//单位
            public string Place { get; set; }//位置
            public string WorkType { get; set; }//作业
            public string Mobile { get; set; }//电话


            public string Committee { get; set; }//摄像头
            public string Email { get; set; }//邮件

        }



    }

}
