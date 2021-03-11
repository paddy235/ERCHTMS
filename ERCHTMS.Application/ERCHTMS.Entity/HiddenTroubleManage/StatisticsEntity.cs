using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// 统计参数
    /// </summary>
    public class StatisticsEntity
    {
        public string sDeptCode { get; set; }
        public string sYear { get; set; }
        public string startDate { get; set; }  //起始时间
        public string endDate { get; set; } //结束时间
        public string sArea { get; set; }
        public string sHidRank { get; set; }
        public string sUserId { get; set; }
        public string sDeptId { get; set; }
        public string sAction { get; set; }
        public string statType { get; set; } //统计类型
        public string sOrganize { get; set; }
        public string sOrganizeCode { get; set; }
        public string sCheckType { get; set; }
        public string isCheck { get; set; }
        public bool isCompany { get; set; }  //是否厂级
        /// <summary>
        /// 电厂或省公司的安全检查隐患
        /// </summary>
        public string sCType { get; set; }

        public int sMark { get; set; }  //标记字段，可用作不同类型查询差异化处理
    }

    /// <summary>
    /// 省级统计数据对象
    /// </summary>
    public class ProvStatisticsEntity  
    {
        public string sAction { get; set; } //请求动作
        public string statType { get; set; } //统计类型
        public string sDepartId { get; set; }  //机构id
        public string sDepartCode { get; set; } //部门编码
        public string sStartDate { get; set; } //起始时间
        public string sEndDate { get; set; } //截止时间 
        public string sArea { get; set; }  //区域范围
        public string sHidRank { get; set; } //隐患级别
        public string sYear { get; set; } //年度
    }


    public class TreeListForHidden 
    {
        public string createuserdeptcode { get; set; }
        public string fullname { get; set; }
        public decimal ordinaryhid { get; set; }
        public decimal importanhid { get; set; }
        public decimal total { get; set; }
        public string sortcode { get; set; }
        public string departmentid { get; set; }
        public string parent { get; set; } 

        public bool haschild { get; set; }
    }

    public class TreeListForHiddenSituation 
    {
        public string changedutydepartcode { get; set; } 
        public string fullname { get; set; }
        public decimal thenchange { get; set; }
        public decimal nonchange { get; set; }
        public decimal total { get; set; }
        public string sortcode { get; set; }
        public string departmentid { get; set; }
        public string parent { get; set; }

        public bool haschild { get; set; }
    }
}
