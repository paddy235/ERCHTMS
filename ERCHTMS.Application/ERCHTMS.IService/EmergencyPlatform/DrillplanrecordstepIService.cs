using ERCHTMS.Entity.EmergencyPlatform;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急演练记录步骤表
    /// </summary>
    public interface DrillplanrecordstepIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<DrillplanrecordstepEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DrillplanrecordstepEntity GetEntity(string keyValue);

        /// <summary>
        /// 应急记录步骤列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 根据recid获取步骤列表
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        IList<DrillplanrecordstepEntity> GetListByRecid(string recid);
        #endregion

        #region 提交数据

        /// <summary>
        /// 根据关联ID删除数据
        /// </summary>
        /// <param name="recid"></param>
        void RemoveFormByRecid(string recid);

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
        void SaveForm(string keyValue, DrillplanrecordstepEntity entity);
        #endregion
    }
}
