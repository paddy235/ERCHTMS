using System;
using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.KbsDeviceManage
{
    /// <summary>
    /// 描 述：闯入人员抓拍记录表
    /// </summary>
    public class WorkcameracaptureService : RepositoryFactory<WorkcameracaptureEntity>, WorkcameracaptureIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WorkcameracaptureEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// 根据工作区域查询抓拍图片
        /// </summary>
        /// <param name="workid"></param>
        /// <returns></returns>
        public List<WorkcameracaptureEntity> GetCaptureList(string workid, string userid,string cameraid) 
        {
            Expression<Func<WorkcameracaptureEntity, bool>> condition= it=>true;
            if (!string.IsNullOrEmpty(workid))
            {
                condition = condition.And(it => it.WorkId == workid);
            }

            if (!string.IsNullOrEmpty(userid))
            {
                condition = condition.And(it => it.UserId == userid);
            }

            if (!string.IsNullOrEmpty(cameraid))
            {
                condition = condition.And(it => it.CameraId == cameraid);
            }

            return BaseRepository().IQueryable(condition).OrderBy(it => it.CreateDate).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WorkcameracaptureEntity GetEntity(string keyValue)
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
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, WorkcameracaptureEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                this.BaseRepository().Update(entity);
            }
            else
            {
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
