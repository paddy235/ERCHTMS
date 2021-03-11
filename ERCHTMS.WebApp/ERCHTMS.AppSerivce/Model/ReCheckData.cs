using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    /// <summary>
    /// 复查对象
    /// </summary>
    public class ReCheckData
    {
        public string recheckstimes { get; set; }  //序号
        public string rechecksdate { get; set; } //复查日期
        public string problemid { get; set; } //隐患编码
        public string recheckspersonid { get; set; } //复查人id
        public string rechecksperson { get; set; }  //复查人姓名
        public string rechecksdepartcode { get; set; } //复查部门编码
        public string rechecksdepartname { get; set; } //复查部门名称
        public string rechecksstatus { get; set; } //复查结果
        public string rechecksidea { get; set; } //复查意见
    }
}