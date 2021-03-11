using BSFramework.Data.Repository;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Data;
using BSFramework.Util.WebControl;

namespace ERCHTMS.Service.EmergencyPlatform
{
    /// <summary>
    /// 描 述：演练记录评价表
    /// </summary>
    public class DrillrecordevaluateService : RepositoryFactory<DrillrecordevaluateEntity>, DrillrecordevaluateIService
    {
        PeopleReviewIService peopleReview = new PeopleReviewService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<DrillrecordevaluateEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        public DataTable GetEvaluateList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["drillrecordid"].IsEmpty())
            {
                string drillrecordid = queryParam["drillrecordid"].ToString();
                pagination.conditionJson += string.Format(" and drillrecordid = '{0}'", drillrecordid);
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DrillrecordevaluateEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, DrillrecordevaluateEntity entity)
        {
            Operator currUser = OperatorProvider.Provider.Current();
            //开始事务
            var res = DbFactory.Base().BeginTrans();
            try
            {
                Repository<DrillplanrecordEntity> planRecord = new Repository<DrillplanrecordEntity>(DbFactory.Base());
                DrillplanrecordEntity Record = planRecord.FindEntity(entity.DrillRecordId);
                string moduleName = "厂级演练记录评价";
                if (!string.IsNullOrWhiteSpace(Record.DrillLevel))
                {
                    switch (Record.DrillLevel)
                    {
                        case "厂级":
                            moduleName = "厂级演练记录评价";
                            break;
                        case "部门级":
                            moduleName = "部门级演练记录评价";
                            break;
                        case "班组级":
                            moduleName = "班组级演练记录评价";
                            break;
                        default:
                            break;
                    }
                }
                ManyPowerCheckEntity nextCheck = peopleReview.CheckEvaluateForNextFlow(currUser, moduleName, Record);
                if (nextCheck == null)
                {
                    //评价结束
                    Record.IsOverEvaluate = 1;
                    Record.EvaluateRole = "";
                    Record.EvaluateDeptId = "";
                    Record.EvaluateDeptCode = "";
                    Record.EvaluateRoleId = "";
                }
                else
                {
                    Record.EvaluateRole = nextCheck.CHECKROLENAME;
                    Record.EvaluateDeptId = nextCheck.CHECKDEPTID;
                    Record.EvaluateDeptCode = nextCheck.CHECKDEPTCODE;
                    Record.NodeId = nextCheck.ID;
                    Record.NodeName = nextCheck.FLOWNAME;
                    Record.EvaluateRoleId = nextCheck.CHECKROLEID;
                    Record.IsOverEvaluate = 0;
                }
                res.Update<DrillplanrecordEntity>(Record);
                entity.Create();
                entity.SingImg = string.IsNullOrWhiteSpace(entity.SingImg) ? "" : entity.SingImg.ToString().Replace("../..", "");
                res.Insert<DrillrecordevaluateEntity>(entity);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
           
        }
        /// <summary>
        /// 保存评估记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void AssessRecordSaveForm(string keyValue, DrillrecordAssessEntity entity) {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                Repository<DrillrecordAssessEntity> planRecord = new Repository<DrillrecordAssessEntity>(DbFactory.Base());
                entity.Create();
                entity.AssessRecordSignImg = string.IsNullOrWhiteSpace(entity.AssessRecordSignImg) ? "" : entity.AssessRecordSignImg.ToString().Replace("../..", "");
                res.Insert<DrillrecordAssessEntity>(entity);
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
