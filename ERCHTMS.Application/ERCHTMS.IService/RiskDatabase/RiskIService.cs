using ERCHTMS.Entity.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.RiskDatabase
{
    /// <summary>
    /// 描 述：安全风险库
    /// </summary>
    public interface RiskIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="workId">作业步骤ID</param>
        /// <param name="areaCode">区域Code</param>
        /// <param name="areaId">区域ID</param>
        /// <param name="grade">风险等级</param>
        ///  <param name="accType">事故类型</param>
        ///  <param name="deptCode">部门编码</param>
        ///  <param name="keyWord">查询关键字</param>
        /// <returns>返回列表</returns>
        IEnumerable<RiskEntity> GetList(string areaCode, string areaId, string grade, string accType, string deptCode, string keyWord);
         /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 风险清单导出
        /// </summary>
        /// <param name="queryJson">查询条件</param>
        /// <param name="riskType">风险类型</param>
        /// <param name="authType">授权范围</param>
        /// <param name="IndexState">是否首页跳转</param>
        /// <returns></returns>
        DataTable GetPageExportList(string queryJson, string riskType, string authType, string IndexState);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        RiskEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取数据
        /// </summary>
        ///  <param name="workId">作业步骤ID</param>
        /// <param name="dangerId">危险点ID</param>
        /// <param name="areaId">区域ID</param>
        /// <returns></returns>
        DataTable GetEntity(string workId, string dangerId, string areaId);
        /// <summary>
        /// 根据风险点ID获取部门Code（多个用英文逗号分割）
        /// </summary>
        /// <param name="dangerId">风险点ID</param>
        /// <returns></returns>
        string GetDeptCode(string dangerId);
        /// <summary>
        /// 首页风险工作指标统计
        /// </summary>
        /// <param name="user">当前用户对象</param>
        /// <returns></returns>
        decimal[] GetHomeStat(ERCHTMS.Code.Operator user);
         /// <summary>
        /// 首页风险排名
        /// </summary>
        /// <param name="user">当前用户对象</param>
        /// <returns></returns>
        string GetRiskRank(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取安全风险考核结果
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="time">考核时间，默认统计所有</param>
        /// <returns></returns>
        object GetRiskWarn(ERCHTMS.Code.Operator user,string time="");
           /// <summary>
        /// 计算最近半年的风险预警值
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetRiskValues(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 根据月份计算当月得分
        /// </summary>
        /// <param name="user"></param>
        /// <param name="time">月份，如2017-10-01</param>
        /// <returns></returns>
        decimal GetRiskValueByTime(ERCHTMS.Code.Operator user, string time);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        int RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        int SaveForm(string keyValue, RiskEntity entity);
        #endregion

        #region 手机app端使用
        #region  11.1-12.3 风险清单及辨识
        /// <summary>
        /// 11.1 风险清单列表
        /// </summary>
        /// <param name="type">查询方式，1：我的，2：重大风险，3：全部</param>
        /// <param name="areaId">区域ID</param>
        /// <param name="grade">风险级别</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        DataTable GetAreaList(int type, string areaId, string grade, ERCHTMS.Code.Operator user, string kind,string deptcode, string keyWord = "");
        /// <summary>
        /// 11.2 根据区域ID获取风险清单
        /// </summary>
        /// <param name="areaId">区域ID</param>
        /// <returns></returns>
        DataTable GetRiskList(Pagination pag);
        /// <summary>
        /// 11.3 根据风险ID获取风险详细信息
        /// </summary>
        /// <param name="riskId">风险记录ID</param>
        /// <returns></returns>
        object GetRisk(string riskId);

        /// <summary>11.4 区域</summary>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        DataTable GetAreas(ERCHTMS.Code.Operator user, string planId);
        /// <summary>12.1	获取辨识计划列表</summary>
        /// <returns></returns>
        DataTable GetPlanList(ERCHTMS.Code.Operator user, string condition);
        /// <summary>12.2	根据计划ID获取辨识区域</summary>
        /// <param name="planId">计划ID</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        DataTable GetAreaByPlanId(string planId, int status, ERCHTMS.Code.Operator user);
        /// <summary>12.3	根据计划ID和区域Code获取风险清单</summary>
        /// <param name="planId">计划ID</param>
        /// <param name="areaCode">区域Code</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        DataTable GetRiskListByPlanId(string planId, string areaCode, int status, ERCHTMS.Code.Operator user);
        /// <summary>
        /// 12.4 新增风险辨识信息
        /// </summary>
        /// <param name="assess">实体</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        int SaveRisk(RiskAssessEntity assess, ERCHTMS.Code.Operator user);

        /// <summary>
        /// 12.5 获取管控责任单位列表
        /// </summary>
        /// <param name="assess">实体</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        DataTable GetDeptList(ERCHTMS.Code.Operator user);
        /// <summary>
        /// 12.9 获取风险辨识岗位列表
        /// </summary>
        /// <param name="assess">实体</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        DataTable GetPostList(string deptCode, ERCHTMS.Code.Operator user);

        #endregion

        #region 13.1-13.2 岗位风险卡
        /// <summary>
        /// 13.1-13.2获取本人或全部岗位风险卡列表
        /// </summary>
        /// <param name="user"></param>
        /// <param name="mode">查询方式，0：本人，1：全部</param>
        /// <returns></returns>
        List<object> GetPostCardList(ERCHTMS.Code.Operator user, int mode = 0);

        /// <summary>
        /// 13.3 根据岗位Id获取风险详情
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="postId">岗位Id</param>
        /// <returns></returns>
        List<object> GetPostRiskList(ERCHTMS.Code.Operator user, string postId,string deptCode);

        #endregion

        #region 12.6-12.8 获取编码相关数据
        /// <summary>
        /// 获取危害属性，危害分类等编码
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        DataTable GetCodeList(string itemCode);

        #endregion

        #region 14.2 统计
        /// <summary>
        /// 风险统计
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        object GetStat(ERCHTMS.Code.Operator user,string deptcode="");
        #endregion

        #endregion
    }
}