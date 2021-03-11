using ERCHTMS.Entity.OccupationalHealthManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病危害因素监测
    /// </summary>
    public interface HazarddetectionIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<HazarddetectionEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        HazarddetectionEntity GetEntity(string keyValue);
        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageListByProc(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取全部列表数据
        /// </summary>
        /// <param name="riskid">危害因素</param>
        /// <param name="areaid">区域</param>
        /// <param name="starttime">时间范围</param>
        /// <param name="endtime">时间范围</param>
        /// <param name="isexcessive">是否超标</param>
        /// <param name="detectionuserid">检测人id</param>
        /// <returns></returns>
        DataTable GetDataTable(string queryJson, string where);

        /// <summary>
        /// 获取测量指标及标准
        /// </summary>
        /// <param name="RiskId">职业病id</param>
        /// <returns></returns>
        string GetStandard(string RiskId, string where);

        /// <summary>
        /// 获取危害因素监测统计数据
        /// </summary>
        /// <param name="year">哪一年数据</param>
        /// <param name="risk">职业病种类</param>
        /// <param name="type">true查询全部 false查询超标数据</param>
        /// <returns></returns>
        DataTable GetStatisticsHazardTable(int year, string risk, bool type, string where);
        #endregion

        #region 提交数据
        /// <summary>
        /// 根据id数组批量删除
        /// </summary>
        /// <param name="Ids"></param>
        void Remove(string Ids);
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
        void SaveForm(string keyValue, HazarddetectionEntity entity);
        #endregion

        #region 手机端
        /// <summary>
        /// 新增危险因素监测数据
        /// </summary>
        /// <param name="assess">实体</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        int SaveHazard(HazarddetectionEntity hazard, ERCHTMS.Code.Operator user);
        /// <summary>
        /// 获取全部列表数据
        /// </summary>
        /// <param name="riskid">危害因素</param>
        /// <param name="areaid">区域</param>
        /// <param name="starttime">时间范围</param>
        /// <param name="endtime">时间范围</param>
        /// <param name="isexcessive">是否超标</param>
        /// <param name="detectionuserid">检测人id</param>
        /// <returns></returns>
        DataTable GetDataTable(string riskid, string areaid, string starttime, string endtime, string isexcessive, string detectionuserid, string where);
        #endregion
    }
}
