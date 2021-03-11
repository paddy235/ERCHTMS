using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.Web.Areas.KbsDeviceManage.Models
{
    public class TaskModel
    {
        public string Workno { get; set; }
        public string Taskname { get; set; }
        public string Taskcontent { get; set; }
        public string DangerLevel { get; set; }
        public string Taskregionid { get; set; }
        public string Taskregionname { get; set; }
        public string Taskregioncode { get; set; }
        public string Taskmanagename { get; set; }
        public string Taskmanageid { get; set; }
        public string Taskmembername { get; set; }
        public string Taskmemberid { get; set; }
        public string Guardianname { get; set; }
        public string Guardianid { get; set; }
        public string Deptname { get; set; }
        public string Deptid { get; set; }
        public string Deptcode { get; set; }
        public DateTime? Actualstarttime { get; set; }

        public DateTime? PlanenStarttime { get; set; }

        public DateTime? Planendtime { get; set; }
        public string Tasktype { get; set; }


        /// <summary>
        /// /工作签发人
        /// </summary>
        public string IssueName { get; set; }
        /// <summary>
        /// /工作签发人Id
        /// </summary>
        public string IssueUserid { get; set; }

        /// <summary>
        /// /工作许可人
        /// </summary>
        public string PermitName { get; set; }

        /// <summary>
        /// /工作许可人
        /// </summary>
        public string PermitUserid { get; set; } 

        /// <summary>
        /// 监护人（主管部门）
        /// </summary>
        public string ExecutiveNames { get; set; }
        /// <summary>
        /// 监护人（主管部门）
        /// </summary>
        public string ExecutiveIds { get; set; }

        /// <summary>
        /// 监护人（安全监察部门）
        /// </summary>
        public string SupervisionNames { get; set; }
        /// <summary>
        /// 监护人（安全监察部门）
        /// </summary>
        public string SupervisionIds { get; set; }



    }
}