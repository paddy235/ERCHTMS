using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Code
{
    /// <summary>
    /// 安全评估对象(计算安全评估值)
    /// </summary>
    public class SafetyAssessedModel
    {
        public string classificationcode { get; set; }
        public string classificationindex { get; set; }
        public decimal classificationscore { get; set; }
        public List<SafetyAssessedChildModel> data { get; set; }
    }


    public class SafetyAssessedChildModel
    {
        public string indexcode { get; set; } //ID
        public string indexname { get; set; }//指标项
        public string indexscore { get; set; } //指标总分
        public string indexstandard { get; set; }//评分标准
        public decimal deductpoint { get; set; }//扣分
        public decimal score { get; set; } //得分 
    }


    public class SafetyAssessedArguments
    {
        public string classificationcode { get; set; }
        public string orgId { get; set; }  
        public string orgCode { get; set; }  
        public string startDate { get; set; }  
        public string endDate { get; set; }
    }
}
