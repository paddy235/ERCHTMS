using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// 描 述：开收工会
    /// </summary>
    public class WorkMeetingBLL
    {
        private WorkMeetingIService service = new WorkMeetingService();

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

        public DataTable GetTable(string sql) {
            return service.GetTable(sql);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WorkMeetingEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WorkMeetingEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 根据当前登录人获取未提交的数据
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetNotCommitData(string userid) {
            return service.GetNotCommitData(userid);
        }
        /// <summary>
        /// 获取今日开工会的临时工程数
        /// </summary>
        /// <returns></returns>
        public int GetTodayTempProject(Operator curUser)
        {
            return service.GetTodayTempProject(curUser);
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
        public void SaveForm(string keyValue, WorkMeetingEntity entity)
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

        public void SaveWorkMeetingForm(string keyValue, WorkMeetingEntity entity, List<WorkmeetingmeasuresEntity> list, string ids)
        {
            try
            {
                service.SaveWorkMeetingForm(keyValue, entity, list,ids);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
