using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：开工申请表
    /// </summary>
    [Table("EPG_STARTAPPLYFOR")]
    public class StartapplyforEntity : BaseEntity
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
        /// 申请人
        /// </summary>
        /// <returns></returns>
        [Column("APPLYPEOPLEID")]
        public string APPLYPEOPLEID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("APPLYPEOPLE")]
        public string APPLYPEOPLE { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        /// <returns></returns>
        [Column("APPLYTIME")]
        public DateTime? APPLYTIME { get; set; }
        /// <summary>
        /// 申请类型
        /// </summary>
        /// <returns></returns>
        [Column("APPLYTYPE")]
        public string APPLYTYPE { get; set; }
        /// <summary>
        /// 申请开工时间
        /// </summary>
        /// <returns></returns>
        [Column("APPLYRETURNTIME")]
        public DateTime? APPLYRETURNTIME { get; set; }
        /// <summary>
        /// 计划竣工时间
        /// </summary>
        [Column("APPLYENDTIME")]
        public DateTime? APPLYENDTIME { get; set; }
        /// <summary>
        /// 申请原因
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCAUSE")]
        public string APPLYCAUSE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string REMARK { get; set; }
        /// <summary>
        /// 申请单号
        /// </summary>
        /// <returns></returns>
        [Column("APPLYNO")]
        public string APPLYNO { get; set; }
        /// <summary>
        /// 是否提交 0 ：未提交 1：提交
        /// </summary>
        /// <returns></returns>
        [Column("ISCOMMIT")]
        public string ISCOMMIT { get; set; }
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
        ///是否结束（0:未结束，1:已结束）
        /// </summary>
        /// <returns></returns>
        [Column("ISOVER")]
        public int IsOver { get; set; }

        /// <summary>
        ///项目审查结果(用英文逗号分隔，0:未完成，1:已完成)
        /// </summary>
        /// <returns></returns>
        [Column("CHECKRESULT")]
        public string CheckResult { get; set; }
        /// <summary>
        ///项目审查人(用英文逗号分隔)
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERS")]
        public string CheckUsers { get; set; }
        /// <summary>
        ///审核角色
        /// </summary>
        /// <returns></returns>
        [Column("AUDITROLE")]
        public string AuditRole { get; set; }

        /// <summary>
        ///是否审查结束（0:未结束，1:已结束）
        /// </summary>
        /// <returns></returns>
        [Column("ISINVESTOVER")]
        public int ISINVESTOVER { get; set; }

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
        [Column("SAFETYMAN")]
        public string SafetyMan { get; set; }
        [Column("DUTYMAN")]
        public string DutyMan { get; set; }
        [Column("HTNUM")]
        public string htnum { get; set; }

    
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