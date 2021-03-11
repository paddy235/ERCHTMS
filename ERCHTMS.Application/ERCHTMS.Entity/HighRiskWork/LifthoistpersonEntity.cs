using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：起重吊装作业操作人员表
    /// </summary>
    [Table("BIS_LIFTHOISTPERSON")]
    public class LifthoistpersonEntity : BaseEntity
    {
        #region 实体成员
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
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODITYUSERID")]
        public string ModityUserid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODITYUSERNAME")]
        public string ModityUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 人员类型
        /// </summary>
        /// <returns></returns>
        [Column("PERSONTYPE")]
        public string PersonType { get; set; }
        /// <summary>
        /// 人员姓名
        /// </summary>
        /// <returns></returns>
        [Column("PERSONNAME")]
        public string PersonName { get; set; }
        /// <summary>
        /// 人员ID
        /// </summary>
        /// <returns></returns>
        [Column("PERSONID")]
        public string PersonId { get; set; }
        /// <summary>
        /// 证件号
        /// </summary>
        /// <returns></returns>
        [Column("CERTIFICATENUM")]
        public string CertificateNum { get; set; }

        /// <summary>
        /// 起重吊装作业主键ID
        /// </summary>
        [Column("RECID")]
        public string RecId { get; set; }

        [NotMapped]
        public List<Photo> lifthoistpersonfile { get; set; }

        /// <summary>
        /// 所属部门ID
        /// </summary>
        [Column("BELONGDEPTID")]
        public string BelongDeptId { get; set; }

        /// <summary>
        /// 所属部门名称
        /// </summary>
        [Column("BELONGDEPTNAME")]
        public string BelongDeptName { get; set; }
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
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
                                }
        #endregion
    }
}