using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using BSFramework.Application.Entity;
using ERCHTMS.Code;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：
    /// </summary>
    public class ProvinceHighWorkBLL
    {
        private ProvinceHighWorkIService service = new ProvinceHighWorkService();


        #region 统计
        /// <summary>
        /// 按作业类型统计
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetProvinceHighCount(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.GetProvinceHighCount(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        ///作业类型统计(统计表格)
        /// </summary>
        /// <returns></returns>
        public string GetProvinceHighList(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.GetProvinceHighList(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        /// 单位对比(统计图)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public string GetProvinceHighDepartCount(string starttime, string endtime)
        {
            return service.GetProvinceHighDepartCount(starttime, endtime);
        }

        /// <summary>
        ///单位对比(统计表格)
        /// </summary>
        /// <returns></returns>
        public string GetProvinceHighDepartList(string starttime, string endtime)
        {
            return service.GetProvinceHighDepartList(starttime, endtime);
        }
        #endregion

        /// <summary>
        /// 获取高风险作业列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            return service.GetPageDataTable(pagination, queryJson);
        }

        #region 手机端高风险作业统计
        public DataTable AppGetHighWork(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.AppGetHighWork(starttime, endtime, deptid, deptcode);
        }
        #endregion
    }
}
