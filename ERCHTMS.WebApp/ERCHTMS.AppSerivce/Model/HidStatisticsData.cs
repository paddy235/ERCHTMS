using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    /// <summary>
    /// 隐患统计
    /// </summary>
    public class HidStatisticsData
    {
        /// <summary>
        /// 隐患总数
        /// </summary>
        public string problemtotalnums { get; set; }
        /// <summary>
        /// 一般隐患
        /// </summary>
        public string normalproblemnums { get; set; }
        /// <summary>
        /// 一般隐患占比率
        /// </summary>
        public string normalproblemrate { get; set; }
        /// <summary>
        ///重大隐患
        /// </summary>
        public string seriousproblemnums { get; set; }
        /// <summary>
        /// 重大隐患占比率
        /// </summary>
        public string seriousproblemrate { get; set; }
        /// <summary>
        /// 隐患整改情况
        /// </summary>
        public IList<ProblemModify> problemmodifylist { get; set; }


    }

    public class ProblemModify
    {
        /// <summary>
        /// 隐患级别
        /// </summary>
        public string seriousproblemrate { get; set; }
        /// <summary>
        /// 已整改数
        /// </summary>
        public string modifynums { get; set; }
        /// <summary>
        /// 未整改数
        /// </summary>
        public string unmodifynums { get; set; }
        /// <summary>
        /// 整改率 
        /// </summary>
        public string modifyrate { get; set; }
    }
}