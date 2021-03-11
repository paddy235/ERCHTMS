using ERCHTMS.Code;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERCHTMS.Entity.EquipmentManage
{
    /// <summary>
    /// �� ����Σ�ջ�ѧƷ���
    /// </summary>
    [Table("XLD_DANGEROUSCHEMICAL")]
    public class DangerChemicalsEntity : BaseEntity
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
        /// ����
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        [Column("ALIAS")]
        public string Alias { get; set; }
        /// <summary>
        /// CAS��
        /// </summary>
        [Column("CAS")]
        public string Cas { get; set; }
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
        /// ��������
        /// </summary>
        [Column("PRODUCTIONDATE")]
        public DateTime? ProductionDate { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        [Column("DEPOSITDATE")]
        public DateTime? DepositDate { get; set; }
        /// <summary>
        /// ��ŵص�
        /// </summary>
        [Column("SITE")]
        public string Site { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        [Column("DEADLINE")]
        public int? Deadline { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        [Column("DUTYUSER")]
        public string DutyUser { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        /// ���β���
        /// </summary>
        [Column("DUTYDEPT")]
        public string DutyDept { get; set; }
        /// <summary>
        /// ���β��ű��
        /// </summary>
        [Column("DUTYDEPTCODE")]
        public string DutyDeptCode { get; set; }
        /// <summary>
        /// �Ƿ����ֳ�
        /// </summary>
        [Column("ISSCENE")]
        public string IsScene { get; set; }
        /// <summary>
        /// ��Ҫ��ȫ����
        /// </summary>
        [Column("MAINRISK")]
        public string MainRisk { get; set; }
        /// <summary>
        /// ��ȡ�ķ�����ʩ
        /// </summary>
        [Column("MEASURE")]
        public string Measure { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        [Column("GRANTPERSON")]
        public string GrantPerson { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        [Column("GRANTPERSONID")]
        public string GrantPersonId { get; set; }

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
        /// <summary>
        /// �Ƿ�ɾ��   0��  1 ��
        /// </summary>
        [Column("ISDELETE")]
        public int? IsDelete { get; set; }

        /// <summary>
        /// ���洢��
        /// </summary>
        [Column("MAXNUM")]
        public string MAXNUM { get; set; }

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