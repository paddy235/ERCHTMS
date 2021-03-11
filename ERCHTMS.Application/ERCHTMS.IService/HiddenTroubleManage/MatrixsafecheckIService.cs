using ERCHTMS.Entity.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace ERCHTMS.IService.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：矩阵安全检查计划
    /// </summary>
    public interface MatrixsafecheckIService
    {
        #region 获取数据
        string GetActionNum();
        /// <summary>
        /// 日历获取数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetCanlendarListJson(string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetPageListJson(Pagination pagination, string queryJson);
        DataTable GetInfoBySql(string sql);

        int ExecuteBySql(string sql);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<MatrixsafecheckEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        MatrixsafecheckEntity GetEntity(string keyValue);

        MatrixsafecheckEntity SetFormJson(string keyValue, string recid);

        DataTable GetContentPageJson(string queryJson);

        DataTable GetDeptPageJson(string queryJson);
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
        void SaveForm(string keyValue, MatrixsafecheckEntity entity);
        #endregion
    }
}
