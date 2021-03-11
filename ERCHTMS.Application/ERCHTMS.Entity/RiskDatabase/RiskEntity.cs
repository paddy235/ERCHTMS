using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.RiskDatabase
{
    /// <summary>
    /// 描 述：安全风险库
    /// </summary>
    public class RiskEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 区域名称
        /// </summary>
        /// <returns></returns>
        [Column("AREANAME")]
        public string AreaName { get; set; }
       
        /// <summary>
        /// 管控岗位名称
        /// </summary>
        /// <returns></returns>
        [Column("POSTNAME")]
        public string PostName { get; set; }
        /// <summary>
        /// 管控岗位ID
        /// </summary>
        /// <returns></returns>
        [Column("POSTID")]
        public string PostId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// 部门Code
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode{ get; set; }

        /// <summary>
        /// 专业名称
        /// </summary>
        /// <returns></returns>
        [Column("MAJORNAME")]
        public string MajorName { get; set; }
        /// <summary>
        /// 专业Code
        /// </summary>
        /// <returns></returns>
        [Column("MAJORCODE")]
        public string MajorCode { get; set; }
        /// <summary>
        /// 班组名称
        /// </summary>
        /// <returns></returns>
        [Column("TEAMNAME")]
        public string TeamName { get; set; }
        /// <summary>
        /// 班组Code
        /// </summary>
        /// <returns></returns>
        [Column("TEAMCODE")]
        public string TeamCode { get; set; }
        /// <summary>
        /// 风险等级
        /// </summary>
        /// <returns></returns>
        [Column("GRADE")]
        public string Grade { get; set; }
        /// <summary>
        /// 评价项目R
        /// </summary>
        /// <returns></returns>
        [Column("ITEMR")]
        public decimal? ItemR { get; set; }
        /// <summary>
        /// 评价项目C
        /// </summary>
        /// <returns></returns>
        [Column("ITEMC")]
        public decimal? ItemC { get; set; }
        /// <summary>
        /// 评价项目B
        /// </summary>
        /// <returns></returns>
        [Column("ITEMB")]
        public decimal? ItemB { get; set; }
        /// <summary>
        /// 评价项目A
        /// </summary>
        /// <returns></returns>
        [Column("ITEMA")]
        public decimal? ItemA { get; set; }
        /// <summary>
        /// 评价方式
        /// </summary>
        /// <returns></returns>
        [Column("WAY")]
        public string Way { get; set; }

        /// <summary>
        /// 事故类型编码
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTTYPE")]
        public string AccidentType { get; set; }
        /// <summary>
        /// 事故类型名称
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTNAME")]
        public string AccidentName { get; set; }
        /// <summary>
        /// 危险源或潜在事件
        /// </summary>
        /// <returns></returns>
        [Column("DANGERSOURCE")]
        public string DangerSource { get; set; }
        /// <summary>
        /// 危害描述
        /// </summary>
        /// <returns></returns>
        [Column("DESCRIPTION")]
        public string Description { get; set; }
        /// <summary>
        /// 风险后果
        /// </summary>
        /// <returns></returns>
        [Column("RESULT")]
        public string Result { get; set; }
        /// <summary>
        /// 作业步骤
        /// </summary>
        /// <returns></returns>
        [Column("WORKCONTENT")]
        public string WorkContent { get; set; }
        /// <summary>
        /// 区域Code
        /// </summary>
        /// <returns></returns>
        [Column("AREACODE")]
        public string AreaCode { get; set; }
        
        /// <summary>
        /// 关联区域ID
        /// </summary>
        /// <returns></returns>
        [Column("AREAID")]
        public string AreaId { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 风险数字等级(与风险等级对应)
        /// </summary>
        /// <returns></returns>
        [Column("GRADEVAL")]
        public int? GradeVal { get; set; }
        
        /// <summary>
        /// 记录状态,0:初始默认值,1:新增，2:修改
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public int? Status { get; set; }
        /// <summary>
        /// 创建用户ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户姓名
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改用户ID
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改记录时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户名称
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>
        /// <returns></returns>
        [Column("DELETEMARK")]
        public int? DeleteMark { get; set; }   
        /// <summary>
        /// 是否生效
        /// </summary>
        /// <returns></returns>
        [Column("ENABLEDMARK")]
        public int? EnabledMark { get; set; }

        /// <summary>
        /// 作业分级
        /// </summary>
        /// <returns></returns>
        [Column("HARMTYPE")]
        public string HarmType { get; set; }
        /// <summary>
        /// 导致的职业病或健康损伤
        /// </summary>
        /// <returns></returns>
        [Column("HARMPROPERTY")]
        public string HarmProperty { get; set; }
        /// <summary>
        /// 风险类别
        /// </summary>
        /// <returns></returns>
        [Column("RISKTYPE")]
        public string RiskType { get; set; }
        /// <summary>
        /// 工序
        /// </summary>
        /// <returns></returns>
        [Column("PROCESS")]
        public string Process { get; set; }
        /// <summary>
        /// 工作任务
        /// </summary>
        /// <returns></returns>
        [Column("WORKTASK")]
        public string WorkTask { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNAME")]
        public string EquipmentName { get; set; }
        /// <summary>
        /// 部件
        /// </summary>
        /// <returns></returns>
        [Column("PARTS")]
        public string Parts { get; set; }
        /// <summary>
        /// 风险后果分类
        /// </summary>
        /// <returns></returns>
        [Column("RESULTTYPE")]
        public string ResultType { get; set; }
        /// <summary>
        /// 风险描述
        /// </summary>
        /// <returns></returns>
        [Column("RISKDESC")]
        public string RiskDesc { get; set; }
        /// <summary>
        /// 隐患描述
        /// </summary>
        /// <returns></returns>
        [Column("HTDESC")]
        public string HtDesc { get; set; }
        /// <summary>
        /// 是否违章
        /// </summary>
        /// <returns></returns>
        [Column("ISWZ")]
        public string IsWZ { get; set; }
        /// <summary>
        /// 隐患等级
        /// </summary>
        /// <returns></returns>
        [Column("HTGRADE")]
        public string HtGrade { get; set; }
        /// <summary>
        /// 隐患类别
        /// </summary>
        /// <returns></returns>
        [Column("HTTYPE")]
        public string HtType { get; set; }
        /// <summary>
        /// 隐患整改措施
        /// </summary>
        /// <returns></returns>
        [Column("HTMEASURES")]
        public string HTMeasures { get; set; }
        /// <summary>
        /// 风险管控措施
        /// </summary>
        /// <returns></returns>
        [Column("MEASURE")]
        public string Measure { get; set; }
        /// <summary>
        /// 故障类型
        /// </summary>
        [Column("FAULTTYPE")]
        public string FaultType { get; set; }
        /// <summary>
        /// 管控层级
        /// </summary>
        [Column("LEVELNAME")]
        public string LevelName { get; set; }
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
            this.Status = 0;
            this.DeleteMark = 0;
            this.EnabledMark = 0;
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
