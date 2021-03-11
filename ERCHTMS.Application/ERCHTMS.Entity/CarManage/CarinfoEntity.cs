using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    [Table("BIS_CARINFO")]
    public class CarinfoEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
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
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// ���ƺ�
        /// </summary>
        /// <returns></returns>
        [Column("CARNO")]
        public string CarNo { get; set; }
        /// <summary>
        /// ��ʻ��
        /// </summary>
        /// <returns></returns>
        [Column("DIRVER")]
        public string Dirver { get; set; }

        /// <summary>
        /// ��ʻ��Id
        /// </summary>
        /// <returns></returns>
        [Column("DIRVERID")]
        public string DirverId { get; set; }

        /// <summary>
        /// ˾���绰
        /// </summary>
        /// <returns></returns>
        [Column("PHONE")]
        public string Phone { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("INSPERCTIONDATE")]
        public DateTime? InsperctionDate { get; set; }
        /// <summary>
        /// �´��������
        /// </summary>
        /// <returns></returns>
        [Column("NEXTINSPERCTIONDATE")]
        public DateTime? NextInsperctionDate { get; set; }
        /// <summary>
        /// �������� 0Ϊ�糧�೵ 1Ϊ˽�ҳ� 2Ϊ���񹫳� 3Ϊ�ݷó��� 4Ϊ���ϳ��� 5ΪΣ��Ʒ���� 6��ʱͨ�г���
        /// </summary>
        /// <returns></returns>
        [Column("TYPE")]
        public int Type { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("NUMBERLIMIT")]
        public int? NumberLimit { get; set; }
        /// <summary>
        /// ��ʻ֤ͼƬ��ַ
        /// </summary>
        /// <returns></returns>
        [Column("DRIVERLICENSEURL")]
        public string DriverLicenseUrl { get; set; }
        /// <summary>
        /// ��ʻ֤ͼƬ��ַ
        /// </summary>
        /// <returns></returns>
        [Column("DRIVINGLICENSEURL")]
        public string DrivingLicenseUrl { get; set; }
        /// <summary>
        /// �����ͺ�
        /// </summary>
        /// <returns></returns>
        [Column("MODEL")]
        public string Model { get; set; }
        /// <summary>
        /// ��λ�豸ID
        /// </summary>
        /// <returns></returns>
        [Column("GPSID")]
        public string GpsId { get; set; }
        /// <summary>
        /// ��λ�豸����
        /// </summary>
        /// <returns></returns>
        [Column("GPSNAME")]
        public string GpsName { get; set; }

        /// <summary>
        /// �Ƿ���Ȩ
        /// </summary>
        [Column("ISAUTHORIZED")]
        public int? IsAuthorized { get; set; }

        /// <summary>
        /// ��Ȩ�û�Id
        /// </summary>
        [Column("AUTHUSERID")]
        public string AuthUserId { get; set; }

        /// <summary>
        /// ��Ȩ�û�����
        /// </summary>
        [Column("AUTHUSERNAME")]
        public string AuthUserName { get; set; }

        /// <summary>
        /// �Ƿ�����0��1��
        /// </summary>
        [Column("ISENABLE")]
        public int IsEnable { get; set; }


        /// <summary>
        /// ��ע
        /// </summary>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// ͨ���Ÿ�ID
        /// </summary>
        [Column("CURRENTGID")]
        public string Currentgid { get; set; }
        /// <summary>
        /// ͨ���Ÿ�����
        /// </summary>
        [Column("CURRENTGNAME")]
        public string Currentgname { get; set; }

        /// <summary>
        /// ����״̬ 0δͨ�� 1ͨ��
        /// </summary>
        [Column("STATE")]
        public string State { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Column("DEPTNAME")]
        public string Deptname { get; set; }

        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("STARTTIME")]
        public DateTime? Starttime { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ENDTIME")]
        public DateTime? Endtime { get; set; }



        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CreateDate = DateTime.Now;
            this.IsEnable = 0;
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