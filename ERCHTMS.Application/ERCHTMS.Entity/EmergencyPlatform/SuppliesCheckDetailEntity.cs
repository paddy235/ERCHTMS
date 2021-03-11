using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ�����ʼ������
    /// </summary>
    [Table("MAE_SUPPLIESCHECKDETAIL")]
    public class SuppliesCheckDetailEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// �������ʼ��id
        /// </summary>
        /// <returns></returns>
        [Column("RECID")]
        public string RecId { get; set; }
        /// <summary>
        /// ά��/�������
        /// </summary>
        /// <returns></returns>
        [Column("REPLACEORCHANGE")]
        public string ReplaceOrChange { get; set; }
        /// <summary>
        /// ����� 0���ϸ�  1�����ϸ�
        /// </summary>
        /// <returns></returns>
        [Column("CHECKRESULT")]
        public int? CheckResult { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERNAME")]
        public string DutyUserName { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("INTIME")]
        public DateTime? InTime { get; set; }
        /// <summary>
        /// ��λ
        /// </summary>
        /// <returns></returns>
        [Column("SUPPLIESUNTILNAME")]
        public string SuppliesUntilName { get; set; }
        /// <summary>
        /// �ִ�����
        /// </summary>
        /// <returns></returns>
        [Column("NUM")]
        public int? Num { get; set; }
        /// <summary>
        /// Ӧ����������
        /// </summary>
        /// <returns></returns>
        [Column("SUPPLIESNAME")]
        public string SuppliesName { get; set; }
        /// <summary>
        /// Ӧ�����ʹ���id
        /// </summary>
        /// <returns></returns>
        [Column("SUPPLIESID")]
        public string SuppliesId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
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
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
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
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
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
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}