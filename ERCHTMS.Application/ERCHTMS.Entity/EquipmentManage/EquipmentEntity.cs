using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EquipmentManage
{
    /// <summary>
    /// 描 述：普通设备基本信息表
    /// </summary>
    [Table("BIS_EQUIPMENT")]
    public class EquipmentEntity : BaseEntity
    {
        #region 实体成员
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
        /// 离场时间
        /// </summary>
        [Column("DEPARTURETIME")]
        public DateTime? DepartureTime { get; set;}

        /// <summary>
        /// 离场原因
        /// </summary>
        [Column("DEPARTUREREASON")]
        public string DepartureReason { get; set; }
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