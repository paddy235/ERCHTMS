using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.PersonManage
{
    /// <summary>
    /// 描 述：人员证件复审记录
    /// </summary>
    [Table("BIS_CERTAUDIT")]
    public class CertAuditEntity : BaseEntity
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
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 复审项目代号
        /// </summary>
        /// <returns></returns>
        [Column("ITEMCODE")]
        public string ItemCode { get; set; }
        /// <summary>
        /// 复审/换证日期
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDATE")]
        public DateTime? AuditDate { get; set; }
        /// <summary>
        /// 下次复审日期
        /// </summary>
        /// <returns></returns>
        [Column("NEXTDATE")]
        public DateTime? NextDate { get; set; }
        /// <summary>
        /// 证书有效期限
        /// </summary>
        /// <returns></returns>
        [Column("ENDDATE")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 发证机关
        /// </summary>
        /// <returns></returns>
        [Column("SENDORGAN")]
        public string SendOrgan { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 关联用户Id
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 关联证书Id
        /// </summary>
        /// <returns></returns>
        [Column("CERTID")]
        public string CertId { get; set; }
        /// <summary>
        /// 复审类型
        /// </summary>
        /// <returns></returns>
        [Column("AUDITTYPE")]
        public string AuditType { get; set; }

        /// <summary>
        /// 复审结果
        /// </summary>
        /// <returns></returns>
        [Column("RESULT")]
        public string Result { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            var user=OperatorProvider.Provider.Current();
            this.Id= string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = user.UserId;
            this.CreateUserName = user.UserName;
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