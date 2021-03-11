using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.SystemManage;
using System;
using System.Collections.Generic;

namespace ERCHTMS.Busines.SystemManage
{
    /// <summary>
    /// 描 述：数据字典分类
    /// </summary>
    public class DataItemBLL
    {
        private IDataItemService service = new DataItemService();

        #region 获取数据
        /// <summary>
        /// 分类列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataItemEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// 分类实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DataItemEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 根据分类编号获取实体对象
        /// </summary>
        /// <param name="ItemCode">编号</param>
        /// <returns></returns>
        public DataItemEntity GetEntityByCode(string ItemCode)
        {
            return service.GetEntityByCode(ItemCode);
        }

        /// <summary>
        /// 根据字典Code获取详细数据列表
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IEnumerable<DataItemDetailEntity> GetList(string code)
        {
            //先获取到字典项
            DataItemEntity DataItems = GetEntityByCode(code);
            DataItemDetailBLL did = new DataItemDetailBLL();
            //根据字典项获取值
            IEnumerable<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId);
            return didList;
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 分类编号不能重复
        /// </summary>
        /// <param name="itemCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistItemCode(string itemCode, string keyValue)
        {
            return service.ExistItemCode(itemCode, keyValue);
        }
        /// <summary>
        /// 分类名称不能重复
        /// </summary>
        /// <param name="itemName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistItemName(string itemName, string keyValue)
        {
            return service.ExistItemName(itemName, keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue,List<DataItemDetailEntity> details)
        {
            try
            {
                service.RemoveForm(keyValue, details);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存分类表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="dataItemEntity">分类实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, DataItemEntity dataItemEntity)
        {
            try
            {
                service.SaveForm(keyValue, dataItemEntity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
