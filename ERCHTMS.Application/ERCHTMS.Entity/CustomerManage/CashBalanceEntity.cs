using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CustomerManage
{
    /// <summary>
    /// �� �����ֽ����
    /// </summary>
    [Table("CLIENT_CASHBALANCE")]
    public class CashBalanceEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// �ֽ��������
        /// </summary>
        /// <returns></returns>
        [Column("CASHBALANCEID")]
        public string CashBalanceId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("OBJECTID")]
        public string ObjectId { get; set; }
        /// <summary>
        /// ExecutionDate
        /// </summary>
        /// <returns></returns>
        [Column("EXECUTIONDATE")]
        public DateTime? ExecutionDate { get; set; }
        /// <summary>
        /// CashAccount
        /// </summary>
        /// <returns></returns>
        [Column("CASHACCOUNT")]
        public string CashAccount { get; set; }
        /// <summary>
        /// Receivable
        /// </summary>
        /// <returns></returns>
        [Column("RECEIVABLE")]
        public decimal? Receivable { get; set; }
        /// <summary>
        /// Expenses
        /// </summary>
        /// <returns></returns>
        [Column("EXPENSES")]
        public decimal? Expenses { get; set; }
        /// <summary>
        /// Balance
        /// </summary>
        /// <returns></returns>
        [Column("BALANCE")]
        public decimal? Balance { get; set; }
        /// <summary>
        /// Abstract
        /// </summary>
        /// <returns></returns>
        [Column("ABSTRACT")]
        public string Abstract { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("SORTCODE")]
        public int? SortCode { get; set; }
        /// <summary>
        /// ɾ�����
        /// </summary>
        /// <returns></returns>
        [Column("DELETEMARK")]
        public int? DeleteMark { get; set; }
        /// <summary>
        /// ��Ч��־
        /// </summary>
        /// <returns></returns>
        [Column("ENABLEDMARK")]
        public int? EnabledMark { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("DESCRIPTION")]
        public string Description { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �����û�����
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �޸�����
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// �޸��û�����
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.CashBalanceId = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.DeleteMark = 0;
            this.EnabledMark = 1;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.CashBalanceId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}