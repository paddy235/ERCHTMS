using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Entity.JTSafetyCheck;
using ERCHTMS.IService.JTSafetyCheck;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;

namespace ERCHTMS.Service.JTSafetyCheck
{
    /// <summary>
    /// 描 述：京泰安全检查
    /// </summary>
    public class JTSafetyCheckService : RepositoryFactory<SafetyCheckEntity>, JTSafetyCheckIService
    {
        #region 获取数据
        /// <summary>
        /// 获取全部列表数据
        /// </summary>
        /// <returns>返回分页列表</returns>
        public List<SafetyCheckEntity> GetPageList()
        {
            return this.BaseRepository().IQueryable().OrderByDescending(it => it.CreateTime).ToList();
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (!string.IsNullOrEmpty(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["deptCode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and deptCode like '{0}%'", queryParam["deptCode"].ToString());
                }
               if (!queryParam["checkType"].IsEmpty())
               {
                   pagination.conditionJson += string.Format(" and checktype='{0}'", queryParam["checkType"].ToString());
               }
               if (!queryParam["checkLevel"].IsEmpty())
               {
                   pagination.conditionJson += string.Format(" and checklevel='{0}'", queryParam["checkLevel"].ToString());
               }
               if (!queryParam["checkTitle"].IsEmpty())
               {
                   pagination.conditionJson += string.Format(" and checktitle like '%{0}%'", queryParam["checkTitle"].ToString().Trim());
               }
               if (!queryParam["startTime"].IsEmpty() && !queryParam["endTime"].IsEmpty())
               {
                   pagination.conditionJson += string.Format(" and startdate between to_date('{0}','yyyy-mm-dd') and to_date('{1}','yyyy-mm-dd')", queryParam["startTime"].ToString(), queryParam["endTime"].ToString());
               }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetyCheckEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        public DataTable GetItemsList(string checkId,string status="")
        {
            string sql = "select id,createuserid,itemname,measures,deptname,dutyuser,plandate,to_char(realitydate,'yyyy-mm-dd') realitydate,checkuser,result,remark from jt_checkitems where checkid='" + checkId + "'";
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (status=="逾期未完成")
                {
                    sql += " and result='未完成'  and  plandate is not null";
                }
                else
                {
                    sql += " and result='"+status+"'";
                }
            }
            sql += " order by sortcode asc";
            return BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafetyCheckEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        public CheckItemsEntity GetItemEntity(string keyValue)
        {
            return DbFactory.Base().FindEntity<CheckItemsEntity>(keyValue);
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
        public void RemoveItemForm(string keyValue)
        {
            DbFactory.Base().Delete<CheckItemsEntity>(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public bool SaveForm(string keyValue, SafetyCheckEntity entity)
        {
            try
            {
                int count = 0;
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Id = keyValue;
                    SafetyCheckEntity se = BaseRepository().FindEntity(keyValue);
                    if (se == null)
                    {
                        entity.Create();
                        count = this.BaseRepository().Insert(entity);
                    }
                    else
                    {
                        entity.Modify(keyValue);
                        count = this.BaseRepository().Update(entity);
                    }

                }
                else
                {
                    entity.Create();
                    count = this.BaseRepository().Insert(entity);
                }
                return count > 0 ? true : false ;
            }
            catch
            {
                return false;
            }
        }
       
        public void SaveItemForm(string keyValue, CheckItemsEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Id = keyValue;
                CheckItemsEntity se = DbFactory.Base().FindEntity<CheckItemsEntity>(keyValue);
                if (se == null)
                {
                    entity.Create();
                    DbFactory.Base().Insert<CheckItemsEntity>(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    DbFactory.Base().Update<CheckItemsEntity>(entity);
                }

            }
            else
            {
                entity.Create();
                DbFactory.Base().Insert<CheckItemsEntity>(entity);
            }
        }
        public bool SaveItems(List<CheckItemsEntity> items)
        {
            var db = DbFactory.Base().BeginTrans();
            try
            {
                db.Insert<CheckItemsEntity>(items);
                db.Commit();
                return true;
            }
            catch(Exception ex)
            {
                db.Rollback();
                return false;
            }
        }
        public bool Save(string keyValue,SafetyCheckEntity entity, List<CheckItemsEntity> items)
        {
            var db = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    entity.Id = keyValue;
                    SafetyCheckEntity se = BaseRepository().FindEntity(keyValue);
                    if (se == null)
                    {
                        entity.Create();
                        db.Insert(entity);
                    }
                    else
                    {
                        entity.Modify(keyValue);
                        db.Update(entity);
                        var checkItems = GetItemsList(keyValue);
                         if (checkItems.Rows.Count>0)
                         {
                             db.Delete<CheckItemsEntity>(checkItems);
                         }
                    }
                }
                else
                {
                    entity.Create();
                    db.Insert(entity);
                }
               
                db.Insert<CheckItemsEntity>(items);
                db.Commit();
                return true;
            }
            catch
            {
                db.Rollback();
                return false;
            }
        }
        #endregion
    }
}
