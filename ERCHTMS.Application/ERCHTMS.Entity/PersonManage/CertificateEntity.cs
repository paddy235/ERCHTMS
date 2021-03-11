using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.PersonManage
{
    /// <summary>
    /// 描 述：人员证书
    /// </summary>
    [Table("BIS_CERTIFICATE")]
    public class CertificateEntity : BaseEntity
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 证书名称
        /// </summary>
        /// <returns></returns>
        [Column("CERTNAME")]
        public string CertName { get; set; }
        /// <summary>
        /// 证书编号
        /// </summary>
        /// <returns></returns>
        [Column("CERTNUM")]
        public string CertNum { get; set; }
        /// <summary>
        /// 发证日期
        /// </summary>
        /// <returns></returns>
        [Column("SENDDATE")]
        public DateTime? SendDate { get; set; }
        /// <summary>
        /// 有效期（年）
        /// </summary>
        /// <returns></returns>
        [Column("YEARS")]
        public int? Years { get; set; }
        /// <summary>
        /// 状态(1：有效，0：无效)
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public int? Status { get; set; }
        /// <summary>
        /// 证书失效日期
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
        /// 附件路径
        /// </summary>
        /// <returns></returns>
        [Column("FILEPATH")]
        public string FilePath { get; set; }
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
        /// 证书类型
        /// </summary>
        /// <returns></returns>
        [Column("CERTTYPE")]
        public string CertType { get; set; }
        /// <summary>
        /// 种类/作业类别
        /// </summary>
        /// <returns></returns>
        [Column("WORKTYPE")]
        public string WorkType { get; set; }
        /// <summary>
        /// 作业项目/准操项目
        /// </summary>
        /// <returns></returns>
        [Column("WORKITEM")]
        public string WorkItem { get; set; }
        /// <summary>
        /// 项目代号
        /// </summary>
        /// <returns></returns>
        [Column("ITEMNUM")]
        public string ItemNum { get; set; }

        /// <summary>
        ///复审结果
        /// </summary>
        /// <returns></returns>
        public string Result { get; set; }
        /// <summary>
        /// 复审日期
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDATE")]
        public DateTime? ApplyDate { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        /// <returns></returns>
        [Column("STARTDATE")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        ///等级
        /// </summary>
        /// <returns></returns>
         [Column("GRADE")]
        public string Grade { get; set; }

        /// <summary>
        ///行业
        /// </summary>
        /// <returns></returns>
         [Column("INDUSTRY")]
        public string Industry { get; set; }

        /// <summary>
        ///人员类型
        /// </summary>
        /// <returns></returns>
         [Column("USERTYPE")]
        public string UserType { get; set; }
         /// <summary>
         ///工种
         /// </summary>
         /// <returns></returns>
         [Column("CRAFT")]
         public string Craft { get; set; }
         /// <summary>
         ///资格名称
         /// </summary>
         /// <returns></returns>
         [Column("ZGNAME")]
         public string ZGName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            var user = OperatorProvider.Provider.Current();
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            this.Status = 1;
            if (user!=null)
            {
                this.CreateUserId = user.UserId;
                this.CreateUserDeptCode = user.DeptCode;
                this.CreateUserOrgCode = user.OrganizeCode;
            }
          
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
            var user = OperatorProvider.Provider.Current();
            if(user!=null)
            {
                this.ModifyUserId = user.UserId;
            }
        }
        #endregion
    }
}