using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.LllegalManage;
using BSFramework.Util.WebControl;
using System.Data;

namespace ERCHTMS.IService.LllegalManage
{
    /// <summary>
    /// 描 述：违章基本信息
    /// </summary>
    public interface LllegalStatisticsIService
    {
        #region 统计图

        DataTable GetLllegalZgv(string deptcode);
        DataTable GetLllegalLevelTotal(string queryJson);
        DataTable GetLllegalLevelList(string queryJson);
        DataTable GetLllegalTypeTotal(string queryJson);
        DataTable GetLllegalTypeList(string queryJson);
        DataTable GetLllegalLevelTotalGrp(string queryJson);
        DataTable GetLllegalLevelListGrp(string queryJson);
        DataTable GetLllegalTypeTotalGrp(string queryJson);
        DataTable GetLllegalTypeListGrp(string queryJson);
        #endregion

        #region 趋势统计图
        DataTable GetLllegalTrendData(string queryJson);
        DataTable GetLllegalTrendDataGrp(string queryJson);
        #endregion
        DataTable GetJFStatis(string queryJson);

        #region 对比统计图
        DataTable GetLllegalCompareData(string queryJson);
        DataTable GetLllegalCompareDataGrp(string queryJson);
        #endregion

        #region 移动端违章统计
        /// <summary>
        /// 移动端违章统计
        /// </summary>
        /// <param name="code"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        DataTable GetAppLllegalStatistics(string code,string year, int mode);
        #endregion


        #region  获取前几个曝光的数据(取当前年的)
        /// <summary>
        /// 获取前几个曝光的数据(取当前年的)
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        DataTable QueryExposureLllegal(string num);
        #endregion

    }
}