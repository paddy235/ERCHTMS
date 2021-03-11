using ERCHTMS.Entity.EngineeringManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.EngineeringManage
{
    /// <summary>
    /// 描 述：危大工程管理
    /// </summary>
    public interface PerilEngineeringIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<PerilEngineeringEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        PerilEngineeringEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, PerilEngineeringEntity entity);
        #endregion

        #region 统计
        /// <summary>
        ///获取统计数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        string GetEngineeringCount(string year = "");

        /// <summary>
        ///获取统计表格数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        string GetEngineeringList(string year = "");

        /// <summary>
        ///获取方案、交底统计数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        string GetEngineeringFile(string year = "");

        /// <summary>
        ///获取方案、交底统计数据(表格)
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        string GetEngineeringFileGrid(string year = "");

        /// <summary>
        ///危大工程完成情况统计
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        string GetEngineeringCase(string year = "");

        /// <summary>
        ///危大工程完成情况统计（表格）
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        string GetEngineeringCaseGrid(string year = "");

        /// <summary>
        ///单位内部、各外委单位对比
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        string GetEngineeringContrast(string year = "", string month = "");

        /// <summary>
        ///单位内部、各外委单位对比（表格）
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        string GetEngineeringContrastGrid(string year = "", string month = "");
        #endregion

        #region 省级统计
        /// <summary>
        ///各电厂单位对比
        /// </summary>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        string GetEngineeringContrastForSJ(string year = "");

        /// <summary>
        /// 各电厂单位对比表格
        /// </summary>
        /// <param name="year">统计年份</param>
        /// <returns></returns>
        DataTable GetEngineeringContrastGridForSJ(string year = "");

        /// <summary>
        /// 工程类别统计表格
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        DataTable GetEngineeringCategoryGridForSJ(string year = "");

        /// <summary>
        /// 工程类别图形
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        string GetEngineeringCategoryForSJ(string year = "");

        /// <summary>
        /// 月度趋势图形
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        string GetEngineeringMonthForSJ(string year = "");

        /// <summary>
        /// 月度趋势表格
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        DataTable GetEngineeringMonthGridForSJ(string year = "");

        /// <summary>
        /// 获取工程类别
        /// </summary>
        /// <returns></returns>
        DataTable GetEngineeringType();
        #endregion

        string GetPerilForSJIndex(string queryJson);

        string GetPeril(string code = "", string st = "", string et = "", string keyword = "");


        #region app接口
        DataTable GetPerilEngineeringList(string sqlwhere);
        #endregion
    }
}
