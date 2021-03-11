using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：高风险作业许可申请
    /// </summary>
    [Table("BIS_HIGHRISKAPPLY")]
    public class HighRiskApplyEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 创建用户id
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
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 申请单位
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTID")]
        public string ApplyDeptId { get; set; }
        /// <summary>
        /// 申请单位
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTCODE")]
        public string ApplyDeptCode { get; set; }
        /// <summary>
        /// 申请单位
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTNAME")]
        public string ApplyDeptName { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERID")]
        public string ApplyUserId { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERNAME")]
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 作业类型
        /// </summary>
        /// <returns></returns>
        [Column("WORKTYPE")]
        public string WorkType { get; set; }
        /// <summary>
        /// 作业开始时间
        /// </summary>
        /// <returns></returns>
        [Column("WORKSTARTTIME")]
        public DateTime? WorkStartTime { get; set; }
        /// <summary>
        /// 作业结束时间
        /// </summary>
        /// <returns></returns>
        [Column("WORKENDTIME")]
        public DateTime? WorkEndTime { get; set; }
        /// <summary>
        /// 作业地点
        /// </summary>
        /// <returns></returns>
        [Column("WORKPLACE")]
        public string WorkPlace { get; set; }
        /// <summary>
        /// 作业内容
        /// </summary>
        /// <returns></returns>
        [Column("WORKCONTENT")]
        public string WorkContent { get; set; }
        /// <summary>
        /// 作业许可证文件
        /// </summary>
        /// <returns></returns>
        [Column("WORKFILES")]
        public string WorkFiles { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        /// <returns></returns>
        [Column("VERIFYUSERIDS")]
        public string VerifyUserIds { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        /// <returns></returns>
        [Column("VERIFYUSERNAMES")]
        public string VerifyUserNames { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEUSERID")]
        public string ApproveUserId { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEUSERNAME")]
        public string ApproveUserName { get; set; }
        /// <summary>
        /// 申请状态(1.申请中,2.审核中,3.审核未通过,4.审批中,5.审批未通过,6.审批完成)
        /// </summary>
        /// <returns></returns>
        [Column("APPLYSTATE")]
        public string ApplyState { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
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