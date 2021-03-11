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
    /// �� ��������Ԥ��
    /// </summary>
    public class WeatherService : RepositoryFactory<WeatherEntity>, WeatherIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<WeatherEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public WeatherEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                //��ѯ����
                if (!queryParam["keyword"].IsEmpty())
                {
                    string keyword = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and (WEATHER like '%{0}%' or REQUIRE like '%{0}%')", keyword);
                }

                #region Ȩ���ж�
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
                if (arr[1].Contains("��"))
                {
                    int index = arr[1].IndexOf("��", StringComparison.Ordinal);
                    int fl = int.Parse(arr[1].Substring(index - 1, index));
                    string sqlfl = string.Format("select REQUIRE from bis_weather where weather like '%��%' and weather like '%{0}%'", fl);
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

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
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
