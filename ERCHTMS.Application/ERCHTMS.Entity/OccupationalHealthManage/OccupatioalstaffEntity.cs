using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OccupationalHealthManage
{
    /// <summary>
    /// �� ����ְҵ����Ա��
    /// </summary>
    [Table("BIS_OCCUPATIOALSTAFF")]
    public class OccupatioalstaffEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ��¼���
        /// </summary>
        /// <returns></returns>
        [Column("OCCID")]
        public string OccId { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("MECHANISMNAME")]
        public string MechanismName { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("INSPECTIONTIME")]
        public DateTime? InspectionTime { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("INSPECTIONNUM")]
        public int? InspectionNum { get; set; }
        /// <summary>
        /// ְҵ������
        /// </summary>
        /// <returns></returns>
        [Column("PATIENTNUM")]
        public int? PatientNum { get; set; }
        /// <summary>
        /// �Ƿ��и���
        /// </summary>
        /// <returns></returns>
        [Column("ISANNEX")]
        public int? IsAnnex { get; set; }
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
        public string ModifyUserid { get; set; }
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
        /// �쳣����
        /// </summary>
        [Column("UNUSUALNUM")]
        public int? UnusualNum { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.OccId = string.IsNullOrEmpty(OccId) ? Guid.NewGuid().ToString() : OccId;
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
            this.OccId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserid = OperatorProvider.Provider.Current().UserId;
        }
        #endregion
    }
}