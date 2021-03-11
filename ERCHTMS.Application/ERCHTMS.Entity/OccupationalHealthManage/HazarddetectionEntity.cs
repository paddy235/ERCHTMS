using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OccupationalHealthManage
{
    /// <summary>
    /// �� ����ְҵ��Σ�����ؼ��
    /// </summary>
    [Table("BIS_HAZARDDETECTION")]
    public class HazarddetectionEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("HID")]
        public string HId { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("AREAID")]
        public string AreaId { get; set; }
        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <returns></returns>
        [Column("AREAVALUE")]
        public string AreaValue { get; set; }
        /// <summary>
        /// ְҵ��Σ������ID
        /// </summary>
        /// <returns></returns>
        [Column("RISKID")]
        public string RiskId { get; set; }
        /// <summary>
        /// ְҵ��Σ����������
        /// </summary>
        /// <returns></returns>
        [Column("RISKVALUE")]
        public string RiskValue { get; set; }
        /// <summary>
        /// ����/�����ص�
        /// </summary>
        /// <returns></returns>
        [Column("LOCATION")]
        public string Location { get; set; }
        /// <summary>
        /// ��ʼ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("STARTTIME")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// ��������ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ENDTIME")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// ����ָ�꼰��׼
        /// </summary>
        /// <returns></returns>
        [Column("STANDARD")]
        public string Standard { get; set; }
        /// <summary>
        /// ��⸺����ID
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONUSERID")]
        public string DetectionUserId { get; set; }
        /// <summary>
        /// ��⸺����
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONUSERNAME")]
        public string DetectionUserName { get; set; }
        /// <summary>
        /// �Ƿ񳬱�
        /// </summary>
        /// <returns></returns>
        [Column("ISEXCESSIVE")]
        public int? IsExcessive { get; set; }
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
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.HId = string.IsNullOrEmpty(HId) ? Guid.NewGuid().ToString() : HId;
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
            this.HId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
                    }
        #endregion
    }
}