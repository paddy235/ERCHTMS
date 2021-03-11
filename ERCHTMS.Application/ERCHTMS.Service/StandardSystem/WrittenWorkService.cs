using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.IService.StandardSystem;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;
using ERCHTMS.Entity.SafetyLawManage;
using Newtonsoft.Json.Linq;
using ERCHTMS.Code;


namespace ERCHTMS.Service.StandardSystem
{
    /// <summary>
    /// 描 述：书面工作程序swp
    /// </summary>
    public class WrittenWorkService : RepositoryFactory<WrittenWorkEntity>, WrittenWorkIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson, string authType)
        {
            DatabaseType dataType = DatabaseType.Oracle;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            #region 查表
            pagination.p_kid = "Id as swpid";
            pagination.p_fields = "CreateDate,FileName,IssueDept,FileCode,PublishDate,CarryDate,FilesId,createuserid,createuserdeptcode,createuserorgcode,belongtypecode";
            pagination.p_tablename = " hrs_writtenwork";
            #endregion
            pagination.conditionJson = "1=1";
            if (!user.IsSystem)
            {
               
                if (!string.IsNullOrEmpty(authType))
                {
                    switch (authType)
                    {
                        case "1":
                            pagination.conditionJson += " and createuserid='" + user.UserId + "'";
                            break;
                        case "2":
                            pagination.conditionJson += " and BelongTypeCode='" + user.DeptCode + "'";
                            break;
                        case "3":
                            pagination.conditionJson += " and BelongTypeCode like'" + user.DeptCode + "%'";
                            break;
                        case "4":
                            pagination.conditionJson += " and BelongTypeCode like'" + user.OrganizeCode + "%'";
                            break;
                    }
                }
                else
                {
                    pagination.conditionJson += " and 0=1";
                }
            }
            #region  筛选条件
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and FileName  like '%{0}%'", queryParam["keyword"].ToString());
            }
            //类型节点
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and BelongTypeCode like '{0}%'", queryParam["code"].ToString());
            }
            //选中的数据
            if (!queryParam["idsData"].IsEmpty())
            {
                var ids = queryParam["idsData"].ToString();
                string idsarr = "";
                if (ids.Contains(','))
                {
                    string[] array = ids.TrimEnd(',').Split(',');
                    foreach (var item in array)
                    {
                        idsarr += "'" + item + "',";
                    }
                    if (idsarr.Contains(","))
                        idsarr = idsarr.TrimEnd(',');
                }
                pagination.conditionJson += string.Format(" and id in({0})", idsarr);
            }
            #endregion
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<WrittenWorkEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WrittenWorkEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WrittenWorkEntity GetEntity(string keyValue)
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
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                db.Delete<WrittenWorkEntity>(keyValue);
                db.Delete<StoreLawEntity>(t => t.LawId
.Equals(keyValue));
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, WrittenWorkEntity entity)
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

