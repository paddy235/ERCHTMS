using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：高危险作业审核/审批表
    /// </summary>
    [Table("BIS_HIGHRISKCHECK")]
    public class HighRiskCheckEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 审批（核）意见
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEREASON")]
        public string ApproveReason { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 审批（核）状态  1 同意  2 不同意
        /// </summary>
        /// <returns></returns>
        [Column("APPROVESTATE")]
        public string ApproveState { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 审批（核）人
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEPERSON")]
        public string ApprovePerson { get; set; }
        /// <summary>
        /// 审核单位
        /// </summary>
        /// <returns></returns>
        [Column("ADEPTNAME")]
        public string ADeptName { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 审核单位
        /// </summary>
        /// <returns></returns>
        [Column("ADEPTID")]
        public string ADeptId { get; set; }
        /// <summary>
        /// 审批（核）人
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEPERSONNAME")]
        public string ApprovePersonName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 申请单id
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEID")]
        public string ApproveId{ get; set; }
        /// <summary>
        /// 审核单位
        /// </summary>
        /// <returns></returns>
        [Column("ADEPTCODE")]
        public string ADeptCode { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 审批步骤  1  审核  2审批
        /// </summary>
        /// <returns></returns>
        [Column("APPROVESTEP")]
        public string ApproveStep { get; set; }
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