using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;

namespace ERCHTMS.Service.FireManage
{
    /// <summary>
    /// 描 述：消防巡查确认记录
    /// </summary>
    public class AffirmRecordService : RepositoryFactory<AffirmRecordEntity>, AffirmRecordIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<AffirmRecordEntity> GetList(string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["PatrolId"].IsEmpty())
                {
                    string PatrolId = queryParam["PatrolId"].ToString();
                    //return this.BaseRepository().IQueryable().Where(t => t.PatrolId == PatrolId).ToList();
                    return this.BaseRepository().IQueryable(t => t.PatrolId == PatrolId).OrderBy(t => t.CreateDate).ToList();
                }
                else
                {
                    return this.BaseRepository().IQueryable().OrderBy(t => t.CreateDate).ToList();
                }
            }
            else
            {
                return this.BaseRepository().IQueryable().ToList();
            }
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public AffirmRecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, AffirmRecordEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                AffirmRecordEntity ae = this.BaseRepository().FindEntity(keyValue);
                if (ae == null)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                    UpdateEverydayPatrolEntity(entity);
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
                UpdateEverydayPatrolEntity(entity);
            }
        }
        /// <summary>
        /// 更新下一步流程负责人和流程状态
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateEverydayPatrolEntity(AffirmRecordEntity entity)
        {

            try
            {
                IRepository db = new RepositoryFactory().BaseRepository();
                EverydayPatrolEntity ee = db.FindEntity<EverydayPatrolEntity>(entity.PatrolId);
                if (ee != null)
                {
                    if (ee.PatrolTypeCode == "RJ" || ee.PatrolTypeCode == "ZJ")
                    {
                        ee.AffirmState = 2;
                    }
                    else {
                        Operator userLogin = ERCHTMS.Code.OperatorProvider.Provider.Current();
                        if (ee.DutyUserId == userLogin.Account)//当前用户为责任人
                        {
                            ee.AffirmState = 2;
                        }
                        else
                        {
                            //UserEntity user = db.FindEntity<UserEntity>(ee.DutyUserId);
                            ee.AffirmUserId = ee.DutyUserId;
                        }
                    }
                    
                    db.Update<EverydayPatrolEntity>(ee);
                }
            }
            catch { }
        }
        #endregion
    }
}
