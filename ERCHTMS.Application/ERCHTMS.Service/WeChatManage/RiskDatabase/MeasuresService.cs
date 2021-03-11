using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util.Extension;
using System.Data.Common;
using BSFramework.Data;

namespace ERCHTMS.Service.RiskDatabase
{
    /// <summary>
    /// 描 述：风险管控措施表
    /// </summary>
    public class MeasuresService : RepositoryFactory<MeasuresEntity>, MeasuresIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<MeasuresEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        ///根据区域获取列表
        /// </summary>
        /// <param name="areaId">区域Id</param>
        /// <param name="riskId">风险库记录ID</param>
        /// <returns></returns>
        public IEnumerable<MeasuresEntity> GetList(string areaId,string riskId)
        {
            var expression = LinqExtensions.True<MeasuresEntity>();
            if (!string.IsNullOrEmpty(areaId))
            {
                expression = expression.And(t => t.AreaId == areaId);
            }
            if (!string.IsNullOrEmpty(riskId))
            {
                expression = expression.And(t => t.RiskId == riskId);
            }
            return this.BaseRepository().IQueryable(expression).OrderBy(t => t.CreateDate).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public MeasuresEntity GetEntity(string keyValue)
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
        /// 删除数据
        /// </summary>
        /// <param name="riskId">风险库记录Id</param>
        public int Remove(string riskId)
        {
            DbParameter[] parameter = 
            {
                  DbParameters.CreateDbParameter("@RiskId", riskId)
            };
            return this.BaseRepository().ExecuteBySql("delete from BIS_MEASURES where riskid=@RiskId",parameter);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, List<MeasuresEntity> list)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                foreach(MeasuresEntity entity in list)
                {
                   entity.Modify(keyValue);
                   this.BaseRepository().Update(entity);
                }
            }
            else
            {
                foreach (MeasuresEntity entity in list)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
            }
        }
        public void Save(string keyValue, MeasuresEntity entity)
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