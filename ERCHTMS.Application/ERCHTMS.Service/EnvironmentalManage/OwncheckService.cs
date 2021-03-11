using ERCHTMS.Entity.EnvironmentalManage;
using ERCHTMS.IService.EnvironmentalManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System;
using BSFramework.Util;
using System.Data;
using BSFramework.Util.Extension;
using BSFramework.Data;

namespace ERCHTMS.Service.EnvironmentalManage
{
    /// <summary>
    /// 描 述：自行检测
    /// </summary>
    public class OwncheckService : RepositoryFactory<OwncheckEntity>, OwncheckIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OwncheckEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OwncheckEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public string GetMaxCode()
        {
            var entity = this.BaseRepository().FindList(" select max(checkcode) as CheckCode from BIS_OWNCHECK").FirstOrDefault();
            if (entity == null || entity.CheckCode == null)
                return DateTime.Now.ToString("yyyy") + "0001";
            if (entity.CheckCode.Substring(0,4)== DateTime.Now.ToString("yyyy"))
                return (int.Parse(entity.CheckCode) + 1).ToString();
            return DateTime.Now.ToString("yyyy") + "0001";

        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                if (!string.IsNullOrEmpty(queryJson))
                {
                    var queryParam = queryJson.ToJObject();
                    if (!queryParam["dataname"].IsEmpty())
                    {
                        pagination.conditionJson += " and dataname like '%" + queryParam["dataname"].ToString() + "%'";
                    }
                    if (!queryParam["stime"].IsEmpty())
                    {
                        pagination.conditionJson += " and uploadtime >= to_date('" + queryParam["stime"].ToString() + "','yyyy-MM-dd')";
                    }
                    if (!queryParam["etime"].IsEmpty())
                    {
                        pagination.conditionJson += " and uploadtime <= to_date('" + queryParam["etime"].ToString() + "','yyyy-MM-dd')";
                    }
                }
                return this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);
            }
            catch (System.Exception ex)
            {

                throw;
            }

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
        public void SaveForm(string keyValue, OwncheckEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                OwncheckEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se == null)
                {
                    entity.Id = keyValue;
                    entity.Create();
                    this.BaseRepository().Insert(entity);


                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
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
