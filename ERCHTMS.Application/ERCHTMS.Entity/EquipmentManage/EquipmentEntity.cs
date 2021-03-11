using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EquipmentManage
{
    /// <summary>
    /// �� ������ͨ�豸������Ϣ��
    /// </summary>
    [Table("BIS_EQUIPMENT")]
    public class EquipmentEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AutoId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        
        /// <summary>
        /// �豸���
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNO")]
        public string EquipmentNo { get; set; }
        /// <summary>
        /// �豸����
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTTYPE")]
        public string EquipmentType { get; set; }
        /// <summary>
        /// ������ϵ
        /// </summary>
        /// <returns></returns>
        [Column("AFFILIATION")]
        public string Affiliation { get; set; }
        /// <summary>
        /// �豸����
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNAME")]
        public string EquipmentName { get; set; }
        /// <summary>
        /// �����λ
        /// </summary>
        /// <returns></returns>
        [Column("EPIBOLYDEPT")]
        public string EPIBOLYDEPT { get; set; }
        /// <summary>
        /// �����λID
        /// </summary>
        /// <returns></returns>
        [Column("EPIBOLYDEPTID")]
        public string EPIBOLYDEPTID { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("EPIBOLYPROJECT")]
        public string EPIBOLYPROJECT { get; set; }
        /// <summary>
        /// �������ID
        /// </summary>
        /// <returns></returns>
        [Column("EPIBOLYPROJECTID")]
        public string EPIBOLYPROJECTID { get; set; }  
        /// <summary>
        /// ʹ��״����δ����/����/ͣ��/���ϣ�
        /// </summary>
        /// <returns></returns>
        [Column("STATE")]
        public string State { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("FACTORYDATE")]
        public DateTime? FactoryDate { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("FACTORYNO")]
        public string FactoryNo { get; set; }
        /// <summary>
        /// ���쵥λ����
        /// </summary>
        /// <returns></returns>
        [Column("OUTPUTDEPTNAME")]
        public string OutputDeptName { get; set; }
        /// <summary>
        /// �Ƿ񾭹��������
        /// </summary>
        /// <returns></returns>
        [Column("ISCHECK")]
        public string IsCheck { get; set; }       
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICT")]
        public string District { get; set; }
        /// <summary>
        /// ��������ID
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTID")]
        public string DistrictId { get; set; }
        /// <summary>
        /// ��������CODE
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTCODE")]
        public string DistrictCode { get; set; }
        /// <summary>
        /// ����ͺ�
        /// </summary>
        /// <returns></returns>
        [Column("SPECIFICATIONS")]
        public string Specifications { get; set; }
        /// <summary>
        /// ��ϵ�绰
        /// </summary>
        /// <returns></returns>
        [Column("TELEPHONE")]
        public string Telephone { get; set; }
        /// <summary>
        /// ��ȫ������ԱID
        /// </summary>
        /// <returns></returns>
        [Column("SECURITYMANAGERUSERID")]
        public string SecurityManagerUserID { get; set; }
        /// <summary>
        /// ��ȫ������Ա
        /// </summary>
        /// <returns></returns>
        [Column("SECURITYMANAGERUSER")]
        public string SecurityManagerUser { get; set; }
        /// <summary>
        /// �ܿز���
        /// </summary>
        /// <returns></returns>
        [Column("CONTROLDEPT")]
        public string ControlDept { get; set; }
        /// <summary>
        /// �ܿز���ID
        /// </summary>
        /// <returns></returns>
        [Column("CONTROLDEPTID")]
        public string ControlDeptID { get; set; }

        /// <summary>
        /// �ܿز���CODE
        /// </summary>
        /// <returns></returns>
        [Column("CONTROLDEPTCODE")]
        public string ControlDeptCode { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("PURCHASETIME")]
        public DateTime? PurchaseTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("USEADDRESS")]
        public string UseAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("RELWORD")]
        public string RelWord { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("REMARK")]
        public string Remark { get; set; }
        
        /// <summary>
        /// �볡ʱ��
        /// </summary>
        [Column("DEPARTURETIME")]
        public DateTime? DepartureTime { get; set;}

        /// <summary>
        /// �볡ԭ��
        /// </summary>
        [Column("DEPARTUREREASON")]
        public string DepartureReason { get; set; }
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