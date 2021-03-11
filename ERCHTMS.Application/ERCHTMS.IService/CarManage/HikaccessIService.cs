using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// 描 述：海康门禁间设备管理
    /// </summary>
    public interface HikaccessIService
    {
        #region 获取数据

        /// <summary>
        /// 用户列表
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
        IEnumerable<HikaccessEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        HikaccessEntity GetEntity(string keyValue);

        /// <summary>
        /// 根据hikID获取设备信息
        /// </summary>
        /// <param name="HikId"></param>
        /// <returns></returns>
        HikaccessEntity HikGetEntity(string HikId);
        #endregion

        #region 提交数据

        /// <summary>
        /// 门禁状态反控
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="type"></param>
        /// <param name="pitem"></param>
        /// <param name="url"></param>
        void ChangeControl(string keyValue, int type, string pitem, string url);
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
        void SaveForm(string keyValue, HikaccessEntity entity);
        #endregion
    }
}
