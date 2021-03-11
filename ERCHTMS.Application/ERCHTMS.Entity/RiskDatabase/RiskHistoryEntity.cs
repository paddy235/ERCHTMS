using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ERCHTMS.Entity.RiskDatabase
{
    /// <summary>
    /// 描 述：企业风险辨识评估记录
    /// </summary>
    public class RiskHistoryEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 内置区域名称
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
        public string DeptCode { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }
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
        /// 区域ID
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTID")]
        public string DistrictId { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTNAME")]
        public string DistrictName { get; set; }

        /// <summary>
        /// 内置区域ID
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
        [DefaultValue(0)]
        public int? DeleteMark { get; set; }
        /// <summary>
        /// 是否生效
        /// </summary>
        /// <returns></returns>
        [Column("ENABLEDMARK")]
        [DefaultValue(0)]
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
        /// 修改标记，0：默认值，1：修改
        /// </summary>
        /// <returns></returns>
        [Column("STATE")]
        public int? State { get; set; }
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
        /// 关联计划ID
        /// </summary>
        /// <returns></returns>
        [Column("PLANID")]
        public string PlanId { get; set; }
        /// <summary>
        /// 故障类型
        /// </summary>
        /// <returns></returns>
        [Column("FAULTTYPE")]
        public string FaultType { get; set; }
        /// <summary>
        /// 管控层级
        /// </summary>
        /// <returns></returns>
        [Column("LEVELNAME")]
        public string LevelName { get; set; }
        /// <summary>
        /// 历史记录ID
        /// </summary>
        /// <returns></returns>
        [Column("HISTORYID")]
        public string HistoryId { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        [Column("JOBNAME")]
        public string JobName { get; set; }
        /// <summary>
        /// 工器具/危化品
        /// </summary>
        [Column("TOOLORDANGER")]
        public string ToolOrDanger { get; set; }
        /// <summary>
        /// 危险源类别
        /// </summary>
        [Column("DANGERSOURCETYPE")]
        public string DangerSourceType { get; set; }
        /// <summary>
        /// 系统
        /// </summary>
        [Column("HJSYSTEM")]
        public string HjSystem { get; set; }
        /// <summary>
        /// 设备
        /// </summary>
        [Column("HJEQUPMENT")]
        public string HjEqupment { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        [Column("PROJECT")]
        public string Project { get; set; }
        /// <summary>
        /// 管控责任人
        /// </summary>
        [Column("DUTYPERSON")]
        public string DutyPerson { get; set; }
        /// <summary>
        /// 管控责任人Id
        /// </summary>
        [Column("DUTYPERSONID")]
        public string DutyPersonId { get; set; }
        /// <summary>
        /// 单元
        /// </summary>
        [Column("ELEMENT")]
        public string Element { get; set; }
        /// <summary>
        /// 故障类别
        /// </summary>
        [Column("FAULTCATEGORY")]
        public string FaultCategory { get; set; }
        /// <summary>
        /// 故障类别
        /// </summary>
        [Column("MAJORNAMETYPE")]
        public string MajorNameType { get; set; }

        /// <summary>
        /// 存储容积
        /// </summary>
        [Column("STORAGESPACE")]
        public string StorageSpace { get; set; }
        /// <summary>
        /// 包装单位
        /// </summary>
        [Column("PACKUNTIL")]
        public string PackUntil { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Column("PACKNUM")]
        public int? PackNum { get; set; }
        /// <summary>
        ///部门
        /// </summary>
        [Column("POSTDEPT")]
        public string PostDept { get; set; }
        /// <summary>
        ///部门Code
        /// </summary>
        [Column("POSTDEPTCODE")]
        public string PostDeptCode { get; set; }
        /// <summary>
        /// 部门Id
        /// </summary>
        [Column("POSTDEPTID")]
        public string PostDeptId { get; set; }
        /// <summary>
        /// 人员
        /// </summary>
        [Column("POSTPERSON")]
        public string PostPerson { get; set; }
        /// <summary>
        /// 人员
        /// </summary>
        [Column("POSTPERSONID")]
        public string PostPersonId { get; set; }

        /// <summary>
        /// 作业活动名称/设备名称
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// 作业活动清单/设备设施清单id
        /// </summary>
        [Column("LISTINGID")]
        public string ListingId { get; set; }
        /// <summary>
        /// 危害名称
        /// </summary>
        [Column("HARMNAME")]
        public string HarmName { get; set; }
        /// <summary>
        /// 危害种类
        /// </summary>
        [Column("HAZARDTYPE")]
        public string HazardType { get; set; }
        /// <summary>
        /// 危害及有关信息描述
        /// </summary>
        [Column("HARMDESCRIPTION")]
        public string HarmDescription { get; set; }
        /// <summary>
        /// 风险种类
        /// </summary>
        [Column("TYPESOFRISK")]
        public string TypesOfRisk { get; set; }
        /// <summary>
        /// 风险范畴
        /// </summary>
        [Column("RISKCATEGORY")]
        public string RiskCategory { get; set; }
        /// <summary>
        /// 暴露于风险的人员、设备信息
        /// </summary>
        [Column("EXPOSEDRISK")]
        public string ExposedRisk { get; set; }
        /// <summary>
        /// 现有的控制措施
        /// </summary>
        [Column("EXISTINGMEASURES")]
        public string ExistingMeasures { get; set; }
        /// <summary>
        /// 是否特种设备
        /// </summary>
        [Column("ISSPECIALEQU")]
        public int? IsSpecialEqu { get; set; }
        /// <summary>
        /// 检查项目名称
        /// </summary>
        [Column("CHECKPROJECTNAME")]
        public string CheckProjectName { get; set; }
        /// <summary>
        /// 检查标准
        /// </summary>
        [Column("CHECKSTANDARD")]
        public string CheckStandard { get; set; }
        /// <summary>
        /// 不符合标准情况及后果
        /// </summary>
        [Column("CONSEQUENCES")]
        public string Consequences { get; set; }
        /// <summary>
        /// 建议采取的控制措施
        /// </summary>
        [Column("ADVICEMEASURES")]
        public string AdviceMeasures { get; set; }
        /// <summary>
        /// 控制措施的有效性
        /// </summary>
        [Column("EFFECTIVENESS")]
        public decimal? Effectiveness { get; set; }
        /// <summary>
        /// 措施的成本因素
        /// </summary>
        [Column("COSTFACTOR")]
        public decimal? CostFactor { get; set; }
        /// <summary>
        /// 控制措施判断后果
        /// </summary>
        [Column("MEASURESRESULT")]
        public string MeasuresResult { get; set; }
        /// <summary>
        /// 是否采纳  0：采纳  1：不采纳
        /// </summary>
        [Column("ISADOPT")]
        public int? IsAdopt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("MEASURESRESULTVAL")]
        public decimal? MeasuresResultVal { get; set; }

        /// <summary>
        /// 是否常规  0：常规 1：非常规
        /// </summary>
        [Column("ISCONVENTIONAL")]
        public int? IsConventional { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK")]
        public string Remark { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
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