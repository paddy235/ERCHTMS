using ERCHTMS.Entity.EmergencyPlatform;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System;
using System.Linq.Expressions;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Code;

namespace ERCHTMS.IService.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急演练记录
    /// </summary>
    public interface IDrillplanrecordService
    {
        #region 获取数据


        #region 应急演练预案类型统计
        /// <summary>
        /// 应急演练预案类型统计
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        DataTable GetDrillPlanRecordTypeSta(string condition, int mode);
        #endregion
        DataTable DrillplanStatList(string drillmode, bool isCompany, string deptCode, string starttime, string endtime);
        DataTable DrillplanStat(string drillmode, bool isCompany, string deptCode, string starttime, string endtime);
        DataTable DrillplanStatDetail(string drillmode, bool isCompany, string deptCode, string starttime, string endtime);
        DataTable GetAssessRecordList(Pagination pagination, string queryJson);

        List<DrillrecordAssessEntity> GetAssessList(string drillrecordid);
        /// <summary>
        /// 获取首页待评价数量
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int GetDrillPlanRecordEvaluateNum(Operator user);
        /// <summary>
        /// 根据当前登陆人获取首页待评估数量
        /// </summary>
        /// <returns></returns>
        int GetDrillPlanRecordAssessNum(Operator user);
        /// <summary>
        /// 获取评价流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        Flow GetEvaluateFlow(string keyValue);
        IEnumerable<DrillplanrecordEntity> GetListForCon(Expression<Func<DrillplanrecordEntity, bool>> condition);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<DrillplanrecordEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        DrillplanrecordEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取历史记录实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        DrillplanrecordHistoryEntity GetHistoryEntity(string keyValue);
        /// <summary>
        /// 获取班组应急演练计划台账
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        DataTable GetBZList(String strsql);
        DataTable GetHistoryPageListJson(Pagination pagination, string queryJson);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, DrillplanrecordEntity entity);
        /// <summary>
        /// 保存历史记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        void SaveHistoryForm(string keyValue, DrillplanrecordHistoryEntity entity);
        #endregion
    }
}
