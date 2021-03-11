using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EmergencyPlatform
{
    /// <summary>
    /// �� ��������������
    /// </summary>
    [Table("MAE_DRILLASSESS")]
    public class DrillassessEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// ������ԱID
        /// </summary>
        /// <returns></returns>
        [Column("VALUATEPERSON")]
        public string ValuatePerson { get; set; }
        /// <summary>
        /// �����ص�
        /// </summary>
        /// <returns></returns>
        [Column("DRILLPLACE")]
        public string DrillPlace { get; set; }
        /// <summary>
        /// ��ʩ
        /// </summary>
        /// <returns></returns>
        [Column("MEASURE")]
        public string Measure { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [Column("FULLABLE")]
        public string Fullable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ʵսЧ������
        /// </summary>
        /// <returns></returns>
        [Column("EFFECTEVALUATE")]
        public string EffecteValuate { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("PROBLEM")]
        public string Problem { get; set; }
        /// <summary>
        /// ��ָ��
        /// </summary>
        /// <returns></returns>
        [Column("TOPPERSON")]
        public string TopPerson { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("DRILLNAME")]
        public string DrillName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// ������֯
        /// </summary>
        /// <returns></returns>
        [Column("WHOLEORGANIZE")]
        public string WholeOrganize { get; set; }
        /// <summary>
        /// ���ʵ�λ��Աְ��
        /// </summary>
        /// <returns></returns>
        [Column("SITESUPPLIESDUTY")]
        public string SiteSuppliesDuty { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("DRILLTYPE")]
        public string DrillType { get; set; }
        /// <summary>
        /// ������Ա����
        /// </summary>
        /// <returns></returns>
        [Column("VALUATEPERSONNAME")]
        public string ValuatePersonName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("SUITABLE")]
        public string Suitable { get; set; }
        /// <summary>
        /// ����Ӧ������ID
        /// </summary>
        /// <returns></returns>
        [Column("DRILLID")]
        public string DrillId { get; set; }
        /// <summary>
        /// ��֯�ֹ�
        /// </summary>
        /// <returns></returns>
        [Column("DIVIDEWORK")]
        public string DivideWork { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("DRILLTIME")]
        public DateTime? DrillTime { get; set; }
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
        /// ���䳷�����
        /// </summary>
        /// <returns></returns>
        [Column("EVACUATE")]
        public string Evacuate { get; set; }
        /// <summary>
        /// ��Ա��λ
        /// </summary>
        /// <returns></returns>
        [Column("PERSONSTANDBY")]
        public string PersonStandBy { get; set; }
        /// <summary>
        /// �����ϼ�
        /// </summary>
        /// <returns></returns>
        [Column("REPORTSUPERIOR")]
        public string ReportSuperior { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("DRILLCONTENT")]
        public string DrillContent { get; set; }
        /// <summary>
        /// ��Ԯ����Ԯ���
        /// </summary>
        /// <returns></returns>
        [Column("RESCUE")]
        public string Rescue { get; set; }
        /// <summary>
        /// �ֳ�����
        /// </summary>
        /// <returns></returns>
        [Column("SITESUPPLIES")]
        public string SiteSupplies { get; set; }
        /// <summary>
        /// ��֯����
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZEDEPT")]
        public string OrganizeDept { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// ��Ա��λְ��
        /// </summary>
        /// <returns></returns>
        [Column("PERSONSTANDBYDUTY")]
        public string PersonStandByDuty { get; set; }
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