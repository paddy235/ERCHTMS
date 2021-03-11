using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// �� �����豸��¼��Ա������־
    /// </summary>
    [Table("BIS_HIKINOUTLOG")]
    public class HikinoutlogEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 0Ϊ�� 1Ϊ��
        /// </summary>
        /// <returns></returns>
        [Column("INOUT")]
        public int? InOut { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// ��Ա����
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �������ץ��ͼƬ
        /// </summary>
        /// <returns></returns>
        [Column("SCREENSHOT")]
        public string ScreenShot { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 0Ϊ˫����Ա 1Ϊ˫�������Ա 2Ϊ������ʱ��Ա
        /// </summary>
        /// <returns></returns>
        [Column("USERTYPE")]
        public int? UserType { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }
        /// <summary>
        /// �����볡ID
        /// </summary>
        /// <returns></returns>
        [Column("INID")]
        public string InId { get; set; }
        /// <summary>
        /// ����ʱ�� (δ����Ϊ��)
        /// </summary>
        /// <returns></returns>
        [Column("OUTTIME")]
        public DateTime? OutTime { get; set; }
        /// <summary>
        /// �Ž�������
        /// </summary>
        /// <returns></returns>
        [Column("DEVICENAME")]
        public string DeviceName { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// �Ž��豸���� 0Ϊ����ʶ���豸 1Ϊ������բ�豸 2λ�Ž�ˢ���豸
        /// </summary>
        /// <returns></returns>
        [Column("DEVICETYPE")]
        public int? DeviceType { get; set; }
        /// <summary>
        /// �Ƿ���� 0Ϊδ���� 1Ϊ����
        /// </summary>
        /// <returns></returns>
        [Column("ISOUT")]
        public int? IsOut { get; set; }
        /// <summary>
        /// �Ž�����������
        /// </summary>
        /// <returns></returns>
        [Column("AREANAME")]
        public string AreaName { get; set; }
        /// <summary>
        /// �Ž��¼����� 1Ϊ����ͨ���¼� 2Ϊ���������¼� 3Ϊ�Ž�ˢ���¼� 4Ϊ�Ž�ָ��ͨ���¼�
        /// </summary>
        /// <returns></returns>
        [Column("EVENTTYPE")]
        public int? EventType { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// �û�ID�������˫�����û���Ϊ��
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// �Ž����ں���ƽ̨ID
        /// </summary>
        /// <returns></returns>
        [Column("DEVICEHIKID")]
        public string DeviceHikID { get; set; }
        
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