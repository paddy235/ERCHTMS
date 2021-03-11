using ERCHTMS.Entity.QuestionManage;
using ERCHTMS.IService.QuestionManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.QuestionManage
{
    /// <summary>
    /// 描 述：问题整改信息
    /// </summary>
    public class QuestionReformService : RepositoryFactory<QuestionReformEntity>, QuestionReformIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<QuestionReformEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public QuestionReformEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, QuestionReformEntity entity)
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


        #region 获取最近一条整改实体对象
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="LllegalId"></param>
        /// <returns></returns>
        public QuestionReformEntity GetEntityByBid(string questionId)
        {
            string sql = string.Format(@"select * from bis_questionreform where questionid ='{0}' order by autoid desc", questionId);
            return this.BaseRepository().FindList(sql).FirstOrDefault();
        }
        #endregion

        #region 获取历史的所有整改信息
        /// <summary>
        /// 获取历史的所有核准信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QuestionReformEntity> GetHistoryList(string questionId)
        {
            var list = this.BaseRepository().IQueryable().Where(p => p.QUESTIONID == questionId).OrderByDescending(p => p.AUTOID).OrderByDescending(p => p.CREATEDATE).ToList();
            if (list.Count() > 0)
            {
                list.RemoveAt(0);  //移除第一个
            }
            return list;
        }
        #endregion
    }
}