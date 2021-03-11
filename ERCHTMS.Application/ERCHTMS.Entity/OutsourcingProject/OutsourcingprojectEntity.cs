using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// �� ���������λ������Ϣ��
    /// </summary>
    [Table("EPG_OUTSOURCINGPROJECT")]
    public class OutsourcingprojectEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// ���˴�����
        /// </summary>
        /// <returns></returns>
        [Column("LEGALREPFAX")]
        public string LEGALREPFAX { get; set; }
        /// <summary>
        /// �������ʱ��(���һ�����̵�ʵ���깤ʱ��)
        /// </summary>
        /// <returns></returns>
        [Column("SERVICESENDTIME")]
        public DateTime? SERVICESENDTIME { get; set; }
        /// <summary>
        /// ��ϵ�˴���
        /// </summary>
        /// <returns></returns>
        [Column("LINKMANFAX")]
        public string LINKMANFAX { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// �����λ����
        /// </summary>
        /// <returns></returns>
        [Column("OUTSOURCINGNAME")]
        public string OUTSOURCINGNAME { get; set; }
        /// <summary>
        /// ͳһ������ô���
        /// </summary>
        /// <returns></returns>
        [Column("CREDITCODE")]
        public string CREDITCODE { get; set; }
        /// <summary>
        /// ���볡״̬��0���볡 1���볡
        /// </summary>
        /// <returns></returns>
        [Column("OUTORIN")]
        public string OUTORIN { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("EMAIL")]
        public string EMAIL { get; set; }
        /// <summary>
        /// ��λId
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTID")]
        public string OUTPROJECTID { get; set; }
        /// <summary>
        /// �����λ�ſ�
        /// </summary>
        /// <returns></returns>
        [Column("GENERALSITUATION")]
        public string GENERALSITUATION { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// ���˴���绰
        /// </summary>
        /// <returns></returns>
        [Column("LEGALREPPHONE")]
        public string LEGALREPPHONE { get; set; }
        /// <summary>
        /// ����ʼʱ��(��һ�����̵�ʵ�ʿ���ʱ��)
        /// </summary>
        /// <returns></returns>
        [Column("SERVICESSTARTTIME")]
        public DateTime? SERVICESSTARTTIME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// ���˴���
        /// </summary>
        /// <returns></returns>
        [Column("LEGALREP")]
        public string LEGALREP { get; set; }
        /// <summary>
        /// ������״̬��0���� 1 �� 
        /// </summary>
        /// <returns></returns>
        [Column("BLACKLISTSTATE")]
        public string BLACKLISTSTATE { get; set; }
        /// <summary>
        /// ��ϵ��
        /// </summary>
        /// <returns></returns>
        [Column("LINKMAN")]
        public string LINKMAN { get; set; }
        /// <summary>
        /// ��ϵ�˵绰
        /// </summary>
        /// <returns></returns>
        [Column("LINKMANPHONE")]
        public string LINKMANPHONE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// ��ҵ��ַ
        /// </summary>
        /// <returns></returns>
        [Column("ADDRESS")]
        public string ADDRESS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }

        /// <summary>
        /// �볡ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("LEAVETIME")]
        public DateTime? LEAVETIME { get; set; }
        /// <summary>
        /// �볡ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("OUTINTIME")]
        public DateTime? OUTINTIME { get; set; }

        /// <summary>
        /// ��ͬ����
        /// </summary>
        [Column("CONTRACTPERSONNUM")]
        public int? ContractPersonNum { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
           this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
           this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}