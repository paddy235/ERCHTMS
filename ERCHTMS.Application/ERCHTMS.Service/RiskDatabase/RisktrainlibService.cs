using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Data;

namespace ERCHTMS.Service.RiskDatabase
{
    /// <summary>
    /// 描 述：风险预知训练库
    /// </summary>
    public class RisktrainlibService : RepositoryFactory<RisktrainlibEntity>, RisktrainlibIService
    {
        #region 获取数据

        public DataTable GetPageListJson(Pagination pagination, string queryJson) {
            DatabaseType dataType = DbHelper.DbType;
           
            var queryParam = queryJson.ToJObject();
            //查询关键字
            if (!queryParam["keyWord"].IsEmpty())
            {
                string keyWord = queryParam["keyWord"].ToString().Trim();
                pagination.conditionJson += string.Format(" and worktask like '%{0}%'", keyWord.Trim());
            }
            if (!queryParam["WorkType"].IsEmpty())
            {
                string WorkType = queryParam["WorkType"].ToString().Trim();
                pagination.conditionJson += string.Format(" and  (',' || WorkTypeCode || ',')  like '%,{0},%'", WorkType.Trim());
            }
            
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<RisktrainlibEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// 获取作业安全分析库
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetRisktrainlibList(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            string strwhere = "";
            if (!queryParam["worktask"].IsEmpty())
            {
                string worktask = queryParam["worktask"].ToString().Trim();
                strwhere = string.Format(" where  worktask like '%{0}%'", worktask.Trim());
            }

            string sql = "select * from BIS_RISKTRAINLIB  "  ;
            if (strwhere != "")
            {
                sql += strwhere;
            }
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RisktrainlibEntity GetEntity(string keyValue)
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
        /// 删除来源风险库数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public bool DelRiskData()
        {
            var res = this.BaseRepository().BeginTrans();
            try
            {
                string subSql = "DELETE FROM BIS_RISKTRAINLIBDETAIL T WHERE T.WORKID IN(SELECT ID FROM BIS_RISKTRAINLIB T WHERE T.DATASOURCES='1') ";
                res.ExecuteBySql(subSql);
                string sql = "DELETE FROM BIS_RISKTRAINLIB T WHERE T.DATASOURCES='1' ";
                res.ExecuteBySql(sql);
                res.Commit();
                return true;
            }
            catch (System.Exception)
            {
                res.Rollback();
                return false;
            }
            
        }

        public void SaveForm(string keyValue, RisktrainlibEntity entity) {
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
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, RisktrainlibEntity entity, List<RisktrainlibdetailEntity> listMesures)
        {
            entity.ID = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                var res = new Repository<RisktrainlibdetailEntity>(DbFactory.Base());
                RisktrainlibEntity re = this.BaseRepository().FindEntity(keyValue);
                if (re == null)
                {
                    entity.Create();
                    if (this.BaseRepository().Insert(entity) > 0)
                    {
                        for (int i = 0; i < listMesures.Count; i++)
                        {
                            listMesures[i].Create();
                        }
                        res.Insert(listMesures);
                    }
                }
                else
                {

                    entity.Modify(keyValue);
                    if (entity.ModifyNum == null) {
                        entity.ModifyNum = 0;
                    } 
                    entity.ModifyNum += 1;
                    if (this.BaseRepository().Update(entity) > 0)
                    {
                        int count = res.ExecuteBySql(string.Format("delete from bis_risktrainlibdetail where workid='{0}'", entity.ID));
                        for (int i = 0; i < listMesures.Count; i++)
                        {
                            listMesures[i].Create();
                        }
                        res.Insert(listMesures);
                    }
                }

            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        public void InsertRiskTrainLib(List<RisktrainlibEntity> RiskLib) {
            var res = DbFactory.Base().BeginTrans();
            try
            {

                res.Insert<RisktrainlibEntity>(RiskLib);
                res.Commit();
            }
            catch (System.Exception)
            {

                res.Rollback();
            }
        }
        public void InsertImportData(List<RisktrainlibEntity> RiskLib, List<RisktrainlibdetailEntity> detailLib) {
            var res = DbFactory.Base().BeginTrans();
            try
            {

                res.Insert<RisktrainlibEntity>(RiskLib);
                res.Insert<RisktrainlibdetailEntity>(detailLib);
                res.Commit();
            }
            catch (System.Exception)
            {

                res.Rollback();
            }
        }
        #endregion
    }
}
