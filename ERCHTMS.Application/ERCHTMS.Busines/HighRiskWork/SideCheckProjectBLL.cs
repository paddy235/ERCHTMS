using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：监督任务检查项目
    /// </summary>
    public class SideCheckProjectBLL
    {
        private SideCheckProjectIService service = new SideCheckProjectService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SideCheckProjectEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SideCheckProjectEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 大项检查项目信息
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public IEnumerable<SideCheckProjectEntity> GetBigCheckInfo()
        {
            return service.GetBigCheckInfo();
        }

        /// <summary>
        /// 根据大项检查项目id获取小项检查项目
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public IEnumerable<SideCheckProjectEntity> GetAllSmallCheckInfo(string parentid)
        {
            return service.GetAllSmallCheckInfo(parentid);
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
        public void SaveForm(string keyValue, SideCheckProjectEntity entity)
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
