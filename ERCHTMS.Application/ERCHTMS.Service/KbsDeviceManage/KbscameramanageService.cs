using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using ERCHTMS.Code;

namespace ERCHTMS.Service.KbsDeviceManage
{
    /// <summary>
    /// 描 述：康巴什摄像头管理
    /// </summary>
    public class KbscameramanageService : RepositoryFactory<KbscameramanageEntity>, KbscameramanageIService
    {
        #region 获取数据
        /// <summary>
        /// 获取全部列表数据
        /// </summary>
        /// <returns>返回分页列表</returns>
        public List<KbscameramanageEntity> GetPageList()
        {
            return this.BaseRepository().IQueryable().OrderByDescending(it=>it.CreateDate).ToList();

        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<KbscameramanageEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public KbscameramanageEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 根据状态获取摄像头数量
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public int GetCameraNum(string status)
        {
            return this.BaseRepository().IQueryable(it => it.State == status).Count();
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
        public void SaveForm(string keyValue, KbscameramanageEntity entity)
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
        /// 接口修改状态用方法
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateState(KbscameramanageEntity entity)
        {
            this.BaseRepository().Update(entity);
        }

        /// <summary>
        /// 摄像头唯一性检查，无重复返回true
        /// </summary>
        /// <param name="cameraId"></param>
        /// <returns></returns>
        public bool UniqueCheck(string cameraId)
        {
            bool any = BaseRepository().IQueryable().Any(x => x.CameraId == cameraId);
            return !any;
        }

        #endregion
    }
}
