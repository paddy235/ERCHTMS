using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.IService.OccupationalHealthManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病人员表
    /// </summary>
    public class OccupatioalstaffService : RepositoryFactory<OccupatioalstaffEntity>, OccupatioalstaffIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OccupatioalstaffEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OccupatioalstaffEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageListByProc(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();

                switch (condition)
                {
                    case "name":          //姓名
                        pagination.conditionJson += string.Format(" and MECHANISMNAME  like '%{0}%'", keyord.Trim());
                        break;
                    default:
                        break;
                }

            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        


        /// <summary>
        /// 根据条件查询所有数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTable(string queryJson,string where)
        {
            string Sql = "SELECT OCCID,MECHANISMNAME,TO_CHAR(INSPECTIONTIME,'yyyy-mm-dd hh24:mi:ss')as INSPECTIONTIME,INSPECTIONNUM,PATIENTNUM,ISANNEX,UNUSUALNUM FROM V_OCCUPATIOALSTAFF WHERE 1=1";
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();

                switch (condition)
                {
                    case "name":          //姓名
                        Sql += string.Format(" and MECHANISMNAME  like '%{0}%'", keyord.Trim());
                        break;
                    default:
                        break;
                }

            }

            Sql += where;

            Sql += " ORDER BY INSPECTIONTIME DESC";
            return this.BaseRepository().FindTable(Sql);
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
        public void SaveForm(string keyValue, OccupatioalstaffEntity entity)
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

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="IsNew">是否新增</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(bool IsNew, OccupatioalstaffEntity entity)
        {
            if (!IsNew)
            {
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
