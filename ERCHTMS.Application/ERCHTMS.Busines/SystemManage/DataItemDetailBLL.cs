using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.SystemManage;
using BSFramework.Cache.Factory;
using BSFramework.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ERCHTMS.Busines.SystemManage
{
    /// <summary>
    /// 描 述：数据字典明细
    /// </summary>
    public class DataItemDetailBLL
    {
        private IDataItemDetailService service = new DataItemDetailService();
        /// <summary>
        /// 缓存key
        /// </summary>
        public string cacheKey = "dataItemCache";

        public List<DataItemDetailEntity> ListByCode(string data)
        {
            return service.ListByItemCode(data);
        }

        #region 获取数据

        /// <summary>
        /// 获取数据字典列表（绑定控件）
        /// </summary>
        /// <param name="EnCode">代码</param>
        /// <returns>返回列表Json</returns>
        public string GetDataItemListOption(string EnCode)
        {
            var data = GetDataItemListByItemCode("'" + EnCode + "'");
            StringBuilder sb = new StringBuilder();
            foreach (DataItemModel dr in data)
            {
                sb.AppendFormat("<option value='{0}'>{1}</option>", dr.ItemValue, dr.ItemName);
            }
            return sb.ToString();
        }

        public IEnumerable<DataItemDetailEntity> GetListItems(string category)
        {
            return service.GetListItems(category);
        }

        /// <summary>
        /// 明细列表
        /// </summary>
        /// <param name="itemId">分类Id</param>
        /// <returns></returns>
        public IEnumerable<DataItemDetailEntity> GetList(string itemId)
        {
            return service.GetList(itemId);
        }
        public DataTable GetListByCode(string itemCode)
        {
            return service.GetListByCode(itemCode);
        }
        public List<DataItemDetailEntity> ListByItemCode(string itemCode)
        {
            return service.ListByItemCode(itemCode);
        }
        /// <summary>
        /// 明细实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DataItemDetailEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 数据字典列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataItemModel> GetDataItemList()
        {
            return service.GetDataItemList();
        }
        public string GetOptionsString(string itemCode)
        {
            DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
            var data = dataItemDetailBLL.GetListByCode(itemCode);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in data.Rows)
            {
                sb.AppendFormat("<option value='{0}'>{1}</option>", dr[1].ToString(), dr[0].ToString());
            }
            return sb.ToString();
        }

        public string GetLiString(string itemCode)
        {
            DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
            var data = dataItemDetailBLL.GetListByCode(itemCode);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in data.Rows)
            {
                sb.AppendFormat("<li class='option-item' style='padding:8px 1em;' data-index='{0}' data-value='{0}'>{1}</option>", dr[1].ToString(), dr[0].ToString());
            }
            return sb.ToString();
        }
        /// <summary>
        /// 通过编码类别名称获取编码
        /// </summary>
        /// <param name="itemName">编码类别名称</param>
        /// <returns></returns>
        public DataTable GetDataItemListByItemName(string itemName)
        {
            return service.GetDataItemListByItemName(itemName);
        }
        #endregion

        /// <summary>
        /// 通过编码获取
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public IEnumerable<DataItemModel> GetDataItemListByItemCode(string itemCode)
        {
            return service.GetDataItemListByItemCode(itemCode);
        }
        /// <summary>
        /// 根据编码名称获取编码值
        /// </summary>
        /// <param name="itemName">编码名称</param>
        /// <returns></returns>
        public string GetItemValue(string itemName)
        {
            return service.GetItemValue(itemName);
        }

        /// <summary>
        /// 获取编码对象，根据编码名称
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public DataItemDetailEntity GetEntityByItemName(string itemName)
        {
            return service.GetEntityByItemName(itemName);
        }

        /// <summary>
        /// 根据编码值获取编码名称
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="itemValue"></param>
        /// <returns></returns>
        public string GetItemName(string itemCode, string itemValue)
        {
            return service.GetItemName(itemCode, itemValue);
        }

        /// <summary>
        /// 根据编码名称获取编码code
        /// </summary>
        /// <param name="itemName">编码名称</param>
        /// <returns></returns>
        public string GetItemCode(string itemName)
        {
            return service.GetItemCode(itemName);
        }

        /// <summary>
        /// 根据编码名称获取有效的编码值
        /// </summary>
        /// <param name="itemName">编码名称</param>
        /// <returns></returns>
        public string GetEnableItemValue(string itemName)
        {
            return service.GetEnableItemValue(itemName);
        }

        public string GetItemValue(string itemName, string itemcode)
        {
            return service.GetItemValue(itemName, itemcode);
        }

        public IEnumerable<DataItemModel> GetDataItem(string itemCode, string itemDetailCode)
        {
            return service.GetDataItem(itemCode, itemDetailCode);
        }
        public IEnumerable<DataItemModel> GetDataItemByDetailCode(string itemCode, string itemDetailCode)
        {
            return service.GetDataItemByDetailCode(itemCode, itemDetailCode);
        }
        public DataTable GetListByItemCode(string itemCode)
        {
            return service.GetListByItemCode(itemCode);
        }
        public DataItemDetailEntity GetListByItemCodeEntity(string itemCode)
        {
            return service.GetListByItemCodeEntity(itemCode);
        }

        public IEnumerable<DataItemModel> GetDataItemByDetailValue(string itemCode, string itemDetailValue)
        {
            return service.GetDataItemByDetailValue(itemCode, itemDetailValue);
        }

        /// <summary>
        /// 通过编码获取对象
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public IEnumerable<DataItemModel> GetAllDataItemListByItemCode(string itemCode)
        {
            return service.GetAllDataItemListByItemCode(itemCode);
        }
        #region 验证数据
        /// <summary>
        /// 项目值不能重复
        /// </summary>
        /// <param name="itemValue">项目值</param>
        /// <param name="keyValue">主键</param>
        /// <param name="itemId">分类Id</param>
        /// <returns></returns>
        public bool ExistItemValue(string itemValue, string keyValue, string itemId)
        {
            return service.ExistItemValue(itemValue, keyValue, itemId);
        }
        /// <summary>
        /// 项目名不能重复
        /// </summary>
        /// <param name="itemName">项目名</param>
        /// <param name="keyValue">主键</param>
        /// <param name="itemId">分类Id</param>
        /// <returns></returns>
        public bool ExistItemName(string itemName, string keyValue, string itemId)
        {
            return service.ExistItemName(itemName, keyValue, itemId);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
                CacheFactory.Cache().RemoveCache(cacheKey);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 根据编码类别Id删除明细
        /// </summary>
        /// <param name="keyValue">编码类别Id</param>
        public void RemoveByItemId(string itemId)
        {
            service.RemoveByItemId(itemId);

        }
        /// <summary>
        /// 保存明细表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="dataItemDetailEntity">明细实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, DataItemDetailEntity dataItemDetailEntity)
        {
            try
            {
                dataItemDetailEntity.SimpleSpelling = Str.PinYin(dataItemDetailEntity.ItemName);
                service.SaveForm(keyValue, dataItemDetailEntity);
                CacheFactory.Cache().RemoveCache(cacheKey);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
