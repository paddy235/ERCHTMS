using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Code;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：1.脚手架搭设、验收、拆除申请2.脚手架搭设、验收、拆除审批
    /// </summary>
    public interface ScaffoldIService
    {
        #region 获取数据

        /// <summary>
        /// 得到当前最大编号
        /// </summary>
        /// <returns></returns>
        string GetMaxCode();

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        DataTable GetList(Pagination page, string queryJson);
        /// <summary>
        /// 台账列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetLedgerList(Pagination page, string queryJson, string authType);


        /// <summary>
        /// 得到流程图
        /// </summary>
        /// <param name="keyValue">业务表ID</param>
        /// <param name="modulename">逐级审核模块名</param>
        /// <returns></returns>
        Flow GetFlow(string keyValue, string modulename);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ScaffoldEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取选择脚手架搭设和拆除
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetSelectPageList(Pagination pagination, string queryJson);


        /// <summary>
        /// 获取人员
        /// </summary>
        /// <param name="flowdeptid"></param>
        /// <param name="flowrolename"></param>
        /// <param name="type"></param>
        string GetUserName(string flowdeptid, string flowrolename, string type = "", string specialtytype = "");

        List<CheckFlowData> GetAppFlowList(string keyValue, string modulename);
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
        void SaveForm(string keyValue, ScaffoldEntity entity);


        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="model">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, ScaffoldModel model);


        /// <summary>
        /// 更新业务表、审核表、验收项目
        /// </summary>
        /// <param name="scaffoldEntity">业务主表实体</param>
        /// <param name="auditEntity">审核表实体</param>
        /// <param name="projects">验收项目 ScaffoldType=1 时才有</param>
        void UpdateForm(ScaffoldEntity scaffoldEntity, ScaffoldauditrecordEntity auditEntity, List<ScaffoldprojectEntity> projects);

        #endregion
    }
}
