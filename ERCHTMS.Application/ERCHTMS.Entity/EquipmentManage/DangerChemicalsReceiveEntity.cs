using ERCHTMS.Code;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERCHTMS.Entity.EquipmentManage
{
    /// <summary>
    /// �� ����Σ�ջ�ѧƷ����
    /// </summary>
    [Table("XLD_DANGEROUSCHEMICALRECEIVE")]
    public class DangerChemicalsReceiveEntity : BaseEntity
    {
        #region Ĭ���ֶ�
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
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
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        #endregion

        #region ʵ���Ա        
        /// <summary>
        /// Σ��ƷID
        /// </summary>
        [Column("MAINID")]
        public string MainId { get; set; }
        /// <summary>
        /// ��;
        /// </summary>
        [Column("PURPOSE")]
        public string Purpose { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        [Column("RECEIVENUM")]
        public string ReceiveNum { get; set; }
        /// <summary>
        /// ����������λ
        /// </summary>
        [Column("RECEIVEUNIT")]
        public string ReceiveUnit { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        [Column("RECEIVEUSERID")]
        public string ReceiveUserId { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        [Column("RECEIVEUSER")]
        public string ReceiveUser { get; set; }
        /// <summary>
        /// ǩ��
        /// </summary>
        [Column("SIGNIMG")]
        public string SignImg { get; set; }
        
        #endregion

        #region �����ֶ�        
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("FLOWNAME")]
        public string FLOWNAME { get; set; }

        /// <summary>
        /// ���̽�ɫ����/ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLE")]
        public string FLOWROLE { get; set; }

        /// <summary>
        /// ���̽�ɫ����
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLENAME")]
        public string FLOWROLENAME { get; set; }

        /// <summary>
        /// ���̲��ű���/ID 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPT")]
        public string FLOWDEPT { get; set; }

        /// <summary>
        /// ���̲�������
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPTNAME")]
        public string FLOWDEPTNAME { get; set; }

        /// <summary>
        /// �Ƿ񱣴�
        /// </summary>
        /// <returns></returns>
        [Column("ISSAVED")]
        public string ISSAVED { get; set; }

        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("ISOVER")]
        public string ISOVER { get; set; }
        /// <summary>
        /// �������Id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }


        /// <summary>
        /// ��������
        /// </summary>
        [Column("GRANTNUM")]
        public string GrantNum { get; set; }
        /// <summary>
        /// ����������λ
        /// </summary>
        [Column("GRANTUNIT")]
        public string GrantUnit { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        [Column("GRANTUSERID")]
        public string GrantUserId { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        [Column("GRANTUSER")]
        public string GrantUser { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        [Column("GRANTOPINION")]
        public string GrantOpinion { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("GRANTDATE")]
        public DateTime? GrantDate { get; set; }
        /// <summary>
        /// ������ǩ��
        /// </summary>
        [Column("GRANTSIGNIMG")]
        public string GrantSignImg { get; set; }
        /// <summary>
        /// ����״̬ 0δ��ʼ����  1������ 2�������
        /// </summary>
        [Column("GRANTSTATE")]
        public int? GrantState { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        [Column("SPECIFICATION")]
        public string Specification { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        [Column("INVENTORY")]
        public string Inventory { get; set; }
        /// <summary>
        /// ��浥λ
        /// </summary>
        [Column("UNIT")]
        public string Unit { get; set; }
        /// <summary>
        /// Σ��Ʒ����
        /// </summary>
        [Column("RISKTYPE")]
        public string RiskType { get; set; }
        /// <summary>
        /// ��ŵص�
        /// </summary>
        [Column("SITE")]
        public string Site { get; set; }
        /// <summary>
        /// ʵ�ʷ��ſ������
        /// </summary>
        [Column("PRACTICALNUM")]
        public string PracticalNum { get; set; }

        /// <summary>
        /// ���λ
        /// </summary>
        [Column("SPECIFICATIONUNIT")]
        public string SpecificationUnit { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        [Column("AMOUNT")]
        public string Amount { get; set; }
        /// <summary>
        /// ������λ
        /// </summary>
        [Column("AMOUNTUNIT")]
        public string AmountUnit { get; set; }

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