using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.SystemManage
{
    /// <summary>
    /// 描 述：数据字典明细
    /// </summary>
    public interface IDataItemDetailService
    {
        #region 获取数据
        IEnumerable<DataItemModel> GetDataItem(string itemCode, string itemDetailCode);

        IEnumerable<DataItemModel> GetDataItemByDetailCode(string itemCode, string itemDetailCode);
        /// <summary>
        /// 明细列表
        /// </summary>
        /// <param name="itemId">分类Id</param>
        /// <returns></returns>
        IEnumerable<DataItemDetailEntity> GetList(string itemId);

        DataItemDetailEntity GetEntityByItemName(string itemName);

        DataTable GetListByCode(string itemCode);
        /// <summary>
        /// 明细实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DataItemDetailEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取数据字典列表（给绑定下拉框提供的）
        /// </summary>
        /// <returns></returns>
        IEnumerable<DataItemModel> GetDataItemList();

        /// <summary>
        /// 通过编码获取
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        IEnumerable<DataItemModel> GetDataItemListByItemCode(string itemCode);

        /// <summary>
        /// 通过编码项
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        IEnumerable<DataItemDetailEntity> GetListItems(string category);

        /// <summary>
        /// 根据编码名称获取编码值
        /// </summary>
        /// <param name="itemName">编码名称</param>
        /// <returns></returns>
        string GetItemValue(string itemName);

        string GetItemCode(string itemName);

        string GetItemValue(string itemName, string itemcode);
        List<DataItemDetailEntity> ListByItemCode(string itemCode);

        /// <summary>
        /// 通过编码类别名称获取编码
        /// </summary>
        /// <param name="itemName">编码类别名称</param>
        /// <returns></returns>
        DataTable GetDataItemListByItemName(string itemName);
        DataTable GetListByItemCode(string itemCode);

        DataItemDetailEntity GetListByItemCodeEntity(string itemCode);

        /// <summary>
        /// 根据编码值获取编码名称
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="itemValue"></param>
        /// <returns></returns>
        string GetItemName(string itemCode, string itemValue);

        /// <summary>
        /// 根据编码名称获取有效的编码值
        /// </summary>
        /// <param name="itemName">编码名称</param>
        /// <returns></returns>
        string GetEnableItemValue(string itemName);

        IEnumerable<DataItemModel> GetDataItemByDetailValue(string itemCode, string itemDetailValue);

        /// <summary>
        /// 通过编码获取对象
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        IEnumerable<DataItemModel> GetAllDataItemListByItemCode(string itemCode);
        #endregion

        #region 验证数据
        /// <summary>
        /// 项目值不能重复
        /// </summary>
        /// <param name="itemValue">项目值</param>
        /// <param name="keyValue">主键</param>
        /// <param name="itemId">分类Id</param>
        /// <returns></returns>
        bool ExistItemValue(string itemValue, string keyValue, string itemId);
        /// <summary>
        /// 项目名不能重复
        /// </summary>
        /// <param name="itemName">项目名</param>
        /// <param name="keyValue">主键</param>
        /// <param name="itemId">分类Id</param>
        /// <returns></returns>
        bool ExistItemName(string itemName, string keyValue, string itemId);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 根据编码类别Id删除明细
        /// </summary>
        /// <param name="keyValue">编码类别Id</param>
        void RemoveByItemId(string itemId);
        /// <summary>
        /// 保存明细表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="dataItemDetailEntity">明细实体</param>
        /// <returns></returns>
        void SaveForm(string keyValue, DataItemDetailEntity dataItemDetailEntity);
        #endregion


    }
}
