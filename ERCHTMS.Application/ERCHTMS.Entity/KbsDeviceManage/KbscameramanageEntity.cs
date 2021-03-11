using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.KbsDeviceManage
{
    /// <summary>
    /// �� ��������ʲ����ͷ����
    /// </summary>
    [Table("BIS_KBSCAMERAMANAGE")]
    public class KbscameramanageEntity : BaseEntity
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
        /// ����ͷID
        /// </summary>
        /// <returns></returns>
        [Column("CAMERAID")]
        public string CameraId { get; set; }
        /// <summary>
        /// ����ͷ����
        /// </summary>
        /// <returns></returns>
        [Column("CAMERANAME")]
        public string CameraName { get; set; }
        /// <summary>
        /// �����ֶ�
        /// </summary>
        /// <returns></returns>
        [Column("SORT")]
        public int? Sort { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("AREAID")]
        public string AreaId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("AREANAME")]
        public string AreaName { get; set; }
        /// <summary>
        /// ����CODE
        /// </summary>
        /// <returns></returns>
        [Column("AREACODE")]
        public string AreaCode { get; set; }
        /// <summary>
        /// ¥����
        /// </summary>
        /// <returns></returns>
        [Column("FLOORNO")]
        public string FloorNo { get; set; }
        /// <summary>
        /// ����ͷIP
        /// </summary>
        /// <returns></returns>
        [Column("CAMERAIP")]
        public string CameraIP { get; set; }
        /// <summary>
        /// ����ͷ����
        /// </summary>
        /// <returns></returns>
        [Column("CAMERAPOINT")]
        public string CameraPoint { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("OPERUSERNAME")]
        public string OperuserName { get; set; }
        /// <summary>
        /// ����ͷ���
        /// </summary>
        /// <returns></returns>
        [Column("CAMERATYPE")]
        public string CameraType { get; set; }
        /// <summary>
        /// ����ͷ���ID
        /// </summary>
        /// <returns></returns>
        [Column("CAMERATYPEID")]
        public int? CameraTypeId { get; set; }

        /// <summary>
        /// ״̬ ����/����
        /// </summary>
        [Column("STATE")]
        public string State { get; set; }

        /// <summary>
        /// �����������
        /// </summary>
        [Column("MONITORINGAREA")]
        public string MonitoringArea { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
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