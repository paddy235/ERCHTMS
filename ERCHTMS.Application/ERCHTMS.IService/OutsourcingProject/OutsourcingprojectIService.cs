using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包单位基础信息表
    /// </summary>
    public interface OutsourcingprojectIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<OutsourcingprojectEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        OutsourcingprojectEntity GetEntity(string keyValue);
        /// <summary>
        /// 根据外包单位Id获取外包单位基础信息
        /// </summary>
        /// <param name="outProjectId"></param>
        /// <returns></returns>
        OutsourcingprojectEntity GetInfo(string outProjectId);
        DataTable GetPageList(Pagination pagination, string queryJson);

        string StaQueryList(string queryJson);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, OutsourcingprojectEntity entity);
        #endregion
    }
}
