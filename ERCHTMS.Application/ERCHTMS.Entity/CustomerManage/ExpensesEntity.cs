using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CustomerManage
{
    /// <summary>
    /// �� ��������֧��
    /// </summary>
    [Table("CLIENT_EXPENSES")]
    public class ExpensesEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ֧������
        /// </summary>
        /// <returns></returns>
        [Column("EXPENSESID")]
        public string ExpensesId { get; set; }
        /// <summary>
        /// ֧������
        /// </summary>
        /// <returns></returns>
        [Column("EXPENSESDATE")]
        public DateTime? ExpensesDate { get; set; }
        /// <summary>
        /// ֧�����
        /// </summary>
        /// <returns></returns>
        [Column("EXPENSESPRICE")]
        public decimal? ExpensesPrice { get; set; }
        /// <summary>
        /// ֧���˻�
        /// </summary>
        /// <returns></returns>
        [Column("EXPENSESACCOUNT")]
        public string ExpensesAccount { get; set; }
        /// <summary>
        /// ֧������
        /// </summary>
        /// <returns></returns>
        [Column("EXPENSESTYPE")]
        public string ExpensesType { get; set; }
        /// <summary>
        /// ֧������1-��˾֧����2-���˵渶��
        /// </summary>
        /// <returns></returns>
        [Column("EXPENSESOBJECT")]
        public int? ExpensesObject { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("MANAGERS")]
        public string Managers { get; set; }
        /// <summary>
        /// ֧��ժҪ
        /// </summary>
        /// <returns></returns>
        [Column("EXPENSESABSTRACT")]
        public string ExpensesAbstract { get; set; }
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
            this.ExpensesId = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.ExpensesObject = 1;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ExpensesId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}