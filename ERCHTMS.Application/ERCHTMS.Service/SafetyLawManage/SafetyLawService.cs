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
    /// 描 述：安全生产法律法规
    /// </summary>
    public class SafetyLawService : RepositoryFactory<SafetyLawEntity>, SafetyLawIService
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
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "filename"://文件名称
                        pagination.conditionJson += string.Format(" and FileName  like '%{0}%'", keyord);
                        break;
                    case "issuedept"://发布机关
                        pagination.conditionJson += string.Format(" and IssueDept  like '%{0}%'", keyord);
                        break;
                    case "filecode"://文号
                        pagination.conditionJson += string.Format(" and filecode  like '%{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }

            //发布地区
            if (!queryParam["type"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and Province  like '{0}%'", queryParam["type"]);

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
            //类型节点
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and LawTypeCode like '%{0}%'", queryParam["code"].ToString());
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
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<SafetyLawEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetyLawEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafetyLawEntity GetEntity(string keyValue)
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
                db.Delete<SafetyLawEntity>(keyValue);
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
        public void SaveForm(string keyValue, SafetyLawEntity entity)
        {
            bool b = false;
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                var sl = BaseRepository().FindEntity(keyValue);
                if (sl != null)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    b = true;
                }
            }
            else
            {
                b = true;
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
