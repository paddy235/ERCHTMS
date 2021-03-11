using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ�������Ϣ������
    /// </summary>
    [Table("BIS_LABOREQUIPMENTINFO")]
    public class LaborequipmentinfoEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����ID
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
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("ASSID")]
        public string AssId { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
        /// <summary>
        /// �û�ID
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("SIZE")]
        public string Size { get; set; }
        /// <summary>
        /// �䱸����
        /// </summary>
        /// <returns></returns>
        [Column("SHOULDNUM")]
        public int? ShouldNum { get; set; }
        /// <summary>
        /// ��Ʒ��Ч��
        /// </summary>
        /// <returns></returns>
        [Column("VALIDITYPERIOD")]
        public DateTime? ValidityPeriod { get; set; }
        /// <summary>
        /// ����ԭ��
        /// </summary>
        /// <returns></returns>
        [Column("RESON")]
        public string Reson { get; set; }
        /// <summary>
        /// �������� 0Ϊ������Ʒ�� 1Ϊ�������� 2Ϊ��������
        /// </summary>
        /// <returns></returns>
        [Column("LABORTYPE")]
        public int? LaborType { get; set; }

        /// <summary>
        /// Ʒ�Ƴ���
        /// </summary>
        /// <returns></returns>
        [Column("BRAND")]
        public string Brand { get; set; }
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

    public class Laborff
    {
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        public string ID { get; set; }
        /// <summary>
        /// ��Ʒ�ͺ�
        /// </summary>
        /// <returns></returns>
        public string Model { get; set; }
        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }
       
        /// <summary>
        /// ��Ʒ��λ
        /// </summary>
        /// <returns></returns>
        public string Unit { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public int? IssueNum { get; set; }
        /// <summary>
        /// ʹ�ò�������
        /// </summary>
        /// <returns></returns>
        public string DeptName { get; set; }
        /// <summary>
        /// ʹ�õ�λ����
        /// </summary>
        /// <returns></returns>
        [Column("ORGNAME")]
        public string OrgName { get; set; }
        /// <summary>
        /// ʹ�ø�λ����
        /// </summary>
        /// <returns></returns>
        public string PostName { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public int UserCount { get; set; }

        /// <summary>
        /// ��������/��
        /// </summary>
        /// <returns></returns>
        public int PerCount { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public int Count { get; set; }

    }
}