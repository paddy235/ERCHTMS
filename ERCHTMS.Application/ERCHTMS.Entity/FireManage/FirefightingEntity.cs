using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.FireManage
{
    /// <summary>
    /// �� ����������ʩ
    /// </summary>
    [Table("HRS_FIREFIGHTING")]
    public class FirefightingEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
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
        /// ˮ����
        /// </summary>
        /// <returns></returns>
        [Column("WATERBELT")]
        public int? WaterBelt { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEDATE")]
        public DateTime? ExamineDate { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSER")]
        public string DutyUser { get; set; }
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
        /// ������ 0�ϸ� 1���ϸ�
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONVERDICT")]
        public int? DetectionVerdict { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        /// <returns></returns>
        [Column("EXTINGUISHERTYPE")]
        public string ExtinguisherType { get; set; }
        /// <summary>
        /// ��������ͱ��
        /// </summary>
        /// <returns></returns>
        [Column("EXTINGUISHERTYPENO")]
        public string ExtinguisherTypeNo { get; set; }
        /// <summary>
        /// ά����λ
        /// </summary>
        /// <returns></returns>
        [Column("SAFEGUARDUNIT")]
        public string SafeguardUnit { get; set; }
        /// <summary>
        /// �´μ��ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("NEXTDETECTIONDATE")]
        public DateTime? NextDetectionDate { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONDATE")]
        public DateTime? DetectionDate { get; set; }
        /// <summary>
        /// ���β���
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPT")]
        public string DutyDept { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEPERIOD")]
        public int? ExaminePeriod { get; set; }
        /// <summary>
        /// ��ⵥλ
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONUNIT")]
        public string DetectionUnit { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICT")]
        public string District { get; set; }
        /// <summary>
        /// ��װ��λ
        /// </summary>
        /// <returns></returns>
        [Column("INSTALLUNIT")]
        public string InstallUnit { get; set; }
        /// <summary>
        /// ���β��ű��
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPTCODE")]
        public string DutyDeptCode { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("PROTECTOBJECT")]
        public string ProtectObject { get; set; }
        /// <summary>
        /// ǹͷ��
        /// </summary>
        /// <returns></returns>
        [Column("SPEARHEAD")]
        public int? Spearhead { get; set; }
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
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// ��������ID
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTID")]
        public string DistrictId { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONPERIOD")]
        public int? DetectionPeriod { get; set; }
        /// <summary>
        /// ����λ��
        /// </summary>
        /// <returns></returns>
        [Column("LOCATION")]
        public string Location { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// ��������Code
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTCODE")]
        public string DistrictCode { get; set; }
        /// <summary>
        /// ��Ƶ�λ
        /// </summary>
        /// <returns></returns>
        [Column("DESIGNUNIT")]
        public string DesignUnit { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("DONEDATE")]
        public DateTime? DoneDate { get; set; }
        /// <summary>
        /// �����˵绰
        /// </summary>
        /// <returns></returns>
        [Column("DUTYTEL")]
        public string DutyTel { get; set; }
        /// <summary>
        /// �豸����
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNAME")]
        public string EquipmentName { get; set; }
        /// <summary>
        /// �豸���Ʊ��
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNAMENO")]
        public string EquipmentNameNo { get; set; }
        /// <summary>
        /// ��Ҫ����
        /// </summary>
        /// <returns></returns>
        [Column("MAINPARAMETER")]
        public string MainParameter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �´μ��ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("NEXTEXAMINEDATE")]
        public DateTime? NextExamineDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AutoId { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// �ϴγ�װʱ��
        /// </summary>
        /// <returns></returns>
        [Column("LASTFILLDATE")]
        public DateTime? LastFillDate { get; set; }
        /// <summary>
        /// �´γ�װʱ��
        /// </summary>
        /// <returns></returns>
        [Column("NEXTFILLDATE")]
        public DateTime? NextFillDate { get; set; }
        /// <summary>
        /// �豸���
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTCODE")]
        public string EquipmentCode { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        /// ����ͺ�
        /// </summary>
        /// <returns></returns>
        [Column("SPECIFICATIONS")]
        public string Specifications { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("LEAVEDATE")]
        public DateTime? LeaveDate { get; set; }
        /// <summary>
        /// nfc
        /// </summary>
        /// <returns></returns>
        [Column("NFC")]
        public string NFC { get; set; }
        /// <summary>
        /// ��װ/�������ڣ��죩
        /// </summary>
        /// <returns></returns>
        [Column("FILLPERIOD")]
        public int? FillPeriod { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEUSER")]
        public string ExamineUser { get; set; }
        /// <summary>
        /// �����ID
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEUSERID")]
        public string ExamineUserId { get; set; }
        /// <summary>
        /// ��鲿��
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEDEPT")]
        public string ExamineDept { get; set; }
        /// <summary>
        /// ��鲿��code
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEDEPTCODE")]
        public string ExamineDeptCode { get; set; }
        /// <summary>
        /// ���ڼ��ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("TERMINALDETECTIONDATE")]
        public DateTime? TerminalDetectionDate { get; set; }
        /// <summary>
        /// ���ڼ�ⵥλ
        /// </summary>
        /// <returns></returns>
        [Column("TERMINALDETECTIONUNIT")]
        public string TerminalDetectionUnit { get; set; }
        /// <summary>
        /// ���ڼ�����
        /// </summary>
        /// <returns></returns>
        [Column("TERMINALDETECTIONVERDICT")]
        public int? TerminalDetectionVerdict { get; set; }
        /// <summary>
        /// ���ڼ������(��)
        /// </summary>
        /// <returns></returns>
        [Column("TERMINALDETECTIONPERIOD")]
        public int? TerminalDetectionPeriod { get; set; }
        /// <summary>
        /// �´ζ��ڼ��ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("NEXTTERMINALDETECTIONDATE")]
        public DateTime? NextTerminalDetectionDate { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("AMOUNT")]
        public int? Amount { get; set; }
        /// <summary>
        /// ������λ
        /// </summary>
        /// <returns></returns>
        [Column("AMOUNTUNIT")]
        public string AmountUnit { get; set; }
        /// <summary>
        /// ������λ
        /// </summary>
        /// <returns></returns>
        [Column("SCRAPDATE")]
        public DateTime? ScrapDate { get; set; }
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