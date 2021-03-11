using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// �� ����ת����¼��
    /// </summary>
    [Table("BIS_TRANSFERRECORD")]
    public class TransferrecordEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// ת���������˺�
        /// </summary>
        /// <returns></returns>
        [Column("OUTTRANSFERUSERACCOUNT")]
        public string OutTransferUserAccount { get; set; }
        /// <summary>
        /// ģ��ID
        /// </summary>
        /// <returns></returns>
        [Column("MODULEID")]
        public string ModuleId { get; set; }
        /// <summary>
        /// ���̽ڵ�ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }
        /// <summary>
        /// ת���������˺�
        /// </summary>
        /// <returns></returns>
        [Column("INTRANSFERUSERACCOUNT")]
        public string InTransferUserAccount { get; set; }
        /// <summary>
        /// ת��������ID
        /// </summary>
        /// <returns></returns>
        [Column("OUTTRANSFERUSERID")]
        public string OutTransferUserId { get; set; }
        /// <summary>
        /// ת��������ID
        /// </summary>
        /// <returns></returns>
        [Column("INTRANSFERUSERID")]
        public string InTransferUserId { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// ����ҵ��ID
        /// </summary>
        /// <returns></returns>
        [Column("RECID")]
        public string RecId { get; set; }
        /// <summary>
        /// �Ƿ���Ч 0:��Ч 1��ʧЧ
        /// </summary>
        /// <returns></returns>
        [Column("DISABLE")]
        public int? Disable { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ת��������
        /// </summary>
        /// <returns></returns>
        [Column("INTRANSFERUSERNAME")]
        public string InTransferUserName { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// ת��������
        /// </summary>
        /// <returns></returns>
        [Column("OUTTRANSFERUSERNAME")]
        public string OutTransferUserName { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
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
        /// �༭����
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