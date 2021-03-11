using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.FireManage
{
    /// <summary>
    /// �� ��������Ѳ��ȷ�ϼ�¼
    /// </summary>
    [Table("HRS_AFFIRMRECORD")]
    public class AffirmRecordEntity : BaseEntity
    {
        #region ʵ���Ա
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
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// Ѳ��ID
        /// </summary>
        /// <returns></returns>
        [Column("PATROLID")]
        public string PatrolId { get; set; }
        /// <summary>
        /// ȷ��ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("AFFIRMDATE")]
        public DateTime? AffirmDate { get; set; }
        /// <summary>
        /// ȷ����ID
        /// </summary>
        /// <returns></returns>
        [Column("AFFIRMUSERID")]
        public string AffirmUserId { get; set; }
        /// <summary>
        /// ȷ����
        /// </summary>
        /// <returns></returns>
        [Column("AFFIRMUSER")]
        public string AffirmUser { get; set; }
        /// <summary>
        /// ȷ��������
        /// </summary>
        /// <returns></returns>
        [Column("AFFIRMUSERTYPE")]
        public string AffirmUserType { get; set; }
        /// <summary>
        /// ǩ��
        /// </summary>
        /// <returns></returns>
        [Column("SIGNATURE")]
        public string Signature { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AutoId { get; set; }
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
            this.AffirmDate = DateTime.Now;
            this.AffirmUserId = OperatorProvider.Provider.Current().UserId;
            this.AffirmUser = OperatorProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// �༭����
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