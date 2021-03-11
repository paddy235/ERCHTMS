using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EquipmentManage
{
    /// <summary>
    /// 描 述：特种设备基本信息表
    /// </summary>
    [Table("BIS_SPECIALEQUIPMENT")]
    public class SpecialEquipmentEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 设备编号
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNO")]
        public string EquipmentNo { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTTYPE")]
        public string EquipmentType { get; set; }
        /// <summary>
        /// 所属关系
        /// </summary>
        /// <returns></returns>
        [Column("AFFILIATION")]
        public string Affiliation { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNAME")]
        public string EquipmentName { get; set; }
        /// <summary>
        /// 外包单位
        /// </summary>
        /// <returns></returns>
        [Column("EPIBOLYDEPT")]
        public string EPIBOLYDEPT { get; set; }
        /// <summary>
        /// 外包单位ID
        /// </summary>
        /// <returns></returns>
        [Column("EPIBOLYDEPTID")]
        public string EPIBOLYDEPTID { get; set; }
        /// <summary>
        /// 外包工程
        /// </summary>
        /// <returns></returns>
        [Column("EPIBOLYPROJECT")]
        public string EPIBOLYPROJECT { get; set; }
        /// <summary>
        /// 外包工程ID
        /// </summary>
        /// <returns></returns>
        [Column("EPIBOLYPROJECTID")]
        public string EPIBOLYPROJECTID { get; set; }

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
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 定期检验记录附件ID
        /// </summary>
        /// <returns></returns>
        [Column("CHECKFILEID")]
        public string CheckFileID { get; set; }

        /// <summary>
        /// 检验记录ID
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTANCE")]
        public string Acceptance { get; set; }
        /// <summary>
        /// 使用状况（未启用/在用/停用/报废）
        /// </summary>
        /// <returns></returns>
        [Column("STATE")]
        public string State { get; set; }
        /// <summary>
        /// 出厂年月
        /// </summary>
        /// <returns></returns>
        [Column("FACTORYDATE")]
        public DateTime? FactoryDate { get; set; }
        /// <summary>
        /// 出厂编号
        /// </summary>
        /// <returns></returns>
        [Column("FACTORYNO")]
        public string FactoryNo { get; set; }
        /// <summary>
        /// 制造单位名称
        /// </summary>
        /// <returns></returns>
        [Column("OUTPUTDEPTNAME")]
        public string OutputDeptName { get; set; }
        /// <summary>
        /// 是否经过检查验收
        /// </summary>
        /// <returns></returns>
        [Column("ISCHECK")]
        public string IsCheck { get; set; }
        /// <summary>
        /// 操作人员ID
        /// </summary>
        /// <returns></returns>
        [Column("OPERUSERID")]
        public string OperUserID { get; set; }
        /// <summary>
        /// 操作人员
        /// </summary>
        /// <returns></returns>
        [Column("OPERUSER")]
        public string OperUser { get; set; }
        /// <summary>
        /// 证书附件ID
        /// </summary>
        /// <returns></returns>
        [Column("CERTIFICATEID")]
        public string CertificateID { get; set; }
        /// <summary>
        /// 使用登记证书编号
        /// </summary>
        /// <returns></returns>
        [Column("CERTIFICATENO")]
        public string CertificateNo { get; set; }
        /// <summary>
        /// 下次检验日期
        /// </summary>
        /// <returns></returns>
        [Column("NEXTCHECKDATE")]
        public DateTime? NextCheckDate { get; set; }
        /// <summary>
        /// 检验日期
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATE")]
        public DateTime? CheckDate { get; set; }
        /// <summary>
        /// 检验周期
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATECYCLE")]
        public string CheckDateCycle { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICT")]
        public string District { get; set; }
        /// <summary>
        /// 所在区域ID
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTID")]
        public string DistrictId { get; set; }
        /// <summary>
        /// 所在区域CODE
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTCODE")]
        public string DistrictCode { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        /// <returns></returns>
        [Column("SPECIFICATIONS")]
        public string Specifications { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        /// <returns></returns>
        [Column("TELEPHONE")]
        public string Telephone { get; set; }
        /// <summary>
        /// 安全管理人员ID
        /// </summary>
        /// <returns></returns>
        [Column("SECURITYMANAGERUSERID")]
        public string SecurityManagerUserID { get; set; }
        /// <summary>
        /// 安全管理人员
        /// </summary>
        /// <returns></returns>
        [Column("SECURITYMANAGERUSER")]
        public string SecurityManagerUser { get; set; }
        /// <summary>
        /// 管控部门
        /// </summary>
        /// <returns></returns>
        [Column("CONTROLDEPT")]
        public string ControlDept { get; set; }
        /// <summary>
        /// 管控部门ID
        /// </summary>
        /// <returns></returns>
        [Column("CONTROLDEPTID")]
        public string ControlDeptID { get; set; }

        /// <summary>
        /// 管控部门CODE
        /// </summary>
        /// <returns></returns>
        [Column("CONTROLDEPTCODE")]
        public string ControlDeptCode { get; set; }
        /// <summary>
        /// 购置时间
        /// </summary>
        /// <returns></returns>
        [Column("PURCHASETIME")]
        public DateTime? PurchaseTime { get; set; }
        /// <summary>
        /// 关联词
        /// </summary>
        [Column("RELWORD")]
        public string RelWord { get; set; }

        /// <summary>
        /// 离场时间
        /// </summary>
        [Column("DEPARTURETIME")]
        public DateTime? DepartureTime { get; set; }

        /// <summary>
        /// 离场原因
        /// </summary>
        [Column("DEPARTUREREASON")]
        public string DepartureReason { get; set; }
        /// <summary>
        /// 使用地点
        /// </summary>
        [Column("EMPLOYSITE")]
        public string EmploySite { get; set; }
        /// <summary>
        /// 设备类别
        /// </summary>
        [Column("EQUIPMENTKIND")]
        public string EquipmentKind { get; set; }
        /// <summary>
        /// 设备品种
        /// </summary>
        [Column("EQUIPMENTBREED")]
        public string EquipmentBreed { get; set; }
        /// <summary>
        /// 使用单位ID
        /// </summary>
        [Column("EMPLOYDEPTID")]
        public string EmployDeptId { get; set; }
        /// <summary>
        /// 使用单位
        /// </summary>
        [Column("EMPLOYDEPT")]
        public string EmployDept { get; set; }
        /// <summary>
        /// 管控专业
        /// </summary>
        [Column("CONTROLMAJOR")]
        public string ControlMajor { get; set; }
        /// <summary>
        /// 操作人员证书编号
        /// </summary>
        [Column("CERTIFICATENUMBER")]
        public string CertificateNumber { get; set; }
        /// <summary>
        /// 设备注册代码
        /// </summary>
        [Column("EQUIPMENTREGISTERNO")]
        public string EquipmentRegisterNo { get; set; }
        /// <summary>
        /// 维保单位
        /// </summary>
        [Column("MAINTAINUNIT")]
        public string MaintainUnit { get; set; }
        /// <summary>
        /// 设备所在地
        /// </summary>
        [Column("LOCATION")]
        public string Location { get; set; }
        /// <summary>
        /// 检验单位
        /// </summary>
        [Column("EXAMINEUNIT")]
        public string ExamineUnit { get; set; }
        /// <summary>
        /// 报检日期
        /// </summary>
        [Column("REPORTEXAMINEDATE")]
        public DateTime? ReportExamineDate { get; set; }
        /// <summary>
        /// 监察单上报日期
        /// </summary>
        [Column("EXAMINEAPPEARDATE")]
        public DateTime? ExamineAppearDate { get; set; }
        /// <summary>
        /// 受理状态
        /// </summary>
        [Column("ACCEPTSTATE")]
        public string AcceptState { get; set; }
        /// <summary>
        /// 检验结论
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEVERDICT")]
        public string ExamineVerdict { get; set; }
        /// <summary>
        /// 检验报告编号
        /// </summary>
        /// <returns></returns>
        [Column("REPORTNUMBER")]
        public string ReportNumber { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
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
        /// 编辑调用
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