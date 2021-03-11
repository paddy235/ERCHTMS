using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Code;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：资质审查基础信息表
    /// </summary>
    public interface AptitudeinvestigateinfoIService
    {
        #region 获取数据
        DataTable GetPageList(Pagination pagination, string queryJson);
     
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<AptitudeinvestigateinfoEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        AptitudeinvestigateinfoEntity GetEntity(string keyValue);
        /// <summary>
        /// 根基工程ID获取资质信息
        /// </summary>
        /// <param name="engineerid"></param>
        /// <returns></returns>
        AptitudeinvestigateinfoEntity GetEntityByOutEngineerId(string engineerid);
        /// <summary>
        /// 根据外包单位Id获取最近一次单位资质信息
        /// </summary>
        /// <param name="outprojectId">单位Id</param>
        /// <returns></returns>
        AptitudeinvestigateinfoEntity GetListByOutprojectId(string outprojectId);

        /// <summary>
        /// 根据外包工程Id获取最近一次单位资质信息
        /// </summary>
        /// <param name="outengineerId"></param>
        /// <returns></returns>
        AptitudeinvestigateinfoEntity GetListByOutengineerId(string outengineerId);
        /// <summary>
        /// 查询审核流程图
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="urlType">查询类型：1 单位资质 2 人员资质 3 特种设备验收 4 电动/安全工器具验收 5三措两案 6入厂许可 7开工申请</param>
        /// <returns></returns>
        Flow GetAuditFlowData(string keyValue, string urltype);
        /// <summary>
        /// 查询审核流程图-手机端使用
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="urltype">查询类型：1 单位资质 2 人员资质 3 特种设备验收 4 电动/安全工器具验收 5三措两案 6入厂许可 7开工申请 8日常考核</param>
        /// <returns></returns>
        List<CheckFlowData> GetAppFlowList(string keyValue, string urltype, Operator currUser);

        List<CheckFlowList> GetAppCheckFlowList(string keyValue, string urltype, Operator currUser);
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
        void SaveForm(string keyValue, AptitudeinvestigateinfoEntity entity);
        #endregion
    }
}
