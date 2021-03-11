using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.IService.StandardSystem;
using ERCHTMS.Service.StandardSystem;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.StandardSystem
{
    /// <summary>
    /// 描 述：标准分类
    /// </summary>
    public class StcategoryBLL
    {
        private StcategoryIService service = new StcategoryService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<StcategoryEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public StcategoryEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 判断此节点下是否有子节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public bool IsHasChild(string parentId)
        {
            return service.IsHasChild(parentId);
        }
        /// <summary>
        /// 合规性评价-取大类
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StcategoryEntity> GetCategoryList()
        {
            return service.GetCategoryList();
        }
        public IEnumerable<StcategoryEntity> GetRankList(string Category)
        {
            return service.GetRankList(Category);
        }

        /// <summary>
        /// 获取实体(根据名称或parentid)
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public StcategoryEntity GetQueryEntity(string queryJson)
        {
            return service.GetQueryEntity(queryJson);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, StcategoryEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
