using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// 描 述：班车上车记录表
    /// </summary>
    public class CarrideService : RepositoryFactory<CarrideEntity>, CarrideIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CarrideEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CarrideEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取进出场相关人员
        /// </summary>
        /// <param name="lid"></param>
        /// <returns></returns>
        public string GetCarRide(string lid)
        {
            string sql = string.Format(@"select LISTAGG(username,',') WITHIN GROUP (ORDER BY username) as us from
            (select realname as username,lid from BIS_CARRIDE ride left join base_user use on use.userid=ride.createuserid) f where lid='{0}' group by lid",
                lid);
            object name=BaseRepository().FindObject(sql);
            if (name != null)
            {
                return name.ToString();
            }
            else
            {
                return "";
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
        public void SaveForm(string keyValue, CarrideEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Delete(it =>
                    it.Status == 0 && it.CarNo == entity.CarNo && it.CreateUserId == entity.CreateUserId);
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
