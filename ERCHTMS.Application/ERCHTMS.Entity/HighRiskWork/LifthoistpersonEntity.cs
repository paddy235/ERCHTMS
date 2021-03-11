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
    /// �� �������ص�װ��ҵ������Ա��
    /// </summary>
    [Table("BIS_LIFTHOISTPERSON")]
    public class LifthoistpersonEntity : BaseEntity
    {
        #region ʵ���Ա
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
        /// ��Ա����
        /// </summary>
        /// <returns></returns>
        [Column("PERSONTYPE")]
        public string PersonType { get; set; }
        /// <summary>
        /// ��Ա����
        /// </summary>
        /// <returns></returns>
        [Column("PERSONNAME")]
        public string PersonName { get; set; }
        /// <summary>
        /// ��ԱID
        /// </summary>
        /// <returns></returns>
        [Column("PERSONID")]
        public string PersonId { get; set; }
        /// <summary>
        /// ֤����
        /// </summary>
        /// <returns></returns>
        [Column("CERTIFICATENUM")]
        public string CertificateNum { get; set; }

        /// <summary>
        /// ���ص�װ��ҵ����ID
        /// </summary>
        [Column("RECID")]
        public string RecId { get; set; }

        [NotMapped]
        public List<Photo> lifthoistpersonfile { get; set; }

        /// <summary>
        /// ��������ID
        /// </summary>
        [Column("BELONGDEPTID")]
        public string BelongDeptId { get; set; }

        /// <summary>
        /// ������������
        /// </summary>
        [Column("BELONGDEPTNAME")]
        public string BelongDeptName { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
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
        /// �༭����
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