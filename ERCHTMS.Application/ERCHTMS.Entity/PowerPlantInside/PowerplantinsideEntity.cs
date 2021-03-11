using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.PowerPlantInside
{
    /// <summary>
    /// 描 述：单位内部快报
    /// </summary>
    [Table("BIS_POWERPLANTINSIDE")]
    public class PowerplantinsideEntity : BaseEntity
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
        /// 事故事件名称
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTEVENTNAME")]
        public string AccidentEventName { get; set; }
        /// <summary>
        /// 事故事件编号
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTEVENTNO")]
        public string AccidentEventNo { get; set; }
        /// <summary>
        /// 事故事件类别
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTEVENTTYPE")]
        public string AccidentEventType { get; set; }
        /// <summary>
        /// 事故事件性质
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTEVENTPROPERTY")]
        public string AccidentEventProperty { get; set; }
        /// <summary>
        /// 事故事件因素
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTEVENTCAUSE")]
        public string AccidentEventCause { get; set; }
        /// <summary>
        /// 机组运行方式
        /// </summary>
        /// <returns></returns>
        [Column("OPERATIONMODE")]
        public string OperationMode { get; set; }
        /// <summary>
        /// 所属系统
        /// </summary>
        /// <returns></returns>
        [Column("BELONGSYSTEM")]
        public string BelongSystem { get; set; }
        /// <summary>
        /// 发生时间
        /// </summary>
        /// <returns></returns>
        [Column("HAPPENTIME")]
        public DateTime? HappenTime { get; set; }
        /// <summary>
        /// 地点（区域）
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICT")]
        public string District { get; set; }
        /// <summary>
        /// 区域id
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTID")]
        public string DistrictId { get; set; }
        /// <summary>
        /// 区域code
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTCODE")]
        public string DistrictCode { get; set; }
        /// <summary>
        /// 所属部门/单位
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPT")]
        public string BelongDept { get; set; }
        /// <summary>
        /// 所属部门/单位id
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTID")]
        public string BelongDeptId { get; set; }
        /// <summary>
        /// 所属部门/单位code
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTCODE")]
        public string BelongDeptCode { get; set; }
        /// <summary>
        /// 相关专业
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTY")]
        public string Specialty { get; set; }
        /// <summary>
        /// 事故事件快报人
        /// </summary>
        /// <returns></returns>
        [Column("CONTROLUSERNAME")]
        public string ControlUserName { get; set; }

        /// <summary>
        /// 事故事件快报人id
        /// </summary>
        /// <returns></returns>
        [Column("CONTROLUSERID")]
        public string ControlUserId { get; set; }

        /// <summary>
        /// 流程部门名称
        /// </summary>
        [Column("FLOWDEPTNAME")]
        public string FlowDeptName { get; set; }

        /// <summary>
        /// 流程部门编码/ID
        /// </summary>
        [Column("FLOWDEPT")]
        public string FlowDept { get; set; }

        /// <summary>
        /// 流程ID
        /// </summary>
        [Column("FLOWID")]
        public string FlowID { get; set; }

        /// <summary>
        /// 流程角色名称
        /// </summary>
        [Column("FLOWROLENAME")]
        public string FlowRoleName { get; set; }

        /// <summary>
        /// 流程角色编码/ID
        /// </summary>
        [Column("FLOWROLE")]
        public string FlowRole { get; set; }

        /// <summary>
        /// 流程名称
        /// </summary>
        [Column("FLOWNAME")]
        public string FlowName { get; set; }

        /// <summary>
        /// 流程完成情况
        /// </summary>
        /// <returns></returns>
        [Column("ISOVER")]
        public int? IsOver { get; set; }

        /// <summary>
        /// 是否保存成功
        /// </summary>
        /// <returns></returns>
        [Column("ISSAVED")]
        public int? IsSaved { get; set; }

        /// <summary>
        /// 影响事故事件因素名称
        /// </summary>
        [Column("ACCIDENTEVENTCAUSENAME")]
        public string AccidentEventCauseName { get; set; }
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