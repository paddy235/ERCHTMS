using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ERCHTMS.Entity.HighRiskWork;

namespace ERCHTMS.AppSerivce.Model
{
    public class WorkSortData
    {
        /// <summary>
        /// 类别id
        /// </summary>
        public string sid { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        public string parentid { get; set; }

        //是否可选
        public  bool  isoptional { get; set; }

        /// <summary>
        /// 作业信息
        /// </summary>
        public DataTable superwork { get; set; }
    }
}