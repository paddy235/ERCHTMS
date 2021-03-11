using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 
    /// </summary>
    [Table("EPG_TOOLS")]
    public class ToolsEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 创建记录用户ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 创建记录用户所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 创建记录用户角色
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 创建记录的时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 创建记录的用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 修改记录的时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 创建记录的用户ID
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 修改记录的用户名
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }

        /// <summary>
        /// 审核状态 0 通过 1 不通过 2 待审核 3 未提交
        /// </summary>
        [Column("AUDITSTATE")]
        public string AUDITSTATE { get; set; }

        /// <summary>
        /// 工器具表主键ID
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSID")]
        public string TOOLSID { get; set; }
        /// <summary>
        /// 外包单位ID
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTID")]
        public string OUTPROJECTID { get; set; }
        /// <summary>
        /// 外包工程ID
        /// </summary>
        /// <returns></returns>
        [Column("OUTENGINEERID")]
        public string OUTENGINEERID { get; set; }
        /// <summary>
        /// 工器具申请人
        /// </summary>
        /// <returns></returns>
        [Column("APPLYPEOPEL")]
        public string APPLYPEOPEL { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        /// <returns></returns>
        [Column("APPLYTIME")]
        public DateTime? APPLYTIME { get; set; }

        /// <summary>
        /// 流程名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWNAME")]
        public string FLOWNAME { get; set; }

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

        /// <summary>
        /// 是否保存
        /// </summary>
        /// <returns></returns>
        [Column("ISSAVED")]
        public string ISSAVED { get; set; }

        /// <summary>
        /// 流程完成情况
        /// </summary>
        /// <returns></returns>
        [Column("ISOVER")]
        public string ISOVER { get; set; }
        /// <summary>
        /// 设备类型（1：工器具，2：特种设备）
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPTYPE")]
        public string EQUIPTYPE { get; set; }
        /// <summary>
        /// 审核流程Id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }
        /// <summary>
        /// 专业类别
        /// </summary>
        [Column("SPECIALTYTYPE")]
        public string SpecialtyType { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.TOOLSID = string.IsNullOrEmpty(TOOLSID) ? Guid.NewGuid().ToString() : TOOLSID;
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
            this.TOOLSID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion

    }
}
