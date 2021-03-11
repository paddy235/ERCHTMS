using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：历史
    /// </summary>
    [Table("EPG_HISTORYDAILYEXAMINE")]
    public class HistorydailyexamineEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 考核金额
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEMONEY")]
        public double? ExamineMoney { get; set; }
        /// <summary>
        /// 考核编号
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINECODE")]
        public string ExamineCode { get; set; }
        /// <summary>
        /// 考核人ID
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEPERSONID")]
        public string ExaminePersonId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 考核内容
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINECONTENT")]
        public string ExamineContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 考核类别
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINETYPE")]
        public string ExamineType { get; set; }
        /// <summary>
        /// 关联ID
        /// </summary>
        /// <returns></returns>
        [Column("CONTRACTID")]
        public string ContractId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 考核时间
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINETIME")]
        public DateTime? ExamineTime { get; set; }
        /// <summary>
        /// 考核人
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEPERSON")]
        public string ExaminePerson { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 被考核部门ID
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINETODEPTID")]
        public string ExamineToDeptId { get; set; }
        /// <summary>
        /// 考核依据
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEBASIS")]
        public string ExamineBasis { get; set; }
        /// <summary>
        /// 被考核部门
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINETODEPT")]
        public string ExamineToDept { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }

        /// <summary>
        /// 流程部门名称
        /// </summary>
        [Column("FLOWDEPTNAME")]
        public string FlowDeptName { get; set; }

        /// <summary>
        /// 流程部门编码/ID
        /// </summary>
        [Column("FLOWDEPT")]
        public string FlowDept { get; set; }

        /// <summary>
        /// 流程角色名称
        /// </summary>
        [Column("FLOWROLENAME")]
        public string FlowRoleName { get; set; }

        /// <summary>
        /// 流程角色编码/ID
        /// </summary>
        [Column("FLOWROLE")]
        public string FlowRole { get; set; }

        /// <summary>
        /// 流程名称
        /// </summary>
        [Column("FLOWNAME")]
        public string FlowName { get; set; }

        /// <summary>
        /// 考核部门
        /// </summary>
        [Column("EXAMINEDEPT")]
        public string ExamineDept { get; set; }

        /// <summary>
        /// 考核部门ID
        /// </summary>
        [Column("EXAMINEDEPTID")]
        public string ExamineDeptId { get; set; }

        /// <summary>
        /// 创建人部门ID
        /// </summary>
        [Column("CREATEUSERDEPTID")]
        public string CreateUserDeptId { get; set; }
        /// <summary>
        /// 是否保存成功
        /// </summary>
        [Column("ISSAVED")]
        public int? IsSaved { get; set; }

        /// <summary>
        /// 流程是否结束
        /// </summary>
        [Column("ISOVER")]
        public int? IsOver { get; set; }
        /// <summary>
        /// 外包工程
        /// </summary>
        [Column("PROJECT")]
        public string Project { get; set; }
        [Column("PROJECTID")]
        public string ProjectId { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
           this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
           this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
            this.CreateUserDeptId = OperatorProvider.Provider.Current().DeptId;


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