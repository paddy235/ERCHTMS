using System.Collections.Generic;
using ERCHTMS.Entity.OutsourcingProject;
using System.Data;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.IService.OutsourcingProject
{
    public interface IToolsService
    {
        #region 获取数据

        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<ToolsEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ToolsEntity GetEntity(string keyValue);
        /// <summary>
        /// 得到流程图
        /// </summary>
        /// <param name="keyValue">业务表ID</param>
        /// <param name="modulename">逐级审核模块名</param>
        /// <returns></returns>
        Flow GetFlow(string keyValue, string modulename);
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
        void SaveForm(string keyValue,string type, ToolsEntity entity);
        #endregion
    }
}
