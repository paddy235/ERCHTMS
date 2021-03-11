using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：已检查的检查项目
    /// </summary>
    public class TaskRelevanceProjectBLL
    {
        private TaskRelevanceProjectIService service = new TaskRelevanceProjectService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TaskRelevanceProjectEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TaskRelevanceProjectEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 根据监督任务获取已检查项目
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public IEnumerable<TaskRelevanceProjectEntity> GetEndCheckInfo(string superviseid)
        {
            return service.GetEndCheckInfo(superviseid);
        }

        /// <summary>
        /// 根据检查项目id和监督任务id获取信息
        /// </summary>
        /// <returns></returns>
        public TaskRelevanceProjectEntity GetCheckResultInfo(string checkprojectid, string superviseid)
        {
            return service.GetCheckResultInfo(checkprojectid, superviseid);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageDataTable(Pagination pagination)
        {
            return service.GetPageDataTable(pagination);
        }

        /// <summary>
        /// 根据监督id获取隐患信息
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetTaskHiddenInfo(string superviseid)
        {
            return service.GetTaskHiddenInfo(superviseid);
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
        public void SaveForm(string keyValue, TaskRelevanceProjectEntity entity)
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
        #endregion
    }
}
