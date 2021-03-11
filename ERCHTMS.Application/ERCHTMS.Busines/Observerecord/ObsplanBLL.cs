using ERCHTMS.Entity.Observerecord;
using ERCHTMS.IService.Observerecord;
using ERCHTMS.Service.Observerecord;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.IService.Observerecord;

namespace ERCHTMS.Busines.Observerecord
{
    /// <summary>
    /// 描 述：观察计划
    /// </summary>
    public class ObsplanBLL
    {
        private ObsplanIService service = new ObsplanService();
        private ObsplanEHSIService serviceEHS = new ObsEHSplanService();
        private ObsplanFBIService serviceFB = new ObsFBplanService();
        private ObsplanTZIService serviceTZ = new ObsTZplanService();
        private ObsFeedBackIService obsfeedback = new ObsFeedBackService();
        private ObsFeedBackEHSIService obsfeedbackehs = new ObsFeedBackEHSService();
        private ObsFeedBackFBIService obsfeedbackfb = new ObsFeedBackFBService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取意见列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
     
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ObsplanEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ObsplanEHSEntity GetEHSEntity(string keyValue)
        {
            return serviceEHS.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ObsplanFBEntity GetFBEntity(string keyValue)
        {
            return serviceFB.GetEntity(keyValue);
        }

        public ObsplanTZEntity GetTZEntity(string keyValue)
        {
            return serviceTZ.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ObsplanEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 根据观察计划Id与任务分解Id获取相应信息
        /// </summary>
        /// <param name="PlanId">计划id </param>
        /// <param name="PlanFjId">任务分解Id</param>
        /// <returns></returns>
        public DataTable GetPlanById(string PlanId, string PlanFjId, string month)
        {
            return service.GetPlanById(PlanId, PlanFjId, month);
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
        public void SaveForm(string keyValue, ObsplanEntity entity)
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
        public void SaveEHSForm(string keyValue, ObsplanEHSEntity entity)
        {
            try
            {
                serviceEHS.SaveForm(keyValue, entity);
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
        public void SaveFBForm(string keyValue, ObsplanFBEntity entity)
        {
            try
            {
                serviceFB.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CommitEhsData(Operator currUser)
        {
            return service.CommitEhsData(currUser);
        }
        #endregion


        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveFeedBackForm(string keyValue, ObsFeedBackEntity entity)
        {
            try
            {
                obsfeedback.SaveForm(keyValue, entity);
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
        public void SaveFeedBackEHSForm(string keyValue, ObsFeedBackEHSEntity entity)
        {
            try
            {
                obsfeedbackehs.SaveForm(keyValue, entity);
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
        public void SaveFeedBackFBForm(string keyValue, ObsFeedBackFBEntity entity)
        {
            try
            {
                obsfeedbackfb.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 查询本年度该观察计划那些月份进行了观察记录
        /// </summary>
        /// <param name="planId">计划Id</param>
        /// <param name="planfjid">计划任务分解Id</param>
        /// <param name="year">年度</param>
        /// <returns></returns>
        public DataTable GetObsRecordIsExist(string planId, string planfjid, string year) {
            try
            {
                return service.GetObsRecordIsExist(planId, planfjid,year);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 导入模板数据-部门级
        /// </summary>
        /// <param name="obsplan"></param>
        /// <param name="obsplanwork"></param>
        public void InsertImportData(List<ObsplanEntity> obsplan, List<ObsplanworkEntity> obsplanwork) {
            try
            {
                service.InsertImportData(obsplan, obsplanwork);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 导入模板数据-Ehs部
        /// </summary>
        /// <param name="obsplan"></param>
        /// <param name="obsplanwork"></param>
        public void InsertImportData(List<ObsplanEHSEntity> obsplan, List<ObsplanworkEntity> obsplanwork)
        {
            try
            {
                service.InsertImportData(obsplan, obsplanwork);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 复制年度计划
        /// </summary>
        /// <param name="currUser">当前用户</param>
        /// <param name="oldYear">复制的年度</param>
        /// <param name="newYear">复制到的年度</param>
        /// <returns></returns>
        public bool CopyHistoryData(Operator currUser, string oldYear, string newYear) {

            return service.CopyHistoryData(currUser, oldYear, newYear);
        }
        /// <summary>
        /// 只修改计划月份直接同步到EHS与发布的数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SynchData(string planid, string planfjid) {

            service.SynchData(planid, planfjid);
        }
    }
}
