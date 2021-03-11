using System;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// 描 述：人员GPS关联表
    /// </summary>
    public class PersongpsService : RepositoryFactory<PersongpsEntity>, PersongpsIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<PersongpsEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable(it => it.VID == queryJson).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public PersongpsEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 查询此GPS设备是否被占用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool GetGps(string id, string gpsid)
        {
            string sql = string.Format(@"select count(id) as cou from bis_persongps where STATE=0 and VID!='{0}' and gpsid='{1}'", id, gpsid);
            int i = Convert.ToInt32(BaseRepository().FindObject(sql));
            return i > 0;
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
        public void SaveForm(string keyValue, PersongpsEntity entity)
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
