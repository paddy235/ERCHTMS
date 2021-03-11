using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util.Extension;
using BSFramework.Util;

namespace ERCHTMS.Service.SystemManage
{
    /// <summary>
    /// 描 述：外包电厂提交资料说明表
    /// </summary>
    public class OutcommitfileService : RepositoryFactory<OutcommitfileEntity>, OutcommitfileIService
    {
        #region 获取数据
        public DataTable GetPageList(Pagination pagination, string queryJson) {
            var queryParam = queryJson.ToJObject();
            DatabaseType dataType = DbHelper.DbType;
            if (!queryParam["Keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.title like'{0}%' ", queryParam["Keyword"].ToString());
            }
            DataTable result = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return result;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OutcommitfileEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OutcommitfileEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 根据机构Code查询本机构是否已经添加
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public bool GetIsExist(string orgCode) {
            var count = this.BaseRepository().IQueryable().Where(x => x.CreateUserOrgCode == orgCode).ToList().Count;
            if (count == 0) return true;
            else
                return false;
        }

        public OutcommitfileEntity GetEntityByOrgCode(string orgCode) {
           return this.BaseRepository().IQueryable().Where(x => x.CreateUserOrgCode == orgCode).ToList().FirstOrDefault();
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
        public void SaveForm(string keyValue, OutcommitfileEntity entity)
        {
            entity.ID = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                var oldEntity = this.BaseRepository().FindEntity(keyValue);
                if (oldEntity != null)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
