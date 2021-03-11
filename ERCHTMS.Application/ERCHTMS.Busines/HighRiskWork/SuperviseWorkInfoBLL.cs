using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：旁站监督作业信息
    /// </summary>
    public class SuperviseWorkInfoBLL
    {
        private SuperviseWorkInfoIService service = new SuperviseWorkInfoService();

        #region 获取数据
        /// <summary>
        /// 根据分配任务id获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SuperviseWorkInfoEntity> GetList(string strwhere)
        {
            return service.GetList(strwhere);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SuperviseWorkInfoEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

         /// <summary>
        /// 根据分配任务id和班组id获取作业信息
        /// </summary>
        /// <param name="taskshareid"></param>
        /// <returns></returns>
        public IEnumerable<SuperviseWorkInfoEntity> GetList(string taskshareid, string teamid)
        {
            return service.GetList(taskshareid,teamid);
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
        public void SaveForm(string keyValue, SuperviseWorkInfoEntity entity)
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
        ///根据分配id删除作业信息
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveWorkByTaskShareId(string keyValue)
        {
            try
            {
                service.RemoveWorkByTaskShareId(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
