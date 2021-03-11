using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.IService.StandardSystem;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.StandardSystem
{
    /// <summary>
    /// 描 述：相应元素表
    /// </summary>
    public class ElementService : RepositoryFactory<ElementEntity>, ElementIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ElementEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ElementEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }

        /// <summary>
        /// 判断节点下有无子节点数据
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public bool IsHasChild(string parentId)
        {
            return this.BaseRepository().FindObject(string.Format("select count(1) from hrs_element where parentid='{0}'", parentId)).ToInt() > 0 ? true : false;
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ElementEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                var node = this.BaseRepository().FindEntity(entity.PARENTID);
                string enCode = node == null ? "" : node.ENCODE;
                int count = BaseRepository().FindObject(string.Format("select count(1) from hrs_element where parentid='{0}'", entity.PARENTID)).ToInt();
                count++;
                if (count.ToString().Length < 2)
                {
                    enCode += "00" + count;
                }
                else if (count.ToString().Length >= 2 && count.ToString().Length < 3)
                {
                    enCode += "0" + count;
                }
                else
                {
                    enCode += count.ToString();
                }
                entity.ENCODE = enCode;
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
