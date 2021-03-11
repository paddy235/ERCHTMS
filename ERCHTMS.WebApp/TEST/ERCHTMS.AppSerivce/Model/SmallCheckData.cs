using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    public class SmallCheckData
    {
        /// <summary>
        /// 检查项目id
        /// </summary>
        public string checkid { get; set; }
        /// <summary>
        /// 检查项目
        /// </summary>
        public string checkcontent { get; set; }
        /// <summary>
        /// 检查结果
        /// </summary>
        public string checkresult { get; set; }
    }
}