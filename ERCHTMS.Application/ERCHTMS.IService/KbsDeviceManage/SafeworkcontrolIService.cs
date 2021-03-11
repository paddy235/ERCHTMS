using ERCHTMS.Entity.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.RiskDatabase;

namespace ERCHTMS.IService.KbsDeviceManage
{
    /// <summary>
    /// 描 述：作业现场安全管控 
    /// </summary>
    public interface SafeworkcontrolIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<SafeworkcontrolEntity> GetList(string queryJson);
        /// <summary>
        /// 根据状态获取现场作业信息
        /// </summary>
        /// <param name="State">1开始 2结束</param>
        /// <returns></returns>
        IEnumerable<SafeworkcontrolEntity> GetStartList(int State);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SafeworkcontrolEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取预警列表信心
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        List<WarningInfoEntity> GetWarningInfoList(int type);
        /// <summary>
        /// 获取所有预警信息列表
        /// </summary>
        /// <returns></returns>
        List<WarningInfoEntity> GetWarningAllList();
        /// <summary>
        /// 获取人员安全管控各个时段人数
        /// </summary>
        /// <returns></returns>
        List<KbsEntity> GetDayTimeIntervalUserNum();
        /// <summary>
        /// 数据列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取到所有当前开始的作业
        /// </summary>
        /// <returns></returns>
        List<SafeworkcontrolEntity> GetNowWork();

        /// <summary>
        /// 获取今日高风险作业
        /// </summary>
        /// <returns></returns>    
        List<SafeworkcontrolEntity> GetDangerWorkToday(string level);
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
        void SaveForm(string keyValue, SafeworkcontrolEntity entity);

        /// <summary>
        /// 更新作业成员是否监管区域内状态
        /// </summary>
        /// <param name="workid"></param>
        /// <param name="userid"></param>
        /// <param name="state"></param>
        void SaveSafeworkUserStateIofo(string workid, string userid, int state);

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void AppSaveForm(string keyValue, SafeworkcontrolEntity entity);
        /// <summary>
        /// 获取预警实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        WarningInfoEntity GetWarningInfoEntity(string keyValue);
        /// <summary>
        /// 保存预警（新增、修改）
        /// </summary>
        /// <param name="type">0新增 1修改</param>
        /// <param name="list"></param>
        void SaveWarningInfoForm(int type, IList<WarningInfoEntity> list);
        /// <summary>
        /// 保存预警（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity"></param>
        void SaveWarningInfoForm(string keyValue, WarningInfoEntity entity);
        /// <summary>
        /// 删除预警
        /// </summary>
        /// <param name="keyValue">主键</param>
        void DelWarningInForm(string keyValue);
        /// <summary>
        /// 批量删除预警信息（通过主表记录Id）
        /// </summary>
        /// <param name="BaseId"></param>
        void DelBatchWarningInForm(string BaseId);

        #endregion

        List<WarningInfoEntity> GetBatchWarningInfoList(string workid);
        List<RiskAssessEntity> GetDistrictLevel();
    }
}
