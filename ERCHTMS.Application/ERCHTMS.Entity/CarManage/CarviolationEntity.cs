using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// �� ����Υ����Ϣ��
    /// </summary>
    [Table("BIS_CARVIOLATION")]
    public class CarviolationEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// ���ƺ�
        /// </summary>
        /// <returns></returns>
        [Column("CARDNO")]
        public string CardNo { get; set; }
        /// <summary>
        /// ˾���绰
        /// </summary>
        /// <returns></returns>
        [Column("PHONE")]
        public string Phone { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// �������� 0Ϊ�糧�೵ 1Ϊ˽�ҳ� 2Ϊ���񹫳� 3Ϊ�ݷó��� 4Ϊ���ϳ��� 5ΪΣ��Ʒ����
        /// </summary>
        /// <returns></returns>
        [Column("CARTYPE")]
        public int? CarType { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// Ĭ��Ϊ0  �� 1Ϊ��
        /// </summary>
        /// <returns></returns>
        [Column("ISPROCESS")]
        public int? IsProcess { get; set; }
        /// <summary>
        /// ��ʻ��
        /// </summary>
        /// <returns></returns>
        [Column("DIRVER")]
        public string Dirver { get; set; }
        /// <summary>
        /// ������¼ID
        /// </summary>
        /// <returns></returns>
        [Column("CID")]
        public string CID { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// �����ʩ
        /// </summary>
        /// <returns></returns>
        [Column("PROCESSMEASURE")]
        public string ProcessMeasure { get; set; }
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
        /// 0Ϊ���� 1Ϊƫ�뺽�� 2Ϊ��ʱ
        /// </summary>
        /// <returns></returns>
        [Column("VIOLATIONTYPE")]
        public int? ViolationType { get; set; }
        /// <summary>
        /// ����Υ������
        /// </summary>
        /// <returns></returns>
        [Column("VIOLATIONMSG")]
        public string ViolationMsg { get; set; }

        /// <summary>
        /// ץ�ĵص�
        /// </summary>
        /// <returns></returns>
        [Column("ADDRESS")]
        public string Address { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("SPEED")]
        public int Speed { get; set; }

        /// <summary>
        /// ����url
        /// </summary>
        [Column("PLATEPICURL")]
        public string platePicUrl { get; set; }

        /// <summary>
        ///����url
        /// </summary>
        [Column("VEHICLEPICURL")]
        public string vehiclePicUrl { get; set; }

        /// <summary>
        /// �����ͺ� ��ͳ� С������
        /// </summary>
        [Column("VEHICLETYPENAME")]
        public string vehicleTypeName { get; set; }

        [Column("DEPTNAME")]
        public string DeptName { get; set; }

        /// <summary>
        /// ͼƬ������Ψһ���
        /// </summary>
        [Column("HIKPICSVR")]
        public string HikPicSvr { get; set; }

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