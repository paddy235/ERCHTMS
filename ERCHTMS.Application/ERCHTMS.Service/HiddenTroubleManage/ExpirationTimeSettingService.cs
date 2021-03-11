using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：到期时间设置表
    /// </summary>
    public class ExpirationTimeSettingService : RepositoryFactory<ExpirationTimeSettingEntity>, ExpirationTimeSettingIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ExpirationTimeSettingEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ExpirationTimeSettingEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion


        #region 删除条件下的所有数据

        /// <summary>
        /// 删除条件下的所有数据
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="modulename"></param>
        public void RemoveAll(string orgid, string modulename)
        {
            try
            {
                string sql = string.Format(@"delete BIS_EXPIRATIONTIMESETTING where ORGANIZEID ='{0}' and  MODULENAME ='{1}' ", orgid, modulename);

                this.BaseRepository().ExecuteBySql(sql);
            }
            catch (System.Exception ex)
            {
                throw ex;
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
        public void SaveForm(string keyValue, ExpirationTimeSettingEntity entity)
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