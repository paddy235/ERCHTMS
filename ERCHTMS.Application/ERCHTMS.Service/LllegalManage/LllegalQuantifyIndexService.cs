using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.IService.LllegalManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.LllegalManage
{
    /// <summary>
    /// 描 述：违章量化指标表
    /// </summary>
    public class LllegalQuantifyIndexService : RepositoryFactory<LllegalQuantifyIndexEntity>, LllegalQuantifyIndexIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LllegalQuantifyIndexEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }


        /// <summary>
        /// 获取是否存在记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int GetScalar(LllegalQuantifyIndexEntity entity)
        {
            try
            {
                string yearmonth = string.Empty;
                if (!string.IsNullOrEmpty(entity.MONTHVALUE))
                {
                    yearmonth = "'" + entity.YEARVALUE + "-" + entity.MONTHVALUE.Replace(",", "','" + entity.YEARVALUE + "-" + "") + "'";
                }
                string sql  = string.Format(@"select count(1) from ( 
                                    select a.deptid,a.dutyid,a.indexvalue,a.yearvalue,(a.yearvalue ||'-'|| b.month) yearmonth from bis_lllegalquantifyindex a
                                    left join  ( select lpad(level,2,0) as month from dual connect by level <13) b on instr(a.monthvalue,b.month)>0 
                                ) where deptid='{0}' and  dutyid='{1}' and yearmonth in ({2})", entity.DEPTID, entity.DUTYID, yearmonth);

                return int.Parse(this.BaseRepository().FindObject(sql).ToString());
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LllegalQuantifyIndexEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, LllegalQuantifyIndexEntity entity)
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