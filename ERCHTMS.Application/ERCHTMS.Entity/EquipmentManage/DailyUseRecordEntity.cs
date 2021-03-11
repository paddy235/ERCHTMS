using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EquipmentManage
{
    /// <summary>
    /// �� �����ճ�ʹ��״����¼��
    /// </summary>
    [Table("BIS_DAILYUSERECORD")]
    public class DailyUseRecordEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// �Ǽ���ԱID
        /// </summary>
        /// <returns></returns>
        [Column("REGISTERUSERID")]
        public string RegisterUserId { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("PROCESSRESULT")]
        public string ProcessResult { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
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
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// �Ǽ�ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("REGISTERDATE")]
        public DateTime? RegisterDate { get; set; }
        /// <summary>
        /// �Ǽ���Ա
        /// </summary>
        /// <returns></returns>
        [Column("REGISTERUSER")]
        public string RegisterUser { get; set; }
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
        public string DistrictID { get; set; }
        /// <summary>
        /// �쳣�����ʩ
        /// </summary>
        /// <returns></returns>
        [Column("TREATMENTMEASURES")]
        public string TreatmentMeasures { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICT")]
        public string District { get; set; }
        /// <summary>
        /// �����Ƿ�����
        /// </summary>
        /// <returns></returns>
        [Column("ISNORMAL")]
        public string IsNormal { get; set; }
        /// <summary>
        /// �豸ID
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTID")]
        public string EquipmentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// �豸����
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNAME")]
        public string EquipmentName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �豸���
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNO")]
        public string EquipmentNo { get; set; }
        /// <summary>
        /// �쳣�������
        /// </summary>
        /// <returns></returns>
        [Column("ABNORMALSITUATION")]
        public string AbnormalSituation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
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