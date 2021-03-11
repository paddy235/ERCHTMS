using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：安全措施变动申请表
    /// </summary>
    [Table("BIS_SAFETYCHANGE")]
    public class SafetychangeEntity : BaseEntity
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
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
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
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
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
        /// 申请单位
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUNIT")]
        public string APPLYUNIT { get; set; }
        /// <summary>
        /// 申请单位ID
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUNITID")]
        public string APPLYUNITID { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        /// <returns></returns>
        [Column("APPLYPEOPLE")]
        public string APPLYPEOPLE { get; set; }
        /// <summary>
        /// 申请人ID
        /// </summary>
        /// <returns></returns>
        [Column("APPLYPEOPLEID")]
        public string APPLYPEOPLEID { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        /// <returns></returns>
        [Column("APPLYTIME")]
        public DateTime? APPLYTIME { get; set; }
        /// <summary>
        /// 作业单位类型 0 ：单位内部 1 外包单位
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNITTYPE")]
        public string WORKUNITTYPE { get; set; }
        /// <summary>
        /// 作业单位
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNIT")]
        public string WORKUNIT { get; set; }
        /// <summary>
        /// 作业单位ID
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNITID")]
        public string WORKUNITID { get; set; }
        /// <summary>
        /// 作业单位COde
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNITCODE")]
        public string WORKUNITCODE { get; set; }
        /// <summary>
        /// 工程ID
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTID")]
        public string PROJECTID { get; set; }
        /// <summary>
        /// 工程名称
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTNAME")]
        public string PROJECTNAME { get; set; }
        /// <summary>
        /// 作业区域
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREA")]
        public string WORKAREA { get; set; }
        /// <summary>
        /// 作业地点
        /// </summary>
        /// <returns></returns>
        [Column("WORKPLACE")]
        public string WORKPLACE { get; set; }
        /// <summary>
        /// 作业内容
        /// </summary>
        /// <returns></returns>
        [Column("WORKCONTENT")]
        public string WORKCONTENT { get; set; }
        /// <summary>
        /// 作业负责人
        /// </summary>
        /// <returns></returns>
        [Column("WORKFZR")]
        public string WORKFZR { get; set; }
        /// <summary>
        /// 需变动的安全设施名称
        /// </summary>
        /// <returns></returns>
        [Column("CHANGENAME")]
        public string CHANGENAME { get; set; }
        /// <summary>
        /// 变动形式
        /// </summary>
        /// <returns></returns>
        [Column("CHANGETYPE")]
        public string CHANGETYPE { get; set; }
        /// <summary>
        /// 申请变动时间
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCHANGETIME")]
        public DateTime? APPLYCHANGETIME { get; set; }
        /// <summary>
        /// 恢复时间
        /// </summary>
        /// <returns></returns>
        [Column("RETURNTIME")]
        public DateTime? RETURNTIME { get; set; }
        /// <summary>
        /// 变动理由
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEREASON")]
        public string CHANGEREASON { get; set; }
        /// <summary>
        /// 防护措施
        /// </summary>
        /// <returns></returns>
        [Column("PROCEDURES")]
        public string PROCEDURES { get; set; }
        /// <summary>
        /// 验收情况
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTANCE")]
        public string ACCEPTANCE { get; set; }
        /// <summary>
        /// 是否验收审核完成 0 未完成 1 已完成
        /// </summary>
        /// <returns></returns>
        [Column("ISACCEPOVER")]
        public int? ISACCEPOVER { get; set; }
        /// <summary>
        /// 是否提交 0 未提交 1 已提交
        /// </summary>
        /// <returns></returns>
        [Column("ISCOMMIT")]
        public int? ISCOMMIT { get; set; }
        /// <summary>
        /// 是否申请审核完成 0 未完成 1 已完成
        /// </summary>
        /// <returns></returns>
        [Column("ISAPPLYOVER")]
        public int? ISAPPLYOVER { get; set; }
        /// <summary>
        /// 作业负责人ID
        /// </summary>
        /// <returns></returns>
        [Column("WORKFZRID")]
        public string WORKFZRID { get; set; }
        /// <summary>
        /// 申请编号
        /// </summary>
        /// <returns></returns>
        [Column("APPLYNO")]
        public string APPLYNO { get; set; }
        /// <summary>
        /// 验收申请人
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPPEOPLE")]
        public string ACCEPPEOPLE { get; set; }
        /// <summary>
        /// 验收申请人ID
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPPEOPLEID")]
        public string ACCEPPEOPLEID { get; set; }
        /// <summary>
        /// 验收时间
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTIME")]
        public DateTime? ACCEPTIME { get; set; }
        /// <summary>
        /// 申请类型
        /// </summary>
        /// <returns></returns>
        [Column("APPLYTYPE")]
        public string APPLYTYPE { get; set; }
        /// <summary>
        /// 当前流程状态
        /// </summary>
        /// <returns></returns>
        [Column("NODENAME")]
        public string NodeName { get; set; }
        /// <summary>
        /// 当前流程节点Id
        /// </summary>
        /// <returns></returns>
        [Column("NODEID")]
        public string NodeId { get; set; }
        /// <summary>
        /// 流程角色编码/ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLE")]
        public string FLOWROLE { get; set; }

        /// <summary>
        /// 流程角色名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLENAME")]
        public string FLOWROLENAME { get; set; }

        /// <summary>
        /// 流程部门编码/ID 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPT")]
        public string FLOWDEPT { get; set; }

        /// <summary>
        /// 流程部门名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPTNAME")]
        public string FLOWDEPTNAME { get; set; }
        [Column("ISACCPCOMMIT")]
        public int ISACCPCOMMIT { get; set; }
        [Column("ACCEPDEPT")]
        public string ACCEPDEPT { get; set; }
        [Column("ACCEPDEPTID")]
        public string ACCEPDEPTID { get; set; }


        /// <summary>
        /// 实际变动开始时间
        /// </summary>
        /// <returns></returns>
        [Column("REALITYCHANGETIME")]
        public DateTime? REALITYCHANGETIME { get; set; }

        /// <summary>
        /// 作业区域Code
        /// </summary>
        [Column("WORKAREACODE")]
        public string WorkAreaCode { get; set; }

        #region 变动申请
        /// <summary>
        /// 专业分类
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYTYPE")]
        public string SPECIALTYTYPE { get; set; }

        /// <summary>
        /// 审核备注
        /// </summary>
        /// <returns></returns>
        [Column("FLOWREMARK")]
        public string FLOWREMARK { get; set; }

        /// <summary>
        /// 抄送人
        /// </summary>
        /// <returns></returns>
        [Column("COPYUSERIDS")]
        public string COPYUSERIDS { get; set; }

        /// <summary>
        /// 抄送人
        /// </summary>
        /// <returns></returns>
        [Column("COPYUSERNAMES")]
        public string COPYUSERNAMES { get; set; }

        /// <summary>
        /// 短信通知审批人(0:否 1:是)
        /// </summary>
        /// <returns></returns>
        [Column("ISMESSAGE")]
        public string ISMESSAGE { get; set; }

        /// <summary>
        /// 专业分类
        /// </summary>
        public string SPECIALTYTYPENAME { get; set; }
        
        #endregion

        #region 验收申请
        /// <summary>
        /// 专业分类
        /// </summary>
        /// <returns></returns>
        [Column("ACCSPECIALTYTYPE")]
        public string ACCSPECIALTYTYPE { get; set; }

        /// <summary>
        /// 抄送人
        /// </summary>
        /// <returns></returns>
        [Column("ACCCOPYUSERIDS")]
        public string ACCCOPYUSERIDS { get; set; }

        /// <summary>
        /// 抄送人
        /// </summary>
        /// <returns></returns>
        [Column("ACCCOPYUSERNAMES")]
        public string ACCCOPYUSERNAMES { get; set; }

        /// <summary>
        /// 短信通知审批人(0:否 1:是)
        /// </summary>
        /// <returns></returns>
        [Column("ACCISMESSAGE")]
        public string ACCISMESSAGE { get; set; }

        /// <summary>
        /// 专业分类
        /// </summary>
        public string ACCSPECIALTYTYPENAME { get; set; }
        #endregion

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
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