using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    /// <summary>
    /// 区域数据
    /// </summary>
    public class AreaData
    {
        /// <summary>
        /// 区域Id
        /// </summary>
        public string areaid { get; set; }
        /// <summary>
        /// 区域code 
        /// </summary>
        public string areacode { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string areaname { get; set; }
        /// <summary>
        /// 父区域id
        /// </summary>
        public string parentareaid { get; set; }
        /// <summary>
        /// 父区域code
        /// </summary>
        public string parentareacode { get; set; }
        /// <summary>
        /// 父区域名称
        /// </summary>
       // public string parentareaname { get; set; }
        /// <summary>
        /// 区域负责人
        /// </summary>
       // public string areadutyperson { get; set; }
        /// <summary>
        /// 区域负责人ID
        /// </summary>
       // public string areadutypersonid { get; set; }
        /// <summary>
        /// 区域负责人名称
        /// </summary>
       // public string areadutypersonname { get; set; }
        /// <summary>
        /// 区域负责人部门
        /// </summary>
       // public string chargedept { get; set; }
        /// <summary>
        /// 区域负责人部门编码
        /// </summary>
       // public string chargedeptcode { get; set; }
        /// <summary>
        /// 区域负责人部门ID
        /// </summary>
       // public string chargedeptid { get; set; }
        /// <summary>
        /// 区域负责人联系电话
        /// </summary>
       // public string areadutypersontel { get; set; }
        /// <summary>
        /// 是否父节点
        /// </summary>
        public bool isparent { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool isdefault { get; set; } 
        /// <summary>
        /// 子节点
        /// </summary>
        public IList<AreaData> children { get; set; }
    }
}