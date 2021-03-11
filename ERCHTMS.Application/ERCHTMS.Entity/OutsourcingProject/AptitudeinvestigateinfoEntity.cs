using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：资质审查基础信息表
    /// </summary>
    [Table("EPG_APTITUDEINVESTIGATEINFO")]
    public class AptitudeinvestigateinfoEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// 外包单位Id
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTID")]
        public string OUTPROJECTID { get; set; }
        /// <summary>
        /// 外包工程Id
        /// </summary>
        /// <returns></returns>
        [Column("OUTENGINEERID")]
        public string OUTENGINEERID { get; set; }
        /// <summary>
        /// 营业执照--社会信用代码
        /// </summary>
        /// <returns></returns>
        [Column("BUSCODE")]
        public string BUSCODE { get; set; }
        /// <summary>
        /// 营业执照--发证机关
        /// </summary>
        /// <returns></returns>
        [Column("BUSCERTIFICATE")]
        public string BUSCERTIFICATE { get; set; }
        /// <summary>
        /// 营业执照--有效时间起
        /// </summary>
        /// <returns></returns>
        [Column("BUSVALIDSTARTTIME")]
        public DateTime? BUSVALIDSTARTTIME { get; set; }
        /// <summary>
        /// 营业执照--有效时间止
        /// </summary>
        /// <returns></returns>
        [Column("BUSVALIDENDTIME")]
        public DateTime? BUSVALIDENDTIME { get; set; }
        /// <summary>
        /// 安全生产许可证--社会信用代码
        /// </summary>
        /// <returns></returns>
        [Column("SPLCODE")]
        public string SPLCODE { get; set; }
        /// <summary>
        /// 安全生产许可证--发证机关
        /// </summary>
        /// <returns></returns>
        [Column("SPLCERTIFICATE")]
        public string SPLCERTIFICATE { get; set; }
        /// <summary>
        /// 安全生产许可证--有效时间起
        /// </summary>
        /// <returns></returns>
        [Column("SPLVALIDSTARTTIME")]
        public DateTime? SPLVALIDSTARTTIME { get; set; }
        /// <summary>
        /// 安全生产许可证--有效时间止
        /// </summary>
        /// <returns></returns>
        [Column("SPLVALIDENDTIME")]
        public DateTime? SPLVALIDENDTIME { get; set; }
        /// <summary>
        /// 资质证件---资质证件号
        /// </summary>
        /// <returns></returns>
        [Column("CQCODE")]
        public string CQCODE { get; set; }
        /// <summary>
        /// 资质证件---发证机关
        /// </summary>
        /// <returns></returns>
        [Column("CQORG")]
        public string CQORG { get; set; }
        /// <summary>
        /// 资质证件---资质范围
        /// </summary>
        /// <returns></returns>
        [Column("CQRANGE")]
        public string CQRANGE { get; set; }
        /// <summary>
        /// 资质证件---发证等级
        /// </summary>
        /// <returns></returns>
        [Column("CQLEVEL")]
        public string CQLEVEL { get; set; }
        /// <summary>
        /// 资质证件---有效期起
        /// </summary>
        /// <returns></returns>
        [Column("CQVALIDSTARTTIME")]
        public DateTime? CQVALIDSTARTTIME { get; set; }
        /// <summary>
        /// 资质证件---有效期止
        /// </summary>
        /// <returns></returns>
        [Column("CQVALIDENDTIME")]
        public DateTime? CQVALIDENDTIME { get; set; }
        /// <summary>
        /// 备注字段
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string REMARK { get; set; }
        /// <summary>
        /// 0:保存 1:已提交
        /// </summary>
        /// <returns></returns>
        [Column("ISSAVEORCOMMIT")]
        public string ISSAVEORCOMMIT { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        /// <returns></returns>
        [Column("LINKMAN")]
        public string LINKMAN { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        /// <returns></returns>
        [Column("LINKMANPHONE")]
        public string LINKMANPHONE { get; set; }
        /// <summary>
        /// 法人代表
        /// </summary>
        /// <returns></returns>
        [Column("LEGALREP")]
        public string LEGALREP { get; set; }
        /// <summary>
        /// 企业地址
        /// </summary>
        /// <returns></returns>
        [Column("ADDRESS")]
        public string ADDRESS { get; set; }
        /// <summary>
        /// 法人代表电话
        /// </summary>
        /// <returns></returns>
        [Column("LEGALREPPHONE")]
        public string LEGALREPPHONE { get; set; }
        /// <summary>
        /// 外包单位概况
        /// </summary>
        /// <returns></returns>
        [Column("GENERALSITUATION")]
        public string GENERALSITUATION { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        /// <returns></returns>
        [Column("EMAIL")]
        public string EMAIL { get; set; }
        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        /// <returns></returns>
        [Column("CREDITCODE")]
        public string CREDITCODE { get; set; }
        /// <summary>
        /// 联系人传真
        /// </summary>
        /// <returns></returns>
        [Column("LINKMANFAX")]
        public string LINKMANFAX { get; set; }
        /// <summary>
        /// 法人代表传真
        /// </summary>
        /// <returns></returns>
        [Column("LEGALREPFAX")]
        public string LEGALREPFAX { get; set; }
        /// <summary>
        /// 外包单位工程负责人
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERDIRECTOR")]
        public string ENGINEERDIRECTOR { get; set; }
        /// <summary>
        /// 安全管理人数
        /// </summary>
        /// <returns></returns>
        [Column("SAFEMANAGERPEOPLE")]
        public int? SAFEMANAGERPEOPLE { get; set; }
        /// <summary>
        /// 工程作业人数
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERWORKPEOPLE")]
        public int? ENGINEERWORKPEOPLE { get; set; }
        /// <summary>
        /// 工程负责人电话
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERDIRECTORPHONE")]
        public string ENGINEERDIRECTORPHONE { get; set; }
        /// <summary>
        /// 工程技术人数
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERTECHPERSON")]
        public int? ENGINEERTECHPERSON { get; set; }

        [Column("NEXTCHECKDEPTCODE")]
        public string NEXTCHECKDEPTCODE { get; set; }
        [Column("ISAUDITOVER")]
        public string ISAUDITOVER { get; set; }
        [Column("NEXTCHECKROLENAME")]
        public string NEXTCHECKROLENAME { get; set; }
        [Column("NEXTCHECKDEPTID")]
        public string NEXTCHECKDEPTID { get; set; }
         [Column("ENGINEERCASHDEPOSIT")]
        public string ENGINEERCASHDEPOSIT { get; set; }
         [Column("ENGINEERCONTENT")]
        public string ENGINEERCONTENT { get; set; }
         [Column("ENGINEERSCALE")]
        public string ENGINEERSCALE { get; set; }
        /// <summary>
        /// 是否有安全许可证 0 有 1无
        /// </summary>
        [Column("ISXK")]
         public string ISXK { get; set; }
        /// <summary>
        /// 是否有资质证 0 有 1无
        /// </summary>
        [Column("ISZZZJ")]
        public string ISZZZJ { get; set; }
        /// <summary>
        /// 审核流程Id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }
        /// <summary>
        /// 外包单位现场负责人
        /// </summary>
        /// <returns></returns>
        [Column("UNITSUPER")]
        public string UnitSuper { get; set; }
        /// <summary>
        /// 外包单位现场负责人Id
        /// </summary>
        /// <returns></returns>
        [Column("UNITSUPERID")]
        public string UnitSuperId { get; set; }
        /// <summary>
        /// 外包单位现场负责人电话
        /// </summary>
        /// <returns></returns>
        [Column("UNITSUPERPHONE")]
        public string UnitSuperPhone { get; set; }
        /// <summary>
        /// 委托人
        /// </summary>
        [Column("MANDATOR")]
        public string Mandator { get; set; }
        /// <summary>
        /// 委托人Id
        /// </summary>
        [Column("MANDATORID")]
        public string MandatorId { get; set; }
        /// <summary>
        /// 授权人Id
        /// </summary>
        [Column("CERTIGIERID")]
        public string CertigierId { get; set; }
        /// <summary>
        /// 授权人Id
        /// </summary>
        [Column("CERTIGIER")]
        public string Certigier { get; set; }
        /// <summary>
        /// 有效期起
        /// </summary>
        [Column("IMPOWERSTARTTIME")]
        public DateTime? ImpowerStartTime { get; set; }
        /// <summary>
        /// 有效期止
        /// </summary>
        [Column("IMPOWERENDTIME")]
        public DateTime? ImpowerEndTime { get; set; }

        /// <summary>
        /// 项目经理
        /// </summary>
        [Column("PROJECTMANAGER")]
        public string ProjectManager { get; set; }

        /// <summary>
        /// 项目经理电话
        /// </summary>
        [Column("PROJECTMANAGERTEL")]
        public string ProjectManagerTel { get; set; }

        /// <summary>
        /// 安全负责人
        /// </summary>
        [Column("SAFETYMODERATOR")]
        public string SafetyModerator { get; set; }

        /// <summary>
        /// 安全负责人电话
        /// </summary>
        [Column("SAFETYMODERATORTEL")]
        public string SafetyModeratorTel { get; set; }
        /// <summary>
        /// 合同甲方签订单位/人
        /// </summary>
        [Column("COMPACTFIRSTPARTY")]
        public string COMPACTFIRSTPARTY { get; set; }
        /// <summary>
        /// 乙方签订单位/人
        /// </summary>
        [Column("COMPACTSECONDPARTY")]
        public string COMPACTSECONDPARTY { get; set; }
        /// <summary>
        /// 合同生效时间
        /// </summary>
        [Column("COMPACTTAKEEFFECTDATE")]
        public DateTime? COMPACTTAKEEFFECTDATE { get; set; }
        /// <summary>
        /// 合同终止时间
        /// </summary>
        [Column("COMPACTEFFECTIVEDATE")]
        public DateTime? COMPACTEFFECTIVEDATE { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        [Column("COMPACTNO")]
        public string COMPACTNO { get; set; }
        /// <summary>
        /// 合同金额
        /// </summary>
        [Column("COMPACTMONEY")]
        public decimal? COMPACTMONEY { get; set; }
        /// <summary>
        /// 合同备注
        /// </summary>
        [Column("COMPACTREMARK")]
        public string COMPACTREMARK { get; set; }
        /// <summary>
        /// 协议甲方签订单位/人员
        /// </summary>
        [Column("PROTOCOLFIRSTPARTY")]
        public string PROTOCOLFIRSTPARTY { get; set; }
        /// <summary>
        /// 协议乙方签订单位/人员
        /// </summary>
        [Column("PROTOCOLSECONDPARTY")]
        public string PROTOCOLSECONDPARTY { get; set; }
        /// <summary>
        /// 签订地点
        /// </summary>
        [Column("SIGNPLACE")]
        public string SIGNPLACE { get; set; }
        /// <summary>
        /// 签订时间
        /// </summary>
        [Column("SIGNDATE")]
        public DateTime? SIGNDATE { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID)?Guid.NewGuid().ToString():ID;
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
           this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
           this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}