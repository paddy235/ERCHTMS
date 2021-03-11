using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    public class DeptData
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public string deptid { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 新编码
        /// </summary>
        public string newcode { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 父编码
        /// </summary>
        public string parentcode { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        public string parentid { get; set; }
        /// <summary>
        /// 组织机构id
        /// </summary>
        public string oranizeid { get; set; }

        //是否是父节点
        public bool isparent { get; set; }
        //机构
        public int isorg { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public IList<DeptData> children { get; set; }

        /// <summary>
        /// 是否可选
        /// </summary>
        public string isoptional { get; set; }

        public string ManagerId { get; set; }
        public string Manager { get; set; }

        public string DeptType { get; set; }
    }
}