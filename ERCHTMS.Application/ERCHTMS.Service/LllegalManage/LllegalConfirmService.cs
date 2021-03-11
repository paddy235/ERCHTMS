using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.IService.LllegalManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.LllegalManage
{
    /// <summary>
    /// 描 述：违章验收确认信息
    /// </summary>
    public class LllegalConfirmService : RepositoryFactory<LllegalConfirmEntity>, LllegalConfirmIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LllegalConfirmEntity> GetList(string queryJson)
        {
            string sql = string.Format(@"select * from BIS_lllegalConfirm where 1=1 {0} order by autoid desc", queryJson);
            return this.BaseRepository().FindList(sql);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LllegalConfirmEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        #region 获取最近一条验收确认实体对象
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        public LllegalConfirmEntity GetEntityByBid(string LllegalId)
        {
            string sql = string.Format(@"select * from BIS_lllegalConfirm where lllegalid ='{0}' order by autoid desc", LllegalId);
            return this.BaseRepository().FindList(sql).FirstOrDefault();
        }
        #endregion

        #region 获取历史的所有验收确认信息
        /// <summary>
        /// 获取历史的所有验收信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LllegalConfirmEntity> GetHistoryList(string LllegalId)
        {
            var list = this.BaseRepository().IQueryable().Where(p => p.LLLEGALID == LllegalId).OrderByDescending(p => p.AUTOID).ToList();
            list = list.Where(p => p.CONFIRMRESULT == "1" || p.CONFIRMRESULT == "0").ToList();
            return list;
        }
        #endregion

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
        public void SaveForm(string keyValue, LllegalConfirmEntity entity)
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