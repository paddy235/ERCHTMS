using ERCHTMS.Entity.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.IService.RiskDatabase
{
    /// <summary>
    /// 描 述：安全风险库
    /// </summary>
    public interface RiskAssessIService
    {
        #region 获取数据
        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        IEnumerable<RiskAssessEntity> GetListFor(string queryJson);

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
        IEnumerable<RiskAssessEntity> GetList(string areaCode, string areaId, string grade, string accType, string deptCode, string keyWord);
        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<RiskAssessEntity> GetPageListRisk(Pagination pagination, string queryJson);
        IEnumerable<RiskAssessEntity> GetPageListRiskAll(string sql);
        DataTable GetPageList(Pagination pagination, string queryJson);

        DataTable GetPageControlListJson(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        RiskAssessEntity GetEntity(string keyValue);
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
        /// 根据部门编码获取风险数量统计图表数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        string GetRiskCountByDeptCode(string deptCode, string year = "", string riskGrade = "重大风险,较大风险,一般风险,低风险");
        /// <summary>
        /// 根据部门编码按区域获取风险数量统计图表数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        string GetAreaRiskCountByDeptCode(string deptCode, string year = "");
        /// <summary>
        /// 根据部门编码获取风险数量统计图表数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        string GetYearRiskCountByDeptCode(string deptCode, string year = "", string riskGrade = "重大风险,较大风险,一般风险,低风险", string areaCode = "");
        /// <summary>
        /// 根据部门编码获取风险对比分析图表数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        string GetRatherRiskCountByDeptCode(string deptCode, string year = "", string riskGrade = "重大风险,较大风险,一般风险,低风险", string areaCode = "");
        /// <summary>
        ///风险统计图表格
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        string GetAreaRiskList(string deptCode, string year = "", string riskGrade = "重大风险,较大风险,一般风险,低风险");
        /// <summary>
        /// 根据部门编码获取风险对比分析图表数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        string GetAreaRatherRiskStat(string deptCode, string year = "", string riskGrade = "重大风险,较大风险,一般风险,低风险");
        /// <summary>
        ///按部门获取风险对比数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        string GetDeptRiskList(string deptCode, string year = "", string riskGrade = "重大风险,较大风险,一般风险,低风险");
        /// <summary>
        /// 根据区域获取设备信息
        /// </summary>
        /// <param name="areaId">电厂区域Id</param>
        /// <returns></returns>
        DataTable GetEuqByAreaId(string areaId);

        /// <summary>
        /// 获取首页风险指标
        /// 省级使用
        /// </summary>
        /// <param name="riskGrade">风险等级</param>
        /// <param name="type">1:重大风险 2 较大风险 3 重大风险超过3个</param>
        /// <returns></returns>
        DataTable GetIndexRiskTarget(string riskGrade, Operator currUser, int type);

        DataTable FindTableBySql(string sql);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        int RemoveForm(string keyValue, string planId = "");
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        int SaveForm(string keyValue, RiskAssessEntity entity);

        /// <summary>
        /// 根据部门编码和查询关键字获取岗位风险卡列表
        /// </summary>
        /// <param name="queryCondition">查询条件</param>
        /// <returns></returns>
        int GetPostCardCount(string queryCondition);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        int Delete(string ids);
        #endregion

        List<RiskAssessEntity> RiskCount(bool includeDynamic);
        List<RiskAssessEntity> RiskCount2(string id);
        List<RiskAssessEntity> RiskCount3(string id);
        List<RiskAssessEntity> RiskCount4(string id);
    }
}