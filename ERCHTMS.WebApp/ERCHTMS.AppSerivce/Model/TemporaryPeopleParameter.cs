using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    /// <summary>
    /// 新增临时人员接口参数实体
    /// </summary>
    public class TemporaryPeopleParameter
    {
        /// <summary>
        /// 真是名称
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string IdentifyID { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
    }
}