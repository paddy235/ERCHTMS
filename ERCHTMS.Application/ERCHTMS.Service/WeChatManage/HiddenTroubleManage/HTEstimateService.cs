using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患评估表
    /// </summary>
    public class HTEstimateService : RepositoryFactory<HTEstimateEntity>, HTEstimateIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HTEstimateEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// 获取历史所有评估信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HTEstimateEntity> GetHistoryList(string hidCode)
        {
            var list = this.BaseRepository().IQueryable().Where(p => p.HIDCODE == hidCode).OrderByDescending(p => p.AUTOID).ToList();
            list = list.Where(p => p.ESTIMATERESULT == "1" || p.ESTIMATERESULT == "0").ToList();
            return list;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HTEstimateEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion


        /// <summary>
        /// 核准信息
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public HTEstimateEntity GetEntityByHidCode(string hidCode)
        {
            string sql = string.Format(@"select * from  bis_htestimate where hidcode ='{0}' order by autoid desc", hidCode);
            return this.BaseRepository().FindList(sql).FirstOrDefault();
        }

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }

        public void RemoveFormByCode(string hidcode)
        {
            string sql = string.Format(@" delete bis_htestimate where hidcode ='{0}' ", hidcode);
            this.BaseRepository().ExecuteBySql(sql);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, HTEstimateEntity entity)
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
