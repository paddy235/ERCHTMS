using System;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// 描 述：海康门禁间设备管理
    /// </summary>
    public class HikaccessService : RepositoryFactory<HikaccessEntity>, HikaccessIService
    {
        #region 获取数据
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            if (!queryParam["AreaId"].IsEmpty())
            {
                string AreaId = queryParam["AreaId"].ToString();
                pagination.conditionJson += string.Format(" and AreaId = '{0}'", AreaId);
            }

            //设备名称
            if (!queryParam["DeviceName"].IsEmpty())
            {
                string DeviceName = queryParam["DeviceName"].ToString();

                pagination.conditionJson += string.Format(" and DeviceName like '%{0}%'", DeviceName);

            }
            //进出类型
            if (!queryParam["OutType"].IsEmpty())
            {
                int OutType = Convert.ToInt32(queryParam["OutType"]);

                pagination.conditionJson += string.Format(" and OutType ={0}", OutType);

            }
            //设备所属门岗
            if (!queryParam["AreaName"].IsEmpty())
            {
                string AreaName = queryParam["AreaName"].ToString();
                pagination.conditionJson += string.Format(" and AreaName like '%{0}%'", AreaName);
            }
            //设备IP
            if (!queryParam["DeviceIP"].IsEmpty())
            {
                string DeviceIP = queryParam["DeviceIP"].ToString();
                pagination.conditionJson += string.Format(" and DeviceIP like '%{0}%'", DeviceIP);
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HikaccessEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HikaccessEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 根据hikID获取设备信息
        /// </summary>
        /// <param name="HikId"></param>
        /// <returns></returns>
        public HikaccessEntity HikGetEntity(string HikId)
        {
            string sql = string.Format("select * from bis_hikaccess  where hikid = '{0}'", HikId);
            return this.BaseRepository().FindList(sql).FirstOrDefault();
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 门禁状态反控
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="type"></param>
        /// <param name="pitem"></param>
        /// <param name="url"></param>
        public void ChangeControl(string keyValue, int type, string pitem, string url)
        {
            //如果是2门开则不更改状态
            if (type != 2)
            {
                string sql = string.Format("update bis_hikaccess set status={0} where hikid in ({1})", type, keyValue);
                this.BaseRepository().ExecuteBySql(sql);
            }

            keyValue = keyValue.Replace("'", "");
            string[] ids = keyValue.Split('\'');
            List<string> doorIndexCodes = new List<string>();
            foreach (var str in ids)
            {
                doorIndexCodes.Add(str);
            }
            string apiurl = "/artemis/api/acs/v1/door/doControl";
            var model = new
            {
                doorIndexCodes,
                controlType = type
            };
            string key = string.Empty;// "21049470";
            string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
            if (!string.IsNullOrEmpty(pitem))
            {
                key = pitem.Split('|')[0];
                sign = pitem.Split('|')[1];
            }
            SocketHelper.LoadCameraList(model, url, apiurl, key, sign);
        }

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
        public void SaveForm(string keyValue, HikaccessEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                entity.Status = 1;
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
