using ERCHTMS.Entity.FireManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Collections;

namespace ERCHTMS.IService.FireManage
{
    /// <summary>
    /// 描 述：消防设施
    /// </summary>
    public interface FirefightingIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<FirefightingEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        FirefightingEntity GetEntity(string keyValue);

        DataTable StatisticsData(string queryJson);
        #endregion

        #region 提交数据
        /// <summary>
        /// 根据id数组批量删除
        /// </summary>
        /// <param name="Ids"></param>
        void Remove(string Ids);
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
        void SaveForm(string keyValue, FirefightingEntity entity);
        #endregion

        /// <summary>
        /// 同一类型，编号不能重复
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Code"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        bool ExistCode(string Type, string Code, string keyValue);
        IList GetCountByArea(List<string> areaCodes);
    }
}
