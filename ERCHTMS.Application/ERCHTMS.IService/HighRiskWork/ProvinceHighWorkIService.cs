using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util.WebControl;

namespace ERCHTMS.IService.HighRiskWork
{
    public interface ProvinceHighWorkIService
    {
        #region 统计
        /// <summary>
        /// 按作业类型统计
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        string GetProvinceHighCount(string starttime, string endtime, string deptid, string deptcode);

        /// <summary>
        ///作业类型统计(统计表格)
        /// </summary>
        /// <returns></returns>
        string GetProvinceHighList(string starttime, string endtime, string deptid, string deptcode);
       
        /// <summary>
        /// 单位对比(统计图)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        string GetProvinceHighDepartCount(string starttime, string endtime);

        /// <summary>
        /// 单位对比(表格)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        string GetProvinceHighDepartList(string starttime, string endtime);
        #endregion

        /// <summary>
        /// 获取高风险作业列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageDataTable(Pagination pagination, string queryJson);


        #region 手机端作业统计
        DataTable AppGetHighWork(string starttime, string endtime, string deptid, string deptcode);
        #endregion
    }
}
