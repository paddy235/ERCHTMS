using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.DangerousJob;
using ERCHTMS.IService.DangerousJob;
using ERCHTMS.Service.DangerousJob;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ERCHTMS.Busines.DangerousJob
{
    /// <summary>
    /// 描 述：危险作业流程流转表
    /// </summary>
    public class DangerousJobFlowDetailBLL
    {
        private DangerousJobFlowDetailIService service = new DangerousJobFlowDetailService();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();


        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<DangerousJobFlowDetailEntity> GetList()
        {
            return service.GetList();
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
        public DangerousJobFlowDetailEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DataTable GetEntityByT(string keyValue, string fid)
        {
            return service.GetEntityByT(keyValue, fid);
        }
        /// <summary>
        /// 获取当前审核人id
        /// </summary>
        /// <param name="BusinessId"></param>
        /// <param name="flowdetailid">流转表id</param>
        /// <returns></returns>
        public string GetCurrentStepUser(string BusinessId, string flowdetailid)
        {
            return service.GetCurrentStepUser(BusinessId, flowdetailid);
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
        public void SaveForm(string keyValue, DangerousJobFlowDetailEntity entity)
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
        /// 审核保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主表主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void CheckSaveForm(string keyValue, DangerousJobFlowDetailEntity entity)
        {
            try
            {
                service.CheckSaveForm(keyValue, entity);
                JobSafetyCardApplyBLL bll = new JobSafetyCardApplyBLL();
                UserBLL userbll = new UserBLL();
                var data = bll.GetEntity(keyValue);
                if (data.JobState == 2)//审核不通过
                {
                    JPushApi.PushMessage(userbll.GetEntity(data.CreateUserId).Account, data.CreateUserName, "ZYAQZ002", data.JobTypeName + "安全证申请审批未通过，请您知晓。", "您于" + data.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + data.JobTypeName + "安全证审批未通过，如您还需进行作业，请重新进行申请。", data.Id);
                }
                else if (data.JobState == 1)//下一步审批人
                {
                    DangerousJobFlowDetailEntity flow = service.GetList().Where(t => t.BusinessId == keyValue && t.Status == 0).FirstOrDefault();
                    string userids = service.GetCurrentStepUser(keyValue, flow.Id);
                    DataTable dt = userbll.GetUserTable(userids.Split(','));
                    JPushApi.PushMessage(string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("realname")).ToArray()), "ZYAQZ003", data.JobTypeName + "安全证申请待您审批，请您及时处理。", data.CreateUserName + "于" + data.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + data.JobTypeName + "安全证需要您进行审批，请您及时处理。", data.Id);
                }
                else
                {
                    //审批通过
                    DataTable dt = userbll.GetUserTable((data.CreateUserId + "," + data.JobPersonId).Split(','));
                    JPushApi.PushMessage(string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("realname")).ToArray()), "ZYAQZ001", data.JobTypeName + "安全证申请已审批通过，请您查收。", "您于" + data.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + data.JobTypeName + "安全证已审批通过，请您合理安排作业时间。", data.Id);
                    if (data.JobType == "Digging") //动土作业审批通过进行到备案状态 需要发送待备案消息
                    {
                        DataTable dt1 = userbll.GetUserTable(data.RecordsPersonId.Split(','));
                        JPushApi.PushMessage(string.Join(",", dt1.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), data.RecordsPerson, "ZYAQZ005", data.JobTypeName + "安全证申请待您备案，请您及时处理。", data.CreateUserName + "于" + data.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + data.JobTypeName + "安全证需要您进行备案，请您及时处理。", data.Id);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 安全风险审批单审核保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主表主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void ApprovalFormCheckSaveForm(string keyValue, DangerousJobFlowDetailEntity entity)
        {
            try
            {
                service.ApprovalFormCheckSaveForm(keyValue, entity);
                JobApprovalFormBLL bll = new JobApprovalFormBLL();
                UserBLL userbll = new UserBLL();
                var data = bll.GetEntity(keyValue);
                data.JobLevelName = dataitemdetailbll.GetItemName("DangerousJobCheck", data.JobLevel);
                if (data.JobState == 4)//审核不通过
                {
                    JPushApi.PushMessage(userbll.GetEntity(data.CreateUserId).Account, data.CreateUserName, "WXZY002", data.JobLevelName + "审批单(" + data.JobTypeName + ")申请审批未通过，请您知晓。", "您于" + data.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + data.JobLevelName + "审批单(" + data.JobTypeName + ")安全证审批未通过，如您还需进行作业，请重新进行申请。", data.Id);
                }
                else if (data.JobState == 1)//下一步审批人
                {
                    DangerousJobFlowDetailEntity flow = service.GetList().Where(t => t.BusinessId == keyValue && t.Status == 0).FirstOrDefault();
                    string userids = service.GetCurrentStepUser(keyValue, flow.Id);
                    DataTable dt = userbll.GetUserTable(userids.Split(','));
                    JPushApi.PushMessage(string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("realname")).ToArray()), "WXZY001", data.JobLevelName + "审批单(" + data.JobTypeName + ")申请待您审批，请您及时处理。", data.CreateUserName + "于" + data.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + data.JobLevelName + "审批单(" + data.JobTypeName + ")需要您进行审批，请您及时处理。", data.Id);
                }
                else
                {
                    //审批通过
                    DataTable dt = userbll.GetUserTable((data.CreateUserId + "," + data.JobPersonId).Split(','));
                    JPushApi.PushMessage(string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("realname")).ToArray()), "WXZY003", data.JobLevelName + "审批单(" + data.JobTypeName + ")申请已审批通过，请您查收。", "您于" + data.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + data.JobLevelName + "审批单(" + data.JobTypeName + ")已审批通过，请您合理安排作业时间。", data.Id);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
