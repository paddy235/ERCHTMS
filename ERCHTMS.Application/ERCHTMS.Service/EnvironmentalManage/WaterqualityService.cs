using System;
using ERCHTMS.Entity.EnvironmentalManage;
using ERCHTMS.IService.EnvironmentalManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Service.CommonPermission;

namespace ERCHTMS.Service.EnvironmentalManage
{
    /// <summary>
    /// 描 述：水质分析
    /// </summary>
    public class WaterqualityService : RepositoryFactory<WaterqualityEntity>, WaterqualityIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();


                //查询条件
                if (!queryParam["keyvalue"].IsEmpty())
                {
                    string id = queryParam["keyvalue"].ToString();
                    pagination.conditionJson += string.Format(" and id ='{0}'", id);
                }

                //查询条件
                if (!queryParam["keyword"].IsEmpty())
                {
                    string sgtypename = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and sampleno like '%{0}%'", sgtypename);
                }

     
                if (!queryParam["sampletype"].IsEmpty())
                {
                    string sgtype = queryParam["sampletype"].ToString();
                    pagination.conditionJson += string.Format(" and sampletype = '{0}'", sgtype);
                }

                if (!queryParam["testdate"].IsEmpty())
                {
                    string testdate = queryParam["testdate"].ToString();
                    pagination.conditionJson += string.Format(" and testdate =  to_date('{0}', 'yyyy-MM-dd')", testdate);
                }
              
                #region 权限判断
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    pagination = PermissionByCurrent.GetPermissionByCurrent2(pagination, queryParam["code"].ToString(), queryParam["isOrg"].ToString());
                }
                #endregion
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WaterqualityEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WaterqualityEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取水质参考标准
        /// </summary>
        public object GetStandardJson(string sampletype)
        {
            string sql = @"select  TESTPROJECT,UNIT,KPITARGET,REFACCORDING,TESTMETHOD from bis_waterrecord  where SAMPLETYPE = " + sampletype + "order by createdate";
            DataTable dt = this.BaseRepository().FindTable(sql);
            List<object> objects = new List<object>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    objects.Add(new
                    {
                        TESTPROJECT = row["TESTPROJECT"].ToString(),
                        UNIT = row["UNIT"].ToString(),
                        KPITARGET = row["KPITARGET"].ToString(),
                        REFACCORDING = row["REFACCORDING"].ToString(),
                        TESTMETHOD = row["TESTMETHOD"].ToString(),
                    });
                }

            }

            return objects;
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
        public void SaveForm(string keyValue, WaterqualityEntity entity)
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
