using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包单位基础信息表
    /// </summary>
    public class OutsourcingprojectBLL
    {
        private OutsourcingprojectIService service = new OutsourcingprojectService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OutsourcingprojectEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OutsourcingprojectEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public DataTable GetPageList(Pagination pagination, string queryJson){
            return service.GetPageList(pagination,queryJson);
        }
        /// <summary>
        /// 根据外包单位Id获取外包单位基础信息
        /// </summary>
        /// <param name="outProjectId"></param>
        /// <returns></returns>
        public OutsourcingprojectEntity GetOutProjectInfo(string outProjectId)
        {
            return service.GetInfo(outProjectId);
        }
        public string StaQueryList(string queryJson) {
            return service.StaQueryList(queryJson);
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
        public void SaveForm(string keyValue, OutsourcingprojectEntity entity)
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
