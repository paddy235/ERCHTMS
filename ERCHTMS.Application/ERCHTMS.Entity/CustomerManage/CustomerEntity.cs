using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CustomerManage
{
    /// <summary>
    /// �� �����ͻ���Ϣ
    /// </summary>
        [Table("CLIENT_CUSTOMER")]
    public class CustomerEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// �ͻ�����
        /// </summary>
        /// <returns></returns>
          [Column("CUSTOMERID")]
            public string CustomerId { get; set; }
        /// <summary>
        /// �ͻ����
        /// </summary>
        /// <returns></returns>
          [Column("ENCODE")]
            public string EnCode { get; set; }
        /// <summary>
        /// �ͻ�����
        /// </summary>
        /// <returns></returns>
          [Column("FULLNAME")]
            public string FullName { get; set; }
        /// <summary>
        /// �ͻ����
        /// </summary>
        /// <returns></returns>
         [Column("SHORTNAME")]
            public string ShortName { get; set; }
        /// <summary>
        /// �ͻ���ҵ
        /// </summary>
        /// <returns></returns>
        [Column("CUSTINDUSTRYID")]
            public string CustIndustryId { get; set; }
        /// <summary>
        /// �ͻ�����
        /// </summary>
        /// <returns></returns>
         [Column("CUSTTYPEID")]
            public string CustTypeId { get; set; }
        /// <summary>
        /// �ͻ�����
        /// </summary>
        /// <returns></returns>
          [Column("CUSTLEVELID")]
            public string CustLevelId { get; set; }
        /// <summary>
        /// �ͻ��̶�
        /// </summary>
        /// <returns></returns>
         [Column("CUSTDEGREEID")]
            public string CustDegreeId { get; set; }
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
         [Column("CONTACT")]
            public string Contact { get; set; }
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
        /// ���˴���
        /// </summary>
        /// <returns></returns>
         [Column("LEGALPERSON")]
            public string LegalPerson { get; set; }
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
         [Column("createusername")]
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
            this.CustomerId = Guid.NewGuid().ToString();
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
            this.CustomerId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}