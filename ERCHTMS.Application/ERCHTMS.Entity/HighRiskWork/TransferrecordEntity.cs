using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：转交记录表
    /// </summary>
    [Table("BIS_TRANSFERRECORD")]
    public class TransferrecordEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 转交申请人账号
        /// </summary>
        /// <returns></returns>
        [Column("OUTTRANSFERUSERACCOUNT")]
        public string OutTransferUserAccount { get; set; }
        /// <summary>
        /// 模块ID
        /// </summary>
        /// <returns></returns>
        [Column("MODULEID")]
        public string ModuleId { get; set; }
        /// <summary>
        /// 流程节点ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }
        /// <summary>
        /// 转交接收人账号
        /// </summary>
        /// <returns></returns>
        [Column("INTRANSFERUSERACCOUNT")]
        public string InTransferUserAccount { get; set; }
        /// <summary>
        /// 转交申请人ID
        /// </summary>
        /// <returns></returns>
        [Column("OUTTRANSFERUSERID")]
        public string OutTransferUserId { get; set; }
        /// <summary>
        /// 转交接收人ID
        /// </summary>
        /// <returns></returns>
        [Column("INTRANSFERUSERID")]
        public string InTransferUserId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 关联业务ID
        /// </summary>
        /// <returns></returns>
        [Column("RECID")]
        public string RecId { get; set; }
        /// <summary>
        /// 是否有效 0:有效 1：失效
        /// </summary>
        /// <returns></returns>
        [Column("DISABLE")]
        public int? Disable { get; set; }
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
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 转交接收人
        /// </summary>
        /// <returns></returns>
        [Column("INTRANSFERUSERNAME")]
        public string InTransferUserName { get; set; }
        /// <summary>
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 转交申请人
        /// </summary>
        /// <returns></returns>
        [Column("OUTTRANSFERUSERNAME")]
        public string OutTransferUserName { get; set; }
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
        }
        #endregion
    }
}