using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using ERCHTMS.Service.EmergencyPlatform;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq.Expressions;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急演练记录
    /// </summary>
    public class DrillplanrecordBLL
    {
        private IDrillplanrecordService service = new DrillplanrecordService();
        private DrillrecordevaluateIService evaluateService = new DrillrecordevaluateService();


        #region 获取数据

        #region 应急演练预案类型统计
        /// <summary>
        /// 应急演练预案类型统计
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetDrillPlanRecordTypeSta(string condition, int mode)
        {
            try
            {
                return service.GetDrillPlanRecordTypeSta(condition, mode);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable DrillplanStatList(string drillmode, bool isCompany, string deptCode, string starttime, string endtime)
        {
            try
            {
                return service.DrillplanStatList(drillmode, isCompany, deptCode, starttime, endtime);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable DrillplanStat(string drillmode, bool isCompany, string deptCode, string starttime, string endtime)
        {
            try
            {
                return service.DrillplanStat(drillmode, isCompany, deptCode, starttime, endtime);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable DrillplanStatDetail(string drillmode, bool isCompany, string deptCode, string starttime, string endtime)
        {
            try
            {
                return service.DrillplanStatDetail(drillmode, isCompany, deptCode, starttime, endtime);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public IEnumerable<DrillplanrecordEntity> GetListForCon(Expression<Func<DrillplanrecordEntity, bool>> condition)
        {
            return service.GetListForCon(condition);
        }

        public List<DrillrecordAssessEntity> GetAssessList(string drillrecordid) 
        {
            return service.GetAssessList(drillrecordid);
        }
        /// <summary>
        /// 获取流程状态图
        /// </summary>
        /// <param name="keyValue">工程Id</param>
        /// <returns></returns>
        public Flow GetEvaluateFlow(string keyValue)
        {
            return service.GetEvaluateFlow(keyValue);
        }
        public DataTable GetHistoryPageListJson(Pagination pagination, string queryJson)
        {
            return service.GetHistoryPageListJson(pagination, queryJson);
        }
        public DataTable GetAssessRecordList(Pagination pagination, string queryJson) {
            return service.GetAssessRecordList(pagination, queryJson);
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        public DataTable GetEvaluateList(Pagination pagination, string queryJson)
        {
            return evaluateService.GetEvaluateList(pagination, queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<DrillplanrecordEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取首页待评价数量
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetDrillPlanRecordEvaluateNum(Operator user)
        {
            return service.GetDrillPlanRecordEvaluateNum(user);
        }
        /// <summary>
        /// 根据当前登陆人获取首页待评估数量
        /// </summary>
        /// <returns></returns>
        public int GetDrillPlanRecordAssessNum(Operator user)
        {
            return service.GetDrillPlanRecordAssessNum(user);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DrillplanrecordEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取历史记录实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        public DrillplanrecordHistoryEntity GetHistoryEntity(string keyValue)
        {
            return service.GetHistoryEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, DrillplanrecordEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存历史记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveHistoryForm(string keyValue, DrillplanrecordHistoryEntity entity)
        {
            try
            {
                service.SaveHistoryForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取班组应急演练计划台账
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        public DataTable GetBZList(String strsql)
        {
            return service.GetBZList(strsql);
        }
        #endregion
    }
}
