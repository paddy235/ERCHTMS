using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.IService.LllegalManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.LllegalManage
{
    /// <summary>
    /// 描 述：反违章奖励表
    /// </summary>
    public class LllegalAwardDetailService : RepositoryFactory<LllegalAwardDetailEntity>, LllegalAwardDetailIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LllegalAwardDetailEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        #region 获取违章奖励信息
        /// <summary>
        /// 获取违章奖励信息
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        public List<LllegalAwardDetailEntity> GetListByLllegalId(string LllegalId)
        {
            string sql = string.Format(@"select * from bis_lllegalawarddetail where lllegalid ='{0}' order by createdate  ", LllegalId);
            return this.BaseRepository().FindList(sql).ToList(); ;
        }
        #endregion

        #region 删除违章奖励
        /// <summary>
        /// 删除违章奖励
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        public int DeleteLllegalAwardList(string LllegalId)
        {
            try
            {
                string sql = string.Format(@"delete bis_lllegalawarddetail where lllegalid ='{0}'  ", LllegalId);
                return this.BaseRepository().ExecuteBySql(sql);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LllegalAwardDetailEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, LllegalAwardDetailEntity entity)
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