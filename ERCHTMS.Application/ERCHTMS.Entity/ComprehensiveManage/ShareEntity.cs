using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.ComprehensiveManage
{
    /// <summary>
    /// 描 述：亮点分享
    /// </summary>
    [Table("HRS_SHARE")]
    public class ShareEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 分享主题
        /// </summary>
        /// <returns></returns>
        [Column("THEME")]
        public string Theme { get; set; }
        /// <summary>
        /// 分享理由
        /// </summary>
        /// <returns></returns>
        [Column("REASON")]
        public string Reason { get; set; }
        /// <summary>
        /// 分享人姓名
        /// </summary>
        /// <returns></returns>
        [Column("SHARENAME")]
        public string ShareName { get; set; }
        /// <summary>
        /// 分享部门名称
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// 分享部门编码
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// 分享时间
        /// </summary>
        /// <returns></returns>
        [Column("ISSUETIME")]
        public DateTime? IssueTime { get; set; }
        /// <summary>
        /// 是否发送 0 否 1 是
        /// </summary>
        /// <returns></returns>
        [Column("ISSEND")]
        public string IsSend { get; set; }
        /// <summary>
        /// 分享指数
        /// </summary>
        /// <returns></returns>
        [Column("SHAREINDEX")]
        public int? ShareIndex { get; set; }
        /// <summary>
        /// 分享ID
        /// </summary>
        /// <returns></returns>
        [Column("SHAREID")]
        public string ShareId { get; set; }
        /// <summary>
        /// 分享网址
        /// </summary>
        /// <returns></returns>
        [Column("SHAREURL")]
        public string ShareUrl { get; set; }
        /// <summary>
        /// 阅读次数
        /// </summary>
        /// <returns></returns>
        [Column("READNUM")]
        public int ReadNum { get; set; }
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
            this.ReadNum = 0;
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