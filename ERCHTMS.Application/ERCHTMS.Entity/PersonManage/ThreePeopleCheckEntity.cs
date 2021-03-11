using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.PersonManage
{
    /// <summary>
    /// 描 述：三种人审批业务表
    /// </summary>
    [Table("BIS_THREEPEOPLECHECK")]
    public class ThreePeopleCheckEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 流程状态
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public string Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 申请人姓名
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 流程节点
        /// </summary>
        /// <returns></returns>
        [Column("NODEID")]
        public string NodeId { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPT")]
        public string BelongDept { get; set; }
        /// <summary>
        /// 所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTCODE")]
        public string BelongDeptCode { get; set; }
        /// <summary>
        /// 所属部门ID
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTID")]
        public string BelongDeptId { get; set; }
        /// <summary>
        /// 流程是否结束
        /// </summary>
        /// <returns></returns>
        [Column("ISOVER")]
        public int? IsOver { get; set; }
        /// <summary>
        /// 申请单位类型（内部，外部）
        /// </summary>
        /// <returns></returns>
        [Column("APPLYTYPE")]
        public string ApplyType { get; set; }

        /// <summary>
        /// 工程名称
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTNAME")]
        public string ProjectName { get; set; }
        /// <summary>
        /// 工程Id 
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTID")]
        public string ProjectId { get; set; }

        /// <summary>
        /// 申请人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 申请人所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 申请人所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 申请人所属部门Id
        /// <returns></returns>
        [Column("CREATEUSERDEPTID")]
        public string CreateUserDeptId { get; set; }
        /// <summary>
        /// 是否提交（0:保存,1:提交）
        /// </summary>
        /// <returns></returns>
        [Column("ISSUMBIT")]
        public int? IsSumbit { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 申请人账号
        /// </summary>
        /// <returns></returns>
        [Column("USERACCOUNT")]
        public string UserAccount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 申请编号
        /// </summary>
        /// <returns></returns>
        [Column("APPLYSNO")]
        public string ApplySno { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
           this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
            var user= OperatorProvider.Provider.Current();
            if (user!=null)
            {
                this.CreateUserId = user.UserId;
                this.CreateUserName = user.UserName;
                this.CreateUserDeptCode = user.DeptCode;
                this.CreateUserOrgCode = user.OrganizeCode;
                this.UserAccount = user.Account;
                CreateUserDeptId = user.DeptId;
            }
            CreateTime = DateTime.Now;
            this.IsOver = 0;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;                             
        }
        #endregion
    }
}