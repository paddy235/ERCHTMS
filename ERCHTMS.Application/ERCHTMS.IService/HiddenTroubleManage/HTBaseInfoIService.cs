using ERCHTMS.Entity.BaseManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data;
using BSFramework.Application.Entity;
using ERCHTMS.Code;
using System.Collections;

namespace ERCHTMS.IService.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患基本信息表
    /// </summary>
    public interface HTBaseInfoIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表 
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<HTBaseInfoEntity> GetList(string queryJson);

        IList<HTBaseInfoEntity> GetListByCode(string hidcode);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        HTBaseInfoEntity GetEntity(string keyValue);
        /// <summary>
        /// 违章列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetRulerPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 按月统计当天登记的违章并且当天未验收的违章数量和当天登记的违章的总数量
        /// </summary>
        /// <param name="currDate">时间</param>
        ///  <param name="deptCode">部门Code</param>
        /// <returns></returns>
        DataTable GetLllegalRegisterNumByMonth(string currDate, string deptCode);
        /// <summary>
        /// 隐患统计
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="area"></param>
        /// <param name="hidrank"></param>
        /// <param name="userId"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        DataTable QueryStatisticsByAction(StatisticsEntity sentity);

        /// <summary>
        /// 电力安全隐患排查治理情况月报表
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="curdate"></param>
        /// <returns></returns>
        DataTable GetHiddenSituationOfMonth(string deptcode, string curdate, Operator curUser);

        #region 获取首页隐患统计
        /// <summary>
        /// 获取首页隐患统计
        /// </summary>
        /// <param name="orginezeId"></param>
        /// <param name="curYear"></param>
        /// <param name="topNum"></param>
        /// <returns></returns>
        DataTable GetHomePageHiddenByHidType(Operator curUser, int curYear, int topNum, int qType);
        #endregion

        #region 根据部门编码获取
        /// <summary>
        /// 根据部门编码获取
        /// </summary>
        /// <param name="orginezeId"></param>
        /// <param name="encode"></param>
        /// <param name="curYear"></param>
        /// <param name="qType"></param>
        /// <returns></returns>
        DataTable GetHomePageHiddenByDepart(string orginezeId, string encode, string curYear, int qType);
        #endregion

        /// <summary>
        /// 获取安全预警
        /// </summary>
        /// <returns></returns>
        DataTable GetHidSafetyWarning(int type, string orgcode);

        /// <summary>
        /// 隐患检查集合
        /// </summary>
        /// <param name="checkId"></param>
        /// <param name="checkman"></param>
        /// <returns></returns>
        DataTable GetList(string checkId, string checkman, string districtcode, string workstream);

        DataTable GetGeneralQuery(string sql, Pagination pagination);

        /// <summary>
        /// 获取通用查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        DataTable GetGeneralQueryBySql(string sql);

        DataTable GetHiddenByKeyValue(string keyValue);

        DataTable GetDescribeListByUserId(string userId, string hiddescribe);

        DataTable QueryHidWorkList(Operator curUser);

        DataTable QueryHidBacklogRecord(string value, string userId);

        DataTable QueryExposureHid(string num);

        DataTable GetAppHidStatistics(string code, int mode, int category);

        DataTable GetBaseInfoForApp(Pagination pagination);

        DataTable GetHiddenInfoOfWarning(Operator user, string startDate, string endDate);

        decimal GetHiddenWarning(Operator user, string startDate);

        object GetHiddenInfoOfEveryMonthWarning(Operator user, string startDate, string endDate);

        DataTable GetSafetyValueOfWarning(int action, string orgCode, string startDate, string endDate);

        #region 重要指标(省级)
        /// <summary>
        /// 重要指标(省级)
        /// </summary>
        /// <param name="action"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetImportantIndexForProvincial(int action, Operator user);
        #endregion


        /// <summary>
        /// 省级统计数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DataTable QueryProvStatisticsByAction(ProvStatisticsEntity entity);

        #region 获取关联对象下的隐患信息
        /// <summary>
        /// 获取关联对象下的隐患信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetHiddenByRelevanceId(Pagination pagination, string queryJson);
        #endregion
        #endregion

        #region 获取分页集合
        /// <summary>
        /// 获取分页集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        DataTable GetHiddenBaseInfoPageList(Pagination pagination, string queryJson);
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
        void SaveForm(string keyValue, HTBaseInfoEntity entity);
        #endregion

        #region MyRegion
        /// <summary>
        /// 导出安全检查对应的隐患内容
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetHiddenOfSafetyCheck(string keyValue, int mode);
        #endregion

        #region 江凌首页-未整改隐患
        /// <summary>
        /// 获取江凌首页-未整改隐患
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        DataTable GetNoChangeHidList(string code);
        IList GetCountByArea(List<string> areaCodes);
        #endregion
    }
}
