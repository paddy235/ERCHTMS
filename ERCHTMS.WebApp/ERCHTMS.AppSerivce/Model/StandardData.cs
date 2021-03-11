using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    public class StandardData
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 测量值
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// 接触限值
        /// </summary>
        public string maxValue { get; set; }
    }
}