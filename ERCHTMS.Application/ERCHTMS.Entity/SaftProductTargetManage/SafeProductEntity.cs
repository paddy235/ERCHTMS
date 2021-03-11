using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERCHTMS.Entity.SaftProductTargetManage
{
    /// <summary>
    /// 描 述：安全生产目标
    /// </summary>
    [Table("BIS_SAFEPRODUCT")]
    public class SafeProductEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 安全目标名称
        /// </summary>
        /// <returns></returns>
        [Column("SAFTTARGETNAME")]
        public string SaftTargetName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        /// <returns></returns>
        [Column("DATEYEAR")]
        public string DateYear { get; set; }
        /// <summary>
        /// 责任书实际签订数量
        /// </summary>
        /// <returns></returns>
        [Column("REALCOUNT")]
        public string RealCount { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 责任书应签订数量
        /// </summary>
        /// <returns></returns>
        [Column("SHOULDCOUNT")]
        public string ShouldCount { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTID")]
        public string BelongDeptId { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTNAME")]
        public string BelongDeptName { get; set; }

        /// <summary>
        /// 签订率
        /// </summary>
        /// <returns></returns>
        [Column("AGREEMENTRATE")]
        public string AgreementRate { get; set; }

        /// <summary>
        /// 下发状态（0：未下发,1：已下发）
        /// </summary>
        /// <returns></returns>
        [Column("SENDSTATUS")]
        public string SendStatus { get; set; }
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
