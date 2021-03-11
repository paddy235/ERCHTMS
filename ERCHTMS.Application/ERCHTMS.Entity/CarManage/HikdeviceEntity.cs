using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// �� �����Ž��豸����
    /// </summary>
    [Table("BIS_HIKDEVICE")]
    public class HikdeviceEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// �豸��ƽ̨�����õ�IP
        /// </summary>
        /// <returns></returns>
        [Column("DEVICEIP")]
        public string DeviceIP { get; set; }
        /// <summary>
        /// �豸�ڰ���ƽ̨�е�ID
        /// </summary>
        /// <returns></returns>
        [Column("HIKID")]
        public string HikID { get; set; }
        /// <summary>
        /// �豸�����������ƣ�һ�Ÿڡ�����ڡ����Ÿڣ�
        /// </summary>
        /// <returns></returns>
        [Column("AREANAME")]
        public string AreaName { get; set; }
        /// <summary>
        /// �������� 0��ʾ�����豸 1��ʾ�����豸
        /// </summary>
        /// <returns></returns>
        [Column("OUTTYPE")]
        public int? OutType { get; set; }
        /// <summary>
        /// �豸����
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
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// �豸����
        /// </summary>
        /// <returns></returns>
        [Column("DEVICETYPE")]
        public string DeviceType { get; set; }
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