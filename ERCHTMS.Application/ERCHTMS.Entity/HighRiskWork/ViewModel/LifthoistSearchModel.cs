using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HighRiskWork.ViewModel
{
    public class LifthoistSearchModel
    {
        public string applycode { get; set; }
        public string applyuserid { get; set; }
        public string auditstate { get; set; }
        public string workstartdate { get; set; }
        public string workenddate { get; set; }
        public string constructionunitid { get; set; }
        public string viewrange { get; set; }
        public string toolname { get; set; }
        public string qualitytype { get; set; }
        /// <summary>
        /// 页面查询类型 0-查起重吊装列表 1-凭吊证列表
        /// </summary>
        public string pagetype { get; set; }

    }

    public class LifthoistViewModel
    {
        public string id { get; set; }
        public int? auditstate { get; set; }
        public string applycompanyid { get; set; }
        public string applycompanycode { get; set; }
        public string applycompanyname { get; set; }
        public string applyusername { get; set; }
        public string applyuserid { get; set; }
        public DateTime? applydate { get; set; }
        public string applycodestr { get; set; }
        public string constructionunitid { get; set; }
        public string constructionunitcode { get; set; }
        public string constructionunitname { get; set; }
        public string constructionaddress { get; set; }
        public string checkmajorid { get; set; }
        public string checkmajorcode { get; set; }
        public string checkmajorname { get; set; }
        public DateTime? workstartdate { get; set; }
        public DateTime? workenddate { get; set; }
        public string toolname { get; set; }
        public int? qualitytype { get; set; }
        public string chargepersonname { get; set; }
        public string chargepersonid { get; set; }
        public string guardianid { get; set; }
        public string guardianname { get; set; }
        public string hoistcontent { get; set; }

        public string drivername { get; set; }
        public string drivernumber { get; set; }
        public string fulltimename { get; set; }
        public string fulltimenumber { get; set; }


        public string chargepersonsign { get; set; }
        public string hoistareapersonnames { get; set; }
        public string hoistareapersonids { get; set; }
        public string hoistareapersonsigns { get; set; }

        public string lifthoistjobid { get; set; }
        public string deletefileids { get; set; }
        public string pagetype { get; set; }


        public string qualitytypename { get; set; }
        public string workdepttype { get; set; }
        public string engineeringname { get; set; }
        public string engineeringid { get; set; }
        public string specialtytype { get; set; }
        public string fazlfiles { get; set; }
        public string quality { get; set; }

        public string workareacode { get; set; }

        public string workareaname { get; set; }

        public string specialtytypename { get; set; }

        public string flowid { get; set; }

        public List<Photo> liftschemes { get; set; }
        public List<Photo> persondatas { get; set; }
        public List<Photo> driverdatas { get; set; }

        public List<Photo> liftfazls { get; set; }



        //安全措施
        public List<LifthoistsafetyEntity> safetys { get; set; }
        //审核记录
        public List<LifthoistauditrecordEntity> auditrecords { get; set; }
        //作业安全分析
        public List<HighRiskRecordEntity> riskrecord { get; set; }
        //人员信息
        public List<LifthoistpersonEntity> lifthoistperson { get; set; }

        /// <summary>
        /// 流程步骤
        /// </summary>
        public List<CheckFlowData> CheckFlow { get; set; }

    }
}
