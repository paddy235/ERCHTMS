using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Entity.JTSafetyCheck;
using System.Data;

namespace ERCHTMS.IService.JTSafetyCheck
{
    /// <summary>
    /// 描 述：康巴什门禁管理
    /// </summary>
    public interface JTSafetyCheckIService
    {
        #region 获取数据

        /// <summary>
        /// 获取全部列表数据
        /// </summary>
        /// <returns>返回分页列表</returns>
        List<SafetyCheckEntity> GetPageList();
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SafetyCheckEntity> GetList(string queryJson);
        DataTable GetItemsList(string checkId,string status);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SafetyCheckEntity GetEntity(string keyValue);
        CheckItemsEntity GetItemEntity(string keyValue);
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
        bool SaveForm(string keyValue, SafetyCheckEntity entity);
        void SaveItemForm(string keyValue, CheckItemsEntity entity);
        /// <summary>
        /// 接口修改状态用方法
        /// </summary>
        /// <param name="entity"></param>
        void RemoveItemForm(string keyValue);
        bool Save(string keyValue, SafetyCheckEntity entity, List<CheckItemsEntity> items);
        bool SaveItems(List<CheckItemsEntity> items);

        #endregion
    }
}
