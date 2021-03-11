using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace ERCHTMS.Entity.ToolEquipmentManage
{
    /// <summary>
    /// 描 述：工器具基础信息表
    /// </summary>
    [Table("BIS_TOOLEQUIPMENT")]
    public class ToolequipmentEntity : BaseEntity
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
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
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
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
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
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
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
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }


        
        /// <summary>
        /// 设备名称id
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTVALUE")]
        public string EquipmentValue { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNAME")]
        public string EquipmentName { get; set; }
        /// <summary>
        /// 工器具类别
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTTYPE")]
        public string EquipmentType { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNO")]
        public string EquipmentNo { get; set; }
        /// <summary>
        /// 安全管理人员
        /// </summary>
        /// <returns></returns>
        [Column("SECURITYMANAGERUSER")]
        public string SecurityManagerUser { get; set; }
        /// <summary>
        /// 安全管理人员ID
        /// </summary>
        /// <returns></returns>
        [Column("SECURITYMANAGERUSERID")]
        public string SecurityManagerUserId { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        /// <returns></returns>
        [Column("TELEPHONE")]
        public string Telephone { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        /// <returns></returns>
        [Column("SPECIFICATIONS")]
        public string Specifications { get; set; }
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
        /// 存放位置
        /// </summary>
        /// <returns></returns>
        [Column("DEPOSITARY")]
        public string Depositary { get; set; }
        /// <summary>
        /// 试验日期
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATE")]
        public DateTime? CheckDate { get; set; }
        /// <summary>
        /// 下次试验日期
        /// </summary>
        /// <returns></returns>
        [Column("NEXTCHECKDATE")]
        public DateTime? NextCheckDate { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        [Column("VALIDITYDATE")]
        public DateTime? ValidityDate { get; set; }

        /// <summary>
        /// 试验人员
        /// </summary>
        /// <returns></returns>
        [Column("OPERUSER")]
        public string OperUser { get; set; }
        /// <summary>
        /// 试验人员ID
        /// </summary>
        /// <returns></returns>
        [Column("OPERUSERID")]
        public string OperUserId { get; set; }
        /// <summary>
        /// 是否经过检查验收
        /// </summary>
        /// <returns></returns>
        [Column("ISCHECK")]
        public string IsCheck { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        /// <returns></returns>
        [Column("OUTPUTDEPTNAME")]
        public string OutputDeptName { get; set; }
        /// <summary>
        /// 出厂编号
        /// </summary>
        /// <returns></returns>
        [Column("FACTORYNO")]
        public string FactoryNo { get; set; }
        /// <summary>
        /// 出厂年月
        /// </summary>
        /// <returns></returns>
        [Column("FACTORYDATE")]
        public DateTime? FactoryDate { get; set; }
        /// <summary>
        /// 使用状况（未启用/在用/停用/报废）
        /// </summary>
        /// <returns></returns>
        [Column("STATE")]
        public string State { get; set; }
        /// <summary>
        /// 管理人员ID
        /// </summary>
        /// <returns></returns>
        [Column("CONTROLUSERID")]
        public string ControlUserId { get; set; }
        /// <summary>
        /// 管理人员name
        /// </summary>
        /// <returns></returns>
        [Column("CONTROLUSERNAME")]
        public string ControlUserName { get; set; }
        /// <summary>
        /// 管理部门name
        /// </summary>
        /// <returns></returns>
        [Column("CONTROLDEPT")]
        public string ControlDept { get; set; }
        /// <summary>
        /// 管理部门ID
        /// </summary>
        /// <returns></returns>
        [Column("CONTROLDEPTID")]
        public string ControlDeptId { get; set; }
        /// <summary>
        /// 管理部门Code
        /// </summary>
        /// <returns></returns>
        [Column("CONTROLDEPTCODE")]
        public string ControlDeptCode { get; set; }
        /// <summary>
        /// 检验周期
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATECYCLE")]
        public string CheckDateCycle { get; set; }
        /// <summary>
        /// 检查记录id
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTANCE")]
        public string Acceptance { get; set; }


        /// <summary>
        /// 工具大类
        /// </summary>
        [Column("TOOLTYPE")]
        public string ToolType { get; set; }


        /// <summary>
        /// 评价
        /// </summary>
        [Column("APPRAISE")]
        public string Appraise { get; set; }

        /// <summary>
        /// 说明书附件id
        /// </summary>
        [Column("DESCRIPTIONFILEID")]
        public string DescriptionFileId { get; set; }

        /// <summary>
        /// 合同证附件id
        /// </summary>
        [Column("CONTRACTFILEID")]
        public string ContractFileId { get; set; }


        /// <summary>
        /// 所属部门
        /// </summary>
        [Column("BELONGDEPT")]
        public string BelongDept { get; set; }

        /// <summary>
        /// 所属部门id
        /// </summary>
        [Column("BELONGDEPTID")]
        public string BelongDeptId { get; set; }

        /// <summary>
        /// 所属部门Code
        /// </summary>
        [Column("BELONGDEPTCODE")]
        public string BelongDeptCode { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [Column("UNIT")]
        public string  Unit { get; set; }


        /// <summary>
        /// 数量
        /// </summary>
        [Column("QUANTITY")]
        public string Quantity { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Column("TIMEUNIT")]
        public string TimeUnit { get; set; }

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