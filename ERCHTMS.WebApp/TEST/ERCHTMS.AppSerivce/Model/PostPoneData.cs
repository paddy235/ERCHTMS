using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    public class PostPoneData
    {
        public string problemid { get; set; }
        public string hiddenid  { get; set; } 

        public string postponedays { get; set; }  //申请天数
        public string postponeresult { get; set; }  //审核处理结果
        public string applyreason { get; set; }// 延期申请原因  /处理意见
        public string applydate { get; set; } // 申请时间
        public string applyperson { get; set; } // 申请人 
        public string applypersonid { get; set; }  //申请人id
        public string applydept { get; set; }  //申请部门code
        public string applydeptname { get; set; }  //申请部门名称

        public string rankname { get; set; }
        public string workstream { get; set; } //流程状态
        public string handleid { get; set; }  //处理类型标识
        public string handlestate { get; set; } //延期状态
    }
}