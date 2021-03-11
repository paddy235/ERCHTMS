using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// �� ����Σ�����س�����
    /// </summary>
    [Table("BIS_HAZARDOUSCAR")]
    public class HazardouscarEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// ��ʻ֤ͼƬ��ַ
        /// </summary>
        /// <returns></returns>
        [Column("DRIVERLICENSEURL")]
        public string DriverLicenseUrl { get; set; }
        /// <summary>
        /// Σ��ƷID
        /// </summary>
        /// <returns></returns>
        [Column("HAZARDOUSID")]
        public string HazardousId { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("PROCESSINGNAME")]
        public string ProcessingName { get; set; }
        /// <summary>
        /// �볡ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("INTIME")]
        public DateTime? InTime { get; set; }
        /// <summary>
        /// ��λ�豸ID
        /// </summary>
        /// <returns></returns>
        [Column("GPSID")]
        public string GPSID { get; set; }
        /// <summary>
        /// ˾���绰
        /// </summary>
        /// <returns></returns>
        [Column("PHONE")]
        public string Phone { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("NOTE")]
        public string Note { get; set; }
        /// <summary>
        /// ��ʻ��
        /// </summary>
        /// <returns></returns>
        [Column("DIRVER")]
        public string Dirver { get; set; }
        /// <summary>
        /// ��λ�豸����
        /// </summary>
        /// <returns></returns>
        [Column("GPSNAME")]
        public string GPSNAME { get; set; }
        /// <summary>
        /// �ݷ�����
        /// </summary>
        /// <returns></returns>
        [Column("ACCOMPANYINGNUMBER")]
        public int? AccompanyingNumber { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// Σ��Ʒ����
        /// </summary>
        /// <returns></returns>
        [Column("HAZARDOUSNAME")]
        public string HazardousName { get; set; }
        /// <summary>
        /// ������ǩ��ͼƬ
        /// </summary>
        /// <returns></returns>
        [Column("PROCESSINGSIGN")]
        public string ProcessingSign { get; set; }
        /// <summary>
        /// ���ƺ�
        /// </summary>
        /// <returns></returns>
        [Column("CARNO")]
        public string CarNo { get; set; }
        /// <summary>
        /// Σ��Ʒ�������0ΪĬ�� 1Ϊ��ȷ�ϳ���  2Ϊ�ѽ��ӳ���
        /// </summary>
        /// <returns></returns>
        [Column("HAZARDOUSPROCESS")]
        public int? HazardousProcess { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("HANDOVERID")]
        public string HandoverId { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("OUTTIME")]
        public DateTime? OutTime { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// ������Ա
        /// </summary>
        /// <returns></returns>
        [Column("ACCOMPANYINGPERSON")]
        public string AccompanyingPerson { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("PROCESSINGID")]
        public string ProcessingId { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// ��¼״̬ 0Ϊ�ѿ�Ʊ 1Ϊ���ύ������ 2Ϊ��¼��GPS���� 3Ϊ����ͨ�� 4Ϊ�ѳ��� 99Ϊ�ܾ��볡
        /// </summary>
        /// <returns></returns>
        [Column("STATE")]
        public int? State { get; set; }
        /// <summary>
        /// ��ʻ֤ͼƬ��ַ
        /// </summary>
        /// <returns></returns>
        [Column("DRIVINGLICENSEURL")]
        public string DrivingLicenseUrl { get; set; }
        /// <summary>
        /// ������ǩ��ͼƬ
        /// </summary>
        /// <returns></returns>
        [Column("HANDOVERSIGN")]
        public string HandoverSign { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("HANDOVERNAME")]
        public string HandoverName { get; set; }
        /// <summary>
        /// ������λ
        /// </summary>
        /// <returns></returns>
        [Column("THECOMPANY")]
        public string TheCompany { get; set; }

        /// <summary>
        /// ������ʻ·�ߣ��������쳣��
        /// </summary>
        /// <returns></returns>
        [Column("DRIVINGROUTE")]
        public string DrivingRoute { get; set; }

        /// <summary>
        /// ����ͣ��ʱ�䣨�������쳣��
        /// </summary>
        /// <returns></returns>
        [Column("RESIDENCETIME")]
        public string ResidenceTime { get; set; }

        /// <summary>
        /// ������ʻ�ٶȣ��������쳣��
        /// </summary>
        /// <returns></returns>
        [Column("DRIVINGSPEED")]
        public string DrivingSpeed { get; set; }

        /// <summary>
        /// �Ƿ����ύ0δ�ύ 1���ύ 2�����ѳ���
        /// </summary>
        /// <returns></returns>
        [Column("ISSUBMIT")]
        public int? Issubmit { get; set; }

        /// <summary>
        /// �ֻ��ֻ�app����״̬Ĭ��0 ͨ��1 δͨ��2
        /// </summary>
        /// <returns></returns>
        [Column("APPSTATUE")]
        public int AppStatue { get; set; }

        /// <summary>
        /// �����볡ʱ��
        /// </summary>
        [Column("APPLYDATE")]
        public DateTime? ApplyDate { get; set; }

        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID)?Guid.NewGuid().ToString():ID;
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
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
                    }
        #endregion
    }
}