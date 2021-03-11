using BSFramework.Data.Repository;
using ERCHTMS.Code;
using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.RiskDatabase
{
    /// <summary>
    /// 描 述：风险预知训练库管控措施
    /// </summary>
    public class RisktrainlibdetailService : RepositoryFactory<RisktrainlibdetailEntity>, RisktrainlibdetailIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<RisktrainlibdetailEntity> GetList(string workId)
        {
            return this.BaseRepository().IQueryable().ToList().Where(x=>x.WorkId==workId).OrderByDescending(x=>x.CreateDate).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RisktrainlibdetailEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        public DataTable GetTrainLibDetail(string workId)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            DataTable dt = this.BaseRepository().FindTable(string.Format(@"select t.id,t.atrisk riskdesc,t.controls content,t.process from bis_risktrainlibdetail t where t.workid='{0}'", workId));
            return dt;
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
        public void SaveForm(string keyValue, RisktrainlibdetailEntity entity)
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

        public void InsertRiskTrainDetailLib(List<RisktrainlibdetailEntity> detailLib) {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                res.Insert<RisktrainlibdetailEntity>(detailLib);
                res.Commit();
            }
            catch (Exception)
            {
                res.Rollback();
            }
        }
        #endregion
    }
}
