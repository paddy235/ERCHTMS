using ERCHTMS.Entity.OccupationalHealthManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病人员表
    /// </summary>
    public interface OccupatioalstaffIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<OccupatioalstaffEntity> GetList(string queryJson);

        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetTable(string queryJson,string where);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        OccupatioalstaffEntity GetEntity(string keyValue);

        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageListByProc(Pagination pagination, string queryJson);
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
        void SaveForm(string keyValue, OccupatioalstaffEntity entity);

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="IsNew">是否新增</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(bool IsNew, OccupatioalstaffEntity entity);
        #endregion
    }
}
