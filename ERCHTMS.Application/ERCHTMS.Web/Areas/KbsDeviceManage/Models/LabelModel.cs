using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.Web.Areas.KbsDeviceManage.Models
{
    public class LableModel 
    {
        public string Id { get; set; }
        public string LabelId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string LableTypeName { get; set; }
        public string LableTypeId { get; set; }
        public string IdCardOrDriver { get; set; }
        public string Phone { get; set; }
        public string Power { get; set; }
        public DateTime? BindTime { get; set; }
        public string Operator { get; set; }
        public string DeptId { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
    }
}