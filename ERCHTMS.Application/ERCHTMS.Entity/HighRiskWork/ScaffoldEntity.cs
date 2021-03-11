using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：1.脚手架搭设、验收、拆除申请2.脚手架搭设、验收、拆除审批
    /// </summary>
    [Table("BIS_SCAFFOLD")]
    public class ScaffoldEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 申请单位ID
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCOMPANYID")]
        public string ApplyCompanyId { get; set; }
        /// <summary>
        /// 申请单位Code
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCOMPANYCODE")]
        public string ApplyCompanyCode { get; set; }
        /// <summary>
        /// 申请单位名称
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCOMPANYNAME")]
        public string ApplyCompanyName { get; set; }
        /// <summary>
        /// 申请人ID
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERID")]
        public string ApplyUserId { get; set; }

        /// <summary>
        /// 申请人名称
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERNAME")]
        public string ApplyUserName { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDATE")]
        public string ApplyDate { get; set; }

        /// <summary>
        /// 申请编号
        ///类型首字母+年份+3位数（如J2018001、J2018002）
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCODE")]
        public string ApplyCode { get; set; }
        /// <summary>
        /// 单位类别(0-单位内部 1-外包单位)
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCOMPANYTYPE")]
        public int? SetupCompanyType { get; set; }
        /// <summary>
        /// 脚手架搭设类型
        ///0-6米以下脚手架搭设申请
        ///1-6米以上脚手架搭设申请
        /// </summary>
        /// <returns></returns>
        [Column("SETUPTYPE")]
        public int? SetupType { get; set; }
        /// <summary>
        /// 脚手架搭设类型
        /// </summary>
        /// <returns></returns>
        [Column("SETUPTYPENAME")]
        public string SetupTypeName { get; set; }
        /// <summary>
        /// 使用单位ID
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCOMPANYID")]
        public string SetupCompanyId { get; set; }
        /// <summary>
        /// 使用单位Code
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCOMPANYCODE")]
        public string SetupCompanyCode { get; set; }
        /// <summary>
        /// 使用单位名称
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCOMPANYNAME")]
        public string SetupCompanyName { get; set; }


        /// <summary>
        /// 搭设/拆除单位ID
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCOMPANYID1")]
        public string SetupCompanyId1 { get; set; }
        /// <summary>
        /// 搭设/拆除单位Code
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCOMPANYCODE1")]
        public string SetupCompanyCode1 { get; set; }
        /// <summary>
        /// 搭设/拆除单位名称
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCOMPANYNAME1")]
        public string SetupCompanyName1 { get; set; }


        /// <summary>
        /// 工程ID
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTID")]
        public string OutProjectId { get; set; }
        /// <summary>
        /// 工程名称
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTNAME")]
        public string OutProjectName { get; set; }
        /// <summary>
        /// 搭设开始时间
        ///验收中为申请搭设时间
        /// </summary>
        /// <returns></returns>
        [Column("SETUPSTARTDATE")]
        public DateTime? SetupStartDate { get; set; }
        /// <summary>
        /// 搭设结束时间
        ///验收中为申请搭设时间
        /// </summary>
        /// <returns></returns>
        [Column("SETUPENDDATE")]
        public DateTime? SetupEndDate { get; set; }
        /// <summary>
        /// 作业区域
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREA")]
        public string WorkArea { get; set; }
        /// <summary>
        /// 搭设地点
        /// </summary>
        /// <returns></returns>
        [Column("SETUPADDRESS")]
        public string SetupAddress { get; set; }
        /// <summary>
        /// 搭设负责人
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCHARGEPERSON")]
        public string SetupChargePerson { get; set; }
        /// <summary>
        /// 搭设负责人ID
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCHARGEPERSONIDS")]
        public string SetupChargePersonIds { get; set; }
        /// <summary>
        /// 搭设人员ID
        ///人员多选，使用“，”号分隔
        /// </summary>
        /// <returns></returns>
        [Column("SETUPPERSONIDS")]
        public string SetupPersonIds { get; set; }
        /// <summary>
        /// 搭设人员
        ///人员多选，使用“，”号分隔
        /// </summary>
        /// <returns></returns>
        [Column("SETUPPERSONS")]
        public string SetupPersons { get; set; }
        /// <summary>
        /// 脚手架用途
        /// </summary>
        /// <returns></returns>
        [Column("PURPOSE")]
        public string Purpose { get; set; }
        /// <summary>
        /// 脚手架参数
        /// </summary>
        /// <returns></returns>
        [Column("PARAMETER")]
        public string Parameter { get; set; }
        /// <summary>
        /// 预计拆除时间
        /// </summary>
        /// <returns></returns>
        [Column("EXPECTDISMENTLEDATE")]
        public DateTime? ExpectDismentleDate { get; set; }
        /// <summary>
        /// 要求拆除时间
        /// </summary>
        /// <returns></returns>
        [Column("DEMANDDISMENTLEDATE")]
        public DateTime? DemandDismentleDate { get; set; }
        /// <summary>
        /// 实际搭设开始时间
        /// </summary>
        /// <returns></returns>
        [Column("ACTSETUPSTARTDATE")]
        public DateTime? ActSetupStartDate { get; set; }
        /// <summary>
        /// 实际搭设结束时间
        /// </summary>
        /// <returns></returns>
        [Column("ACTSETUPENDDATE")]
        public DateTime? ActSetupEndDate { get; set; }
        /// <summary>
        /// 拆除开始日期
        /// </summary>
        /// <returns></returns>
        [Column("DISMENTLESTARTDATE")]
        public DateTime? DismentleStartDate { get; set; }
        /// <summary>
        /// 拆除结束日期
        /// </summary>
        /// <returns></returns>
        [Column("DISMENTLEENDDATE")]
        public DateTime? DismentleEndDate { get; set; }
        /// <summary>
        /// 拆除部位
        /// </summary>
        /// <returns></returns>
        [Column("DISMENTLEPART")]
        public string DismentlePart { get; set; }
        /// <summary>
        /// 拆除原因
        /// </summary>
        /// <returns></returns>
        [Column("DISMENTLEREASON")]
        public string DismentleReason { get; set; }
        /// <summary>
        /// 拆除人员
        /// </summary>
        /// <returns></returns>
        [Column("DISMENTLEPERSONS")]
        public string DismentlePersons { get; set; }

        /// <summary>
        /// 拆除人员ID
        /// </summary>
        /// <returns></returns>
        [Column("DISMENTLEPERSONSIDS")]
        public string DismentlePersonsIds { get; set; }

        /// <summary>
        /// 架体材质
        /// </summary>
        /// <returns></returns>
        [Column("FRAMEMATERIAL")]
        public string FrameMaterial { get; set; }
        /// <summary>
        /// 搭设信息ID
        ///脚手架搭设信息ID，保留字段，验收及拆除时
        ///存入用户选择的信息ID
        /// </summary>
        /// <returns></returns>
        [Column("SETUPINFOID")]
        public string SetupInfoId { get; set; }
        /// <summary>
        /// 搭设信息Code
        /// </summary>
        /// <returns></returns>
        [Column("SETUPINFOCODE")]
        public string SetupInfoCode { get; set; }
        /// <summary>
        /// 脚手架类型
        ///0-搭设申请
        ///1-验收申请
        ///2-拆除申请
        /// </summary>
        /// <returns></returns>
        [Column("SCAFFOLDTYPE")]
        public int? ScaffoldType { get; set; }
        /// <summary>
        /// 审核状态
        ///脚手架类型为“搭设申请”时
        ///0-申请中
        ///1-审核中
        ///2-审核未通过
        ///3-审核通过
        ///脚手架类型为“验收申请”时
        ///0-申请中
        ///1-审核中
        ///2-审核未通过
        ///3-审核通过
        ///4-验收中
        ///5-验收未通过
        ///6-验收通过
        ///脚手架类型为“拆除申请”时
        ///0-申请中
        ///1-审核中
        ///2-审核未通过
        ///3-审核通过
        /// </summary>
        /// <returns></returns>
        [Column("AUDITSTATE")]
        public int? AuditState { get; set; }
        /// <summary>
        /// 措施落实人
        /// </summary>
        /// <returns></returns>
        [Column("MEASURECARRYOUT")]
        public string MeasureCarryout { get; set; }
        
        /// <summary>
        /// 措施落实人ID
        /// </summary>
        /// <returns></returns>
        [Column("MEASURECARRYOUTID")]
        public string MeasureCarryoutId { get; set; }
        /// <summary>
        /// 方案措施
        /// </summary>
        /// <returns></returns>
        [Column("MEASUREPLAN")]
        public string MeasurePlan { get; set; }


        /// <summary>
        /// 流程ID
        /// </summary>
        [Column("FLOWID")]
        public string FlowId { get; set; }

        /// <summary>
        /// 流程名
        /// </summary>
        [Column("FLOWNAME")]
        public string FlowName { get; set; }

        /// <summary>
        /// 流程角色ID
        /// </summary>
        [Column("FLOWROLEID")]
        public string FlowRoleId { get; set; }

        /// <summary>
        ///流程角色名
        /// </summary>
        [Column("FLOWROLENAME")]
        public string FlowRoleName { get; set; }
        /// <summary>
        /// 流程部门ID
        /// </summary>
        [Column("FLOWDEPTID")]
        public string FlowDeptId { get; set; }

        /// <summary>
        /// 流程部门名
        /// </summary>
        [Column("FLOWDEPTNAME")]
        public string FlowDeptName { get; set; }

        /// <summary>
        /// 确认结果（0：申请 1：确认 2：审核 3：完成）
        /// </summary>
        [Column("INVESTIGATESTATE")]
        public int? InvestigateState { get; set; }

        /// <summary>
        /// 验收照片
        /// </summary>
        [Column("ACCEPTFILEID")]
        public string AcceptFileId { get; set; }

        /// <summary>
        /// 实际拆除开始时间
        /// </summary>
        [Column("REALITYDISMENTLESTARTDATE")]
        public DateTime? RealityDismentleStartDate { get; set; }

        /// <summary>
        /// 实际拆除结束时间
        /// </summary>
        [Column("REALITYDISMENTLEENDDATE")]
        public DateTime? RealityDismentleEndDate { get; set; }

        /// <summary>
        ///专业类别
        /// </summary>
        [Column("SPECIALTYTYPE")]
        public string SpecialtyType { get; set; }

        /// <summary>
        ///审核备注
        /// </summary>
        [Column("FLOWREMARK")]
        public string FlowRemark { get; set; }


        /// <summary>
        /// 专业类别名称
        /// </summary>
        public string SpecialtyTypeName { get; set; }

        /// <summary>
        /// 抄送人
        /// </summary>
        [Column("COPYUSERNAMES")]
        public string CopyUserNames { get; set;  }

        /// <summary>
        /// 抄送人ID
        /// </summary>
        [Column("COPYUSERIDS")]
        public string CopyUserIds { get; set; }

        /// <summary>
        /// 作业区域Code
        /// </summary>
        [Column("WORKAREACODE")]
        public string WorkAreaCode { get; set; }

        /// <summary>
        /// 作业状态 0: 正常作业  1:暂停作业
        /// </summary>
        [Column("WORKOPERATE")]
        public string WorkOperate { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create(string keyValue)
        {
            this.Id = keyValue;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}