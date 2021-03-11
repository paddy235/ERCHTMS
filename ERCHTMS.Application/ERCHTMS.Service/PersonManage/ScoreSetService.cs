using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
namespace ERCHTMS.Service.PersonManage
{
    /// <summary>
    /// 描 述：积分设置
    /// </summary>
    public class ScoreSetService : RepositoryFactory<ScoreSetEntity>, ScoreSetIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ScoreSetEntity> GetList(string queryJson)
        {
            var expression = LinqExtensions.True<ScoreSetEntity>();
            if (!string.IsNullOrEmpty(queryJson)) 
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["deptCode"].IsEmpty())
                {
                    string deptCode = queryParam["deptCode"].ToString().Trim();
                    expression = expression.And(t => t.DeptCode.Equals(deptCode) || t.DeptCode == "00");
                }
                if (!queryParam["itemName"].IsEmpty())
                {
                    string itemName = queryParam["itemName"].ToString().Trim();
                    expression = expression.And(t => t.ItemName.Contains(itemName));
                }
                if (!queryParam["itemType"].IsEmpty())
                {
                    string itemType = queryParam["itemType"].ToString().Trim();
                    expression = expression.And(t => t.ItemType.Equals(itemType));
                }
                if (!queryParam["isAuto"].IsEmpty())
                {
                    int isAuto = queryParam["isAuto"].ToString().Trim().ToInt();
                    expression = expression.And(t => t.IsAuto == isAuto);
                }
            }

            return this.BaseRepository().IQueryable(expression).ToList();
        }
        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="page">分页条件</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination page,string queryJson)
        {
            var expression = LinqExtensions.True<ScoreSetEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["deptCode"].IsEmpty())
            {
                string deptCode = queryParam["deptCode"].ToString().Trim();
                page.conditionJson += string.Format(" and DeptCode='00' || deptcode='{0}'",deptCode);
            }
            if (!queryParam["itemName"].IsEmpty())
            {
                string itemName = queryParam["itemName"].ToString().Trim();
                page.conditionJson += string.Format(" and itemName like '%{0}%'", itemName);
            }
            if (!queryParam["itemType"].IsEmpty())
            {
                string itemType = queryParam["itemType"].ToString().Trim();
                page.conditionJson += string.Format(" and itemType='{0}'", itemType);
            }
            return this.BaseRepository().FindTableByProcPager(page, BSFramework.Data.DbHelper.DbType);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ScoreSetEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, ScoreSetEntity entity)
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
