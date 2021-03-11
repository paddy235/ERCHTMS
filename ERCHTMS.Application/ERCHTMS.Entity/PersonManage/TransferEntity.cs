using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.PersonManage
{
    /// <summary>
    /// 描 述：转岗信息表
    /// </summary>
    [Table("BIS_TRANSFER")]
    public class TransferEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        [Column("TID")]
        public string TID { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 转入部门ID
        /// </summary>
        /// <returns></returns>
        [Column("INDEPTID")]
        public string InDeptId { get; set; }
        /// <summary>
        /// 转入部门名称
        /// </summary>
        /// <returns></returns>
        [Column("INDEPTNAME")]
        public string InDeptName { get; set; }
        /// <summary>
        /// 转入部门Code
        /// </summary>
        /// <returns></returns>
        [Column("INDEPTCODE")]
        public string InDeptCode { get; set; }
        /// <summary>
        /// 转到部门名称
        /// </summary>
        /// <returns></returns>
        [Column("OUTDEPTNAME")]
        public string OutDeptName { get; set; }
        /// <summary>
        /// 转到部门ID
        /// </summary>
        /// <returns></returns>
        [Column("OUTDEPTID")]
        public string OutDeptId { get; set; }
        /// <summary>
        /// 转到部门Code
        /// </summary>
        /// <returns></returns>
        [Column("OUTDEPTCODE")]
        public string OutDeptCode { get; set; }
        /// <summary>
        /// 转岗人员ID
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 转岗人员姓名
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
        /// <summary>
        /// 转岗日期
        /// </summary>
        /// <returns></returns>
        [Column("TRANSFERTIME")]
        public DateTime? TransferTime { get; set; }
        /// <summary>
        /// 是否需要确认 0不需要确认 1需要确认 2已确认
        /// </summary>
        /// <returns></returns>
        [Column("ISCONFIRM")]
        public int? IsConfirm { get; set; }
        /// <summary>
        /// 转岗前职务ID
        /// </summary>
        /// <returns></returns>
        [Column("INJOBID")]
        public string InJobId { get; set; }
        /// <summary>
        /// 转岗前职务名称
        /// </summary>
        /// <returns></returns>
        [Column("INJOBNAME")]
        public string InJobName { get; set; }
        /// <summary>
        /// 转到职务id
        /// </summary>
        /// <returns></returns>
        [Column("OUTJOBID")]
        public string OutJobId { get; set; }
        /// <summary>
        /// 转到职务名称
        /// </summary>
        /// <returns></returns>
        [Column("OUTJOBNAME")]
        public string OutJobName { get; set; }
        /// <summary>
        /// 转岗前岗位ID
        /// </summary>
        /// <returns></returns>
        [Column("INPOSTID")]
        public string InPostId { get; set; }
        /// <summary>
        /// 转岗前岗位名称
        /// </summary>
        /// <returns></returns>
        [Column("INPOSTNAME")]
        public string InPostName { get; set; }
        /// <summary>
        /// 转到岗位ID
        /// </summary>
        /// <returns></returns>
        [Column("OUTPOSTID")]
        public string OutPostId { get; set; }
        /// <summary>
        /// 转到岗位名称
        /// </summary>
        /// <returns></returns>
        [Column("OUTPOSTNAME")]
        public string OutPostName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.TID = string.IsNullOrEmpty(TID) ? Guid.NewGuid().ToString() : TID;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.TID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
        }
        #endregion
    }

    public class AppInTransfer
    {
        public string info { get; set; }
        public string code { get; set; }

      
    }

    public class BzAppTransfer
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 前部门id
        /// </summary>
        public string olddepartmentid { get; set; }
        /// <summary>
        /// 前部门名称
        /// </summary>
        public string olddepartment { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public string departmentid { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string department { get; set; }
        /// <summary>
        /// 移岗时间
        /// </summary>
        public string allocationtime { get; set; }
        /// <summary>
        /// 前岗位
        /// </summary>
        public string oldRoleDutyName { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        public string RoleDutyName { get; set; }
        /// <summary>
        /// 前岗位
        /// </summary>
        public string RoleDutyId { get; set; }
        /// <summary>
        /// 职务id
        /// </summary>
        public string quartersid { get; set; }
        /// <summary>
        /// 前职务
        /// </summary>
        public string oldquarters { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string quarters { get; set; }
        /// <summary>
        /// 离厂时间
        /// </summary>
        public string leavetime { get; set; }
        /// <summary>
        /// 离厂说明
        /// </summary>
        public string leaveremark { get; set; }
        /// <summary>
        /// 是否处理
        /// </summary>
        public bool iscomplete { get; set; }
    }

    public class BzBase
    {
        public string userId { get; set; }

        public object data { get; set; }
    }

    /// <summary>
    /// 转岗完成同步实体
    /// </summary>
    public class BxwcTransfer
    {
        /// <summary>
        /// 岗位
        /// </summary>
        public string RoleDutyName { get; set; }
        /// <summary>
        /// 前岗位
        /// </summary>
        public string RoleDutyId { get; set; }
        /// <summary>
        /// 职务id
        /// </summary>
        public string quartersid { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string quarters { get; set; }
        /// <summary>
        /// 业务id
        /// </summary>
        public string id { get; set; }
    }
}