using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：入厂许可申请
    /// </summary>
    public interface IntromissionIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<IntromissionEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        IntromissionEntity GetEntity(string keyValue);



        #region 通过入厂许可获取外包工程相关信息

        /// <summary>
        /// 通过入厂许可获取外包工程相关信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetOutSourcingProjectByIntromId(string keyValue);
        #endregion

        /// <summary>  //获取审查记录信息
        /// 通过入厂许可申请ID获取对应的审查记录内容
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetDtRecordList(string keyValue);
        /// <summary>  //获取审查记录信息
        /// 通过开工申请ID获取对应的审查记录内容
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetStartRecordList(string keyValue);

        DataTable GetDataTableBySql(string sql);

        /// <summary>  //获取历史审查记录信息
        /// 通过入厂许可申请ID获取对应的审查记录内容
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetHistoryDtRecordList(string keyValue);

        DataTable GetHistoryStartRecordList(string keyValue);

        #region 获取审查数据
        /// <summary>
        /// 获取审查数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetIntromissionPageList(Pagination pagination, string queryJson);
        #endregion

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
        void SaveForm(string keyValue, IntromissionEntity entity);
        #endregion
    }
}