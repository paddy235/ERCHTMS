using ERCHTMS.Entity.LaborProtectionManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.LaborProtectionManage
{
    /// <summary>
    /// 描 述：劳动防护用品表
    /// </summary>
    public interface LaborinfoIService
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
        IEnumerable<LaborinfoEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        LaborinfoEntity GetEntity(string keyValue);

        /// <summary>
        /// 根据ids获取批量发放所需数据
        /// </summary>
        /// <param name="InfoId"></param>
        /// <returns></returns>
        DataTable Getplff(string InfoId);

        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetTable(string queryJson, string where);
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
        void ImportSaveForm(List<LaborinfoEntity> entity, List<LaborprotectionEntity> prolist,List<LaborequipmentinfoEntity> eqlist);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, LaborinfoEntity entity, string json, string ID);
        #endregion
    }
}
