using System;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Service.CommonPermission;
using Newtonsoft.Json.Serialization;

namespace ERCHTMS.Service.SystemManage
{
    /// <summary>
    /// 描 述：天气预警
    /// </summary>
    public class WeatherService : RepositoryFactory<WeatherEntity>, WeatherIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WeatherEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WeatherEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

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
                if (!queryParam["keyword"].IsEmpty())
                {
                    string keyword = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and (WEATHER like '%{0}%' or REQUIRE like '%{0}%')", keyword);
                }

                #region 权限判断
                //if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                //{
                //    pagination = PermissionByCurrent.GetPermissionByCurrent2(pagination, queryParam["code"].ToString(), queryParam["isOrg"].ToString());
                //}
                #endregion
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        public string GetRequire(string weather)
        {
            string []arr = weather.Split('|');
            var objlist = new List<object>();
            if (!string.IsNullOrWhiteSpace(arr[0]))
            {
                string sql = string.Format("select REQUIRE from bis_weather where weather like '%{0}%'", arr[0]);
                var require = this.BaseRepository().FindList(sql);

                foreach (var item in require)
                {
                    if (!string.IsNullOrEmpty(item.Require))
                    {
                        objlist.Add(new
                        {
                            require = item.Require
                        });
                    }

                }
            }
          
            if (!string.IsNullOrWhiteSpace(arr[1]))
            {
                if (arr[1].Contains("级"))
                {
                    int index = arr[1].IndexOf("级", StringComparison.Ordinal);
                    int fl = int.Parse(arr[1].Substring(index - 1, index));
                    string sqlfl = string.Format("select REQUIRE from bis_weather where weather like '%风%' and weather like '%{0}%'", fl);
                    var require = this.BaseRepository().FindList(sqlfl);

                    foreach (var item in require)
                    {
                        if (!string.IsNullOrEmpty(item.Require))
                        {
                            objlist.Add(new
                            {
                                require = item.Require
                            });
                        }

                    }
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(objlist);
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
        public void SaveForm(string keyValue, WeatherEntity entity)
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
