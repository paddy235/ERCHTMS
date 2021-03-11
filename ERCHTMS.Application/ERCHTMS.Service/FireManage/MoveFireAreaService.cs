using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using ERCHTMS.Code;

namespace ERCHTMS.Service.FireManage
{
    /// <summary>
    /// 描 述：动火区域维护
    /// </summary>
    public class MoveFireAreaService : RepositoryFactory<MoveFireAreaEntity>, MoveFireAreaIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<MoveFireAreaEntity> GetList(string queryJson)
        {
            //return this.BaseRepository().IQueryable().ToList();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["Rank"].IsEmpty())
                {
                    int Rank = Convert.ToInt32(queryParam["Rank"]);
                    //return this.BaseRepository().IQueryable().Where(t => t.PatrolId == PatrolId).ToList();
                    return this.BaseRepository().IQueryable(t => t.Rank == Rank && t.CreateUserOrgCode == user.OrganizeCode).ToList();
                }
                else
                {
                    return this.BaseRepository().IQueryable().ToList();
                }
            }
            else
            {
                return this.BaseRepository().IQueryable().ToList();
            }
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public MoveFireAreaEntity GetEntity(string keyValue)
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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, MoveFireAreaEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
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
        #endregion
    }
}
