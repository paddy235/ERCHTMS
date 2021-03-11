using ERCHTMS.Entity.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.IService.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：五定安全检查
    /// </summary>
    public interface FivesafetycheckIService
    {
        #region 获取数据
        /// <summary>
        /// 输入名称和部门集合,返回最相似的词语
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetDeptByName(string name);
        /// <summary>
        /// 根据检查类型编号查询首页
        /// </summary>
        /// <param name="itemcode"></param>
        /// <returns></returns>
        DataTable DeskTotalByCheckType(string itemcode);

        /// <summary>
        /// 返回安全考核不同类型待审批的数量的数量
        /// </summary>
        /// <param name="fivetype">检查类型</param>
        /// <param name="istopcheck"> 0:上级公司检查 1：公司安全检查</param>
        /// <param name="type"> 0:审核流程，1：整改  2：验收</param>
        /// <returns></returns>
        string GetApplyNum(string fivetype, string istopcheck, string type);

        DataTable GetInfoBySql(string sql);
        DataTable ExportAuditTotal(string keyvalue);
        Flow GetAuditFlowData(string keyValue, string urltype);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<FivesafetycheckEntity> GetList(string queryJson);

        IEnumerable<UserEntity> GetStepDept(ManyPowerCheckEntity powerinfo, string id);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetPageListJson(Pagination pagination, string queryJson);

        /// <summary>
        ///  获取整改情况列表分页
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetAuditListJson(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        FivesafetycheckEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, FivesafetycheckEntity entity);
        #endregion
    }
}
