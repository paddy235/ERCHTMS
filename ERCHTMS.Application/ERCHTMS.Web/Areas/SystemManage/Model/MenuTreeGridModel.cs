using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.Web.Areas.SystemManage.Model
{
    public class MenuTreeGridModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string  DeptId { get; set; }
        /// <summary>
        /// 数据来源  0 栏目  1 菜单
        /// </summary>
        public int DataSouceType { get; set; }
        public string Remark { get; set; }

        public string ParentName { get; set; }
        public string ParentId { get;  set; }
        public int? Sort { get;  set; }
        /// <summary>
        /// 栏目ID
        /// </summary>
        public string  ColumnId { get; set; }
    }
}