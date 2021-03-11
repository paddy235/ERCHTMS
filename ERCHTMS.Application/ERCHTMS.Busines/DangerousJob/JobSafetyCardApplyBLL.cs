using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.IService.DangerousJob;
using ERCHTMS.Service.DangerousJob;
using ERCHTMS.Entity.DangerousJob;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.JPush;
using System.Linq;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Code;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Busines.DangerousJob
{
    /// <summary>
    /// 描 述：作业安全证申请
    /// </summary>
    public class JobSafetyCardApplyBLL
    {
        private JobSafetyCardApplyIService service = new JobSafetyCardApplyService();
        private DangerousJobFlowDetailIService DangerousJobFlowDetailIService = new DangerousJobFlowDetailService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<JobSafetyCardApplyEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public JobSafetyCardApplyEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DataTable ConfigurationByWorkList(string keyValue, string moduleno)
        {
            return service.ConfigurationByWorkList(keyValue, moduleno);
        }

        public DataTable GetPageView(Pagination pagination, string queryJson)
        {
            return service.GetPageView(pagination, queryJson);
        }

        public DataTable FindTable(string sql)
        {
            return service.FindTable(sql);
        }
        public string getName( string encode,string detailvalue,string detailencode)
        {
            string strName = "";
            var entity = new DataItemDetailBLL().GetDataItemListByItemCode("'" + encode + "'").Where(a => a.ItemValue == detailvalue && a.ItemCode == detailencode).FirstOrDefault();
            if (entity != null)
                strName = entity.ItemName;
            return strName;
        }

        /// <summary>
        /// 获取今日高危作业列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTodayWorkList(Pagination pagination, string queryJson)
        {
            return service.GetTodayWorkList(pagination, queryJson);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, JobSafetyCardApplyEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, JobSafetyCardApplyEntity entity, List<ManyPowerCheckEntity> data,string arr, string arrData)
        {
            try
            {
                service.SaveForm(keyValue, entity, data, arr,arrData);
                entity = GetEntity(keyValue);
                if (entity.IsSubmit == 1)
                {
                    UserBLL userbll = new UserBLL();
                    if (entity.JobType == "LimitedSpace" || entity.JobType == "EquOverhaulClean")
                    {
                        DataTable dt = userbll.GetUserTable(entity.MeasurePersonId.Split(','));
                        JPushApi.PushMessage(string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), entity.MeasurePerson, "ZYAQZ007", entity.JobTypeName + "安全证待您进行安全措施确认，请您及时处理。", entity.CreateUserName + "于" + entity.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + entity.JobTypeName + "安全证需要您进行本部门的安全措施确认，请您及时处理。", entity.Id);
                    }
                    else
                    {
                        DangerousJobFlowDetailEntity flow = DangerousJobFlowDetailIService.GetList().Where(t => t.BusinessId == entity.Id && t.Status == 0).FirstOrDefault();
                        string userids = DangerousJobFlowDetailIService.GetCurrentStepUser(entity.Id, flow.Id);
                        DataTable dt = userbll.GetUserTable(userids.Split(','));
                        JPushApi.PushMessage(string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("realname")).ToArray()), "ZYAQZ003", entity.JobTypeName + "安全证申请待您审批，请您及时处理。", entity.CreateUserName + "于" + entity.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + entity.JobTypeName + "安全证需要您进行审批，请您及时处理。", entity.Id);
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 变更操作人
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="TransferUserName"></param>
        /// <param name="TransferUserAccount"></param>
        /// <param name="TransferUserId"></param>
        public void ExchangeForm(string keyValue, string TransferUserName, string TransferUserAccount, string TransferUserId, Operator user)
        {
            service.ExchangeForm(keyValue, TransferUserName, TransferUserAccount, TransferUserId);
            
            var entity = service.GetEntity(keyValue);
            JPushApi.PushMessage(TransferUserAccount, TransferUserName, "ZYAQZ003", entity.JobTypeName + "安全证的流程处理人已转交给您，请您及时处理。", entity.JobTypeName + "安全证的流程处理人已由" + user.UserName + "转交给您，请您及时处理。", entity.Id);
            //switch (entity.JobState)
            //{
            //    case 1: //审核中状态更改流程基础表跟流程流转表的审核人信息
            //        JPushApi.PushMessage(TransferUserAccount, TransferUserName, "ZYAQZ003", entity.JobTypeName + "安全证申请待您审批，请您及时处理。", entity.CreateUserName + "于" + entity.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + entity.JobTypeName + "安全证需要您进行审批，请您及时处理。", entity.Id);
            //        break;
            //    case 3: //措施确认中状态
            //        JPushApi.PushMessage(TransferUserAccount, TransferUserName, "ZYAQZ007", entity.JobTypeName + "安全证待您进行安全措施确认，请您及时处理。", entity.CreateUserName + "于" + entity.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + entity.JobTypeName + "安全证需要您进行本部门的安全措施确认，请您及时处理。", entity.Id);
            //        break;
            //    case 4: //停电中状态
            //        JPushApi.PushMessage(TransferUserAccount, TransferUserName, "ZYAQZ008", entity.JobTypeName + "安全证待您进行停电，请您及时处理。", entity.CreateUserName + "于" + entity.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + entity.JobTypeName + "安全证需要您进行停电操作，请您及时处理。", entity.Id);
            //        break;
            //    case 5: //备案中状态
            //        JPushApi.PushMessage(TransferUserAccount, TransferUserName, "ZYAQZ005", entity.JobTypeName + "安全证申请待您备案，请您及时处理。", entity.CreateUserName + "于" + entity.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + entity.JobTypeName + "安全证需要您进行备案，请您及时处理。", entity.Id);
            //        break;
            //    case 6: //验收中状态
            //        JPushApi.PushMessage(TransferUserAccount, TransferUserName, "ZYAQZ004", entity.JobTypeName + "安全证待您进行作业后验收，请您及时处理。", entity.CreateUserName + "于" + entity.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + entity.JobTypeName + "安全证需要您进行作业后验收，请您及时处理。", entity.Id);
            //        break;
            //    case 7: //送电中
            //        JPushApi.PushMessage(TransferUserAccount, TransferUserName, "ZYAQZ010", entity.JobTypeName + "已结束，待您进行送电，请您及时处理。", entity.CreateUserName + "于" + entity.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + entity.JobTypeName + "已结束，需要您进行送电操作，请您及时处理。", entity.Id);
            //        break;
            //    default:
            //        break;
            //}
        }

        /// <summary>
        /// 获取流程图
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public Flow GetFlow(string KeyValue)
        {
            return service.GetFlow(KeyValue);
        }
        /// <summary>
        /// 获取手机流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public List<CheckFlowData> GetAppFlowList(string keyValue)
        {
            return service.GetAppFlowList(keyValue);
        }
        #endregion

        public string GetDangerousJobCount(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.GetDangerousJobCount(starttime, endtime, deptid, deptcode);
        }

        public string GetDangerousJobList(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.GetDangerousJobList(starttime, endtime, deptid, deptcode);
        }

    }
}
