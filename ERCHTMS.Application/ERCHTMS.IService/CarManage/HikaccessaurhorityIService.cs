using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// 描 述：门禁点权限表
    /// </summary>
    public interface HikaccessaurhorityIService
    {
        #region 获取数据

        /// <summary>
        /// 权限列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<HikaccessaurhorityEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        HikaccessaurhorityEntity GetEntity(string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue, string pitem, string url);

        /// <summary>
        /// 根据用户删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveUserForm(string keyValue, string pitem, string url);

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string StartTime, string EndTime, List<Access> DeptList, List<Access> AccessList, int Type,
            string pitem, string url);

        #endregion
    }
}
