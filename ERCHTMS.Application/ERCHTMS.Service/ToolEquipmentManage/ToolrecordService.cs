using ERCHTMS.Entity.ToolEquipmentManage;
using ERCHTMS.IService.ToolEquipmentManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;

namespace ERCHTMS.Service.ToolEquipmentManage
{
    /// <summary>
    /// 描 述：电动工器具试验记录
    /// </summary>
    public class ToolrecordService : RepositoryFactory<ToolrecordEntity>, ToolrecordIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ToolrecordEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ToolrecordEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            pagination.conditionJson += string.Format(" and toolequipmentid='{0}'", queryParam["keyValue"].ToString());                     
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ToolrecordEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                ToolrecordEntity eEntity = GetEntity(keyValue);
                if (eEntity != null)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
            }
        }
        #endregion
    }
}
