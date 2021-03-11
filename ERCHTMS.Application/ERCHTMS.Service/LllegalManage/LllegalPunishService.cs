using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.IService.LllegalManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.LllegalManage
{
    /// <summary>
    /// 描 述：违章责任人处罚信息
    /// </summary>
    public class LllegalPunishService : RepositoryFactory<LllegalPunishEntity>, LllegalPunishIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LllegalPunishEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LllegalPunishEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        #region 获取考核记录实体对象(mark:0表示违章责任人的记录 ,1 表示违章关联责任人的记录)
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        public LllegalPunishEntity GetEntityByBid(string LllegalId)
        {
            string sql = string.Format(@"select * from bis_lllegalpunish where lllegalid ='{0}' and mark ='0' order by autoid desc", LllegalId);
            return this.BaseRepository().FindList(sql).FirstOrDefault();
        }
        #endregion

        #region 获取考核记录集合对象(type:0表示违章责任人的记录 ,1 表示违章关联责任人的记录)
        /// <summary>
        /// 获取集合(type:0表示违章责任人的记录 ,1 表示违章关联责任人的记录)
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        public List<LllegalPunishEntity> GetListByLllegalId(string LllegalId,string type) 
        {
            string sql = string.Format(@"select * from bis_lllegalpunish where lllegalid ='{0}' ", LllegalId);
            if (!string.IsNullOrEmpty(type))
            {
                sql += string.Format(" and mark ='{0}'", type);
            }
            sql += "  order by autoid";
            return this.BaseRepository().FindList(sql).ToList(); ;
        }
        #endregion

        #region 获取考核记录集合对象(type:0表示违章责任人的记录 ,1 表示违章关联责任人的记录)
        /// <summary>
        /// 获取集合(type:0表示违章责任人的记录 ,1 表示违章关联责任人的记录)
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        public int DeleteLllegalPunishList(string LllegalId, string type)
        {
            string sql = string.Format(@"delete bis_lllegalpunish where lllegalid ='{0}'  ", LllegalId);
            if (!string.IsNullOrEmpty(type)) 
            {
                sql += string.Format(" and mark ='{0}'", type);
            }
            return this.BaseRepository().ExecuteBySql(sql);
        }
        #endregion

        #region 获取考核记录实体对象(mark:1表示主要负责人 0表示关联)
        /// <summary>
        /// 获取考核记录实体对象
        /// </summary>
        /// <param name="approveId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public LllegalPunishEntity GetEntityByApproveId(string approveId)
        {
            string sql = string.Format(@"select * from bis_lllegalpunish where approveid ='{0}'  order by autoid desc", approveId);
            return this.BaseRepository().FindList(sql).FirstOrDefault();
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
        public void SaveForm(string keyValue, LllegalPunishEntity entity)
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