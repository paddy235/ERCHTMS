using ERCHTMS.Entity.HighRiskWork.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    public class IntromData
    {
        public string intromid { get; set; }  //历史记录id
        public string outengineer { get; set; } //外包工程名称
        public string outengineerid { get; set; }//工程id
        public string deptName { get; set; } //发包部门
        public string projectcode { get; set; } //工程编号
        public string projecttype { get; set; } //工程类型
        public string areaname { get; set; } //所属区域
        public string projectlevel { get; set; }//工程风险等级
        public string projectcontent { get; set; } //工程内容
        public string applypeople { get; set; }  //申请人
        public DateTime? applytime { get; set; } //申请时间

        public List<IntromDetail> detaildata { get; set; }  //审查内容

        public List<AuditResult> auditdata { get; set; }  //审核内容 
        public List<CheckFlowData> nodeList { get; set; }//审核流程图
    }

    public class IntromDetail
    {
        public string detailid { get; set; }
        /// <summary>
        /// 审查内容id
        /// </summary>
        public string contentid { get; set; }
        /// <summary>
        /// 审查内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 审查结果
        /// </summary>
        public string result { get; set; }
        /// <summary>
        /// 审查人
        /// </summary>
        public string peoplename { get; set; }
        public string peopleid { get; set; }
        public string signpic { get; set; }
    }


    /// <summary>
    /// 历史入厂许可申请对象
    /// </summary>
    public class HistoryIntrom
    {
        public string historyintromid { get; set; }  //历史记录id
        public string outengineer { get; set; } //外包工程名称
        public string applypeople { get; set; }  //申请人
        public DateTime? applytime { get; set; } //申请时间
    }

    /// <summary>
    /// 入厂许可申请对象
    /// </summary>
    public class IntromResult 
    {
        public string intromid { get; set; }  //历史记录id 
        public string outengineer { get; set; } //外包工程名称
        public string applypeople { get; set; }  //申请人
        public string state { get; set; }  //获取状态
        public DateTime? applytime { get; set; } //申请时间
        public string approveuserid { get; set; }
        public string approveusername { get; set; }
    }

    /// <summary>
    /// 审核记录
    /// </summary>
    public class AuditResult 
    {
        /// <summary>
        /// 审核结果
        /// </summary>
        public string approveresult { get; set; }
        /// <summary>
        /// 审核意见
        /// </summary>
        public string approveopinion { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string auditpeople { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string audittime { get; set; }
        /// <summary>
        /// 审核部门
        /// </summary>
        public string auditdept { get; set; }

        public string auditsignimg { get; set; }
    } 
}