using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CustomerManage
{
    /// <summary>
    /// �� �����̻���Ϣ
    /// </summary>
    [Table("CLIENT_CHANCE")]
    public class ChanceEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// �̻�����
        /// </summary>
        /// <returns></returns>
        [Column("CHANCEID")]
        public string ChanceId { get; set; }
        /// <summary>
        /// �̻����
        /// </summary>
        /// <returns></returns>
        [Column("ENCODE")]
        public string EnCode { get; set; }
        /// <summary>
        /// �̻�����
        /// </summary>
        /// <returns></returns>
        [Column("FULLNAME")]
        public string FullName { get; set; }
        /// <summary>
        /// �̻����
        /// </summary>
        /// <returns></returns>
        [Column("CHANCETYPEID")]
        public string ChanceTypeId { get; set; }
        /// <summary>
        /// �̻���Դ
        /// </summary>
        /// <returns></returns>
        [Column("SOURCEID")]
        public string SourceId { get; set; }
        /// <summary>
        /// �̻��׶�
        /// </summary>
        /// <returns></returns>
        [Column("STAGEID")]
        public string StageId { get; set; }
        /// <summary>
        /// �ɹ���
        /// </summary>
        /// <returns></returns>
        [Column("SUCCESSRATE")]
        public decimal? SuccessRate { get; set; }
        /// <summary>
        /// Ԥ�ƽ��
        /// </summary>
        /// <returns></returns>
        [Column("AMOUNT")]
        public decimal? Amount { get; set; }
        /// <summary>
        /// Ԥ������
        /// </summary>
        /// <returns></returns>
        [Column("PROFIT")]
        public decimal? Profit { get; set; }
        /// <summary>
        /// ���۷���
        /// </summary>
        /// <returns></returns>
           [Column("SALECOST")]
        public decimal? SaleCost { get; set; }
        /// <summary>
        /// Ԥ�Ƴɽ�ʱ��
        /// </summary>
        /// <returns></returns>
           [Column("DEALDATE")]
        public DateTime? DealDate { get; set; }
        /// <summary>
        /// ת���ͻ�
        /// </summary>
        /// <returns></returns>
           [Column("ISTOCUSTOM")]
        public int? IsToCustom { get; set; }
        /// <summary>
        /// ��˾����
        /// </summary>
        /// <returns></returns>
           [Column("COMPANYNAME")]
        public string CompanyName { get; set; }
        /// <summary>
        /// ��˾����
        /// </summary>
        /// <returns></returns>
          [Column("COMPANYNATUREID")]
        public string CompanyNatureId { get; set; }
        /// <summary>
        /// ��˾��ַ
        /// </summary>
        /// <returns></returns>
         [Column("COMPANYADDRESS")]
        public string CompanyAddress { get; set; }
        /// <summary>
        /// ��˾��վ
        /// </summary>
        /// <returns></returns>
          [Column("COMPANYSITE")]
        public string CompanySite { get; set; }
        /// <summary>
        /// ��˾���
        /// </summary>
        /// <returns></returns>
         [Column("COMPANYDESC")]
        public string CompanyDesc { get; set; }
        /// <summary>
        /// ����ʡ��
        /// </summary>
        /// <returns></returns>
           [Column("PROVINCE")]
        public string Province { get; set; }
        /// <summary>
        /// ���ڳ���
        /// </summary>
        /// <returns></returns>
        [Column("CITY")]
        public string City { get; set; }
        /// <summary>
        /// ��ϵ��
        /// </summary>
        /// <returns></returns>
           [Column("CONTACTS")]
        public string Contacts { get; set; }
        /// <summary>
        /// �ֻ�
        /// </summary>
        /// <returns></returns>
          [Column("MOBILE")]
        public string Mobile { get; set; }
        /// <summary>
        /// �绰
        /// </summary>
        /// <returns></returns>
           [Column("TEL")]
        public string Tel { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
          [Column("FAX")]
        public string Fax { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        /// <returns></returns>
          [Column("QQ")]
        public string QQ { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        /// <returns></returns>
        [Column("EMAIL")]
        public string Email { get; set; }
        /// <summary>
        /// ΢��
        /// </summary>
        /// <returns></returns>
          [Column("WECHAT")]
        public string Wechat { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
          [Column("HOBBY")]
        public string Hobby { get; set; }
        /// <summary>
        /// ������ԱId
        /// </summary>
        /// <returns></returns>
          [Column("TRACEUSERID")]
        public string TraceUserId { get; set; }
        /// <summary>
        /// ������Ա
        /// </summary>
        /// <returns></returns>
         [Column("TRACEUSERNAME")]
        public string TraceUserName { get; set; }
        /// <summary>
        /// �̻�״̬���루0-���ϣ�
        /// </summary>
        /// <returns></returns>
         [Column("CHANCESTATE")]
        public int? ChanceState { get; set; }
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
            this.ChanceId = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.ModifyDate = DateTime.Now;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ChanceId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}