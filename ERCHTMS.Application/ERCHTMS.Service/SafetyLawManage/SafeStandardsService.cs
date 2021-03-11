using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.IService.SafetyLawManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.SafetyLawManage
{
    /// <summary>
    /// 描 述：安全操作规程
    /// </summary>
    public class SafeStandardsService : RepositoryFactory<SafeStandardsEntity>, SafeStandardsIService
    {
        DepartmentService DepartmentService = new DepartmentService();
        #region 获取数据

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["filename"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and FileName like '%{0}%'", queryParam["filename"].ToString());
            }

            if (user.RoleName.Contains("省级用户"))
            {
                if (!queryParam["orgcode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and createuserorgcode ='{0}'", queryParam["orgcode"].ToString());
                }
            }
            else
            {
                //0本级,1上级
                if (!queryParam["state"].IsEmpty())
                {
                    if (queryParam["state"].ToString() == "0")
                    {
                        pagination.conditionJson += string.Format(" and createuserorgcode ='{0}'", user.OrganizeCode);
                    }
                    else
                    {
                        var provdata = DepartmentService.GetList().Where(t => user.NewDeptCode.StartsWith(t.DeptCode) && t.Nature == "省级" && string.IsNullOrWhiteSpace(t.Description));
                        if (provdata.Count() > 0)
                        {
                            DepartmentEntity provEntity = provdata.FirstOrDefault();
                            pagination.conditionJson += string.Format(" and createuserorgcode ='{0}'", provEntity.EnCode);
                        }
                    }
                }
            }
            //时间范围
            if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
            {
                string startTime = queryParam["sTime"].ToString();
                string endTime = queryParam["eTime"].ToString();
                if (queryParam["sTime"].IsEmpty())
                {
                    startTime = "1899-01-01";
                }
                if (queryParam["eTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                pagination.conditionJson += string.Format(" and ReleaseDate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            //选择的类型
            if (!queryParam["treeCode"].IsEmpty())
            {
                if (!queryParam["flag"].IsEmpty())
                {
                    if (queryParam["flag"].ToString() == "0")
                    {
                        pagination.conditionJson += string.Format(" and  LawTypeCode='{0}'", queryParam["treeCode"].ToString());
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(@" and LawTypeCode like '{0}%'", queryParam["treeCode"].ToString());
                    }
                }
                else
                {
                    pagination.conditionJson += string.Format(" and  LawTypeCode='{0}'", queryParam["treeCode"].ToString());
                }
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
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and ISSUEDEPT in (select departmentid from base_department where encode like '{0}%')", queryParam["code"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<SafeStandardsEntity> GetPageList(Pagination pagination, string queryJson)
        {
            if (!string.IsNullOrWhiteSpace(queryJson))
            {
                string sql = "select * from BIS_SAFESTANDARDS";
                if (queryJson.Trim().StartsWith("and"))
                {
                    sql += " where 1=1 " + queryJson;
                }
                else
                {
                    sql += " where " + queryJson;
                }
                return this.BaseRepository().FindList(sql).ToList();
            }
            else
            {
                return this.BaseRepository().IQueryable().ToList();
            }
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafeStandardsEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafeStandardsEntity GetEntity(string keyValue)
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
                db.Delete<SafeStandardsEntity>(keyValue);
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
        public void SaveForm(string keyValue, SafeStandardsEntity entity)
        {
            bool b = true;
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                var sl = BaseRepository().FindEntity(keyValue);
                if (sl != null)
                {
                    b = false;
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
            }
            if (b)
            {
                entity.Id = keyValue;
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
