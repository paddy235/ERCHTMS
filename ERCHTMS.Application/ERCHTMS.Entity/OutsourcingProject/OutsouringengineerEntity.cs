using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包工程信息表
    /// </summary>
    [Table("EPG_OUTSOURINGENGINEER")]
    public class OutsouringengineerEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 责任部门
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETDEPT")]
        public string ENGINEERLETDEPT { get; set; }
        /// <summary>
        /// 用工部门
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERUSEDEPT")]
        public string ENGINEERUSEDEPT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETPEOPLEID")]
        public string ENGINEERLETPEOPLEID { get; set; }
        /// <summary>
        /// 用工部门Id
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERUSEDEPTID")]
        public string ENGINEERUSEDEPTID { get; set; }
        /// <summary>
        /// 安全管理人数
        /// </summary>
        /// <returns></returns>
        [Column("SAFEMANAGERPEOPLE")]
        public int? SAFEMANAGERPEOPLE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 工程编码
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERCODE")]
        public string ENGINEERCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 工程状态
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERSTATE")]
        public string ENGINEERSTATE { get; set; }
        /// <summary>
        /// 外包单位工程负责人
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERDIRECTOR")]
        public string ENGINEERDIRECTOR { get; set; }
        /// <summary>
        /// 外包单位工程负责人Id
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERDIRECTORID")]
        public string ENGINEERDIRECTORID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 计划完成时间
        /// </summary>
        /// <returns></returns>
        [Column("ACTUALSTARTDATE")]
        public DateTime? ACTUALSTARTDATE { get; set; }
        /// <summary>
        /// 工程作业人数
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERWORKPEOPLE")]
        public int? ENGINEERWORKPEOPLE { get; set; }
        /// <summary>
        /// 预计工期
        /// </summary>
        /// <returns></returns>
        [Column("PREDICTTIME")]
        public string PREDICTTIME { get; set; }
        /// <summary>
        /// 责任部门负责人电话
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETPEOPLEPHONE")]
        public string ENGINEERLETPEOPLEPHONE { get; set; }
        /// <summary>
        /// 实际工期 
        /// </summary>
        /// <returns></returns>
        [Column("PRACTICALTIME")]
        public string PRACTICALTIME { get; set; }
        /// <summary>
        /// 工程负责人电话
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERDIRECTORPHONE")]
        public string ENGINEERDIRECTORPHONE { get; set; }
        /// <summary>
        /// 用工部门负责人电话
        /// </summary>
        /// <returns></returns>
        [Column("USEDEPTPEOPPHONE")]
        public string USEDEPTPEOPPHONE { get; set; }
        /// <summary>
        /// 外包工程名称
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERNAME")]
        public string ENGINEERNAME { get; set; }
        /// <summary>
        /// 外包单位Id
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTID")]
        public string OUTPROJECTID { get; set; }
        /// <summary>
        /// 外包单位编码
        /// </summary>
        [NotMapped]
        public string OUTPROJECTCODE { get; set; }
        /// <summary>
        /// 外包工程名称
        /// </summary>
        [NotMapped]
        public string OUTPROJECTNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 用工部门负责人Id
        /// </summary>
        /// <returns></returns>
        [Column("USEDEPTPEOPLEID")]
        public string USEDEPTPEOPLEID { get; set; }
        /// <summary>
        /// 实际开工时间
        /// </summary>
        /// <returns></returns>
        [Column("PLANENDDATE")]
        public DateTime? PLANENDDATE { get; set; }
        /// <summary>
        /// 工程规模
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERSCALE")]
        public string ENGINEERSCALE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 工程技术人数
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERTECHPERSON")]
        public int? ENGINEERTECHPERSON { get; set; }
        /// <summary>
        /// 工程类型
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERTYPE")]
        public string ENGINEERTYPE { get; set; }
        public string ENGINEERTYPENAME { get; set; }
        /// <summary>
        /// 发包部门负责人
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETPEOPLE")]
        public string ENGINEERLETPEOPLE { get; set; }
        /// <summary>
        /// 工程风险等级
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLEVEL")]
        public string ENGINEERLEVEL { get; set; }
        public string ENGINEERLEVELNAME { get; set; }
        /// <summary>
        /// 安全保证金
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERCASHDEPOSIT")]
        public string ENGINEERCASHDEPOSIT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 实际完成时间
        /// </summary>
        /// <returns></returns>
        [Column("ACTUALENDDATE")]
        public DateTime? ACTUALENDDATE { get; set; }
        /// <summary>
        /// 所属区域
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERAREA")]
        public string ENGINEERAREA { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        [Column("ENGINEERAREANAME")]
        public string EngAreaName { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string ENGINEERAREANAME { get; set; }
        /// <summary>
        /// 用工部门负责人
        /// </summary>
        /// <returns></returns>
        [Column("USEDEPTPEOPLE")]
        public string USEDEPTPEOPLE { get; set; }
        /// <summary>
        /// 工程内容
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERCONTENT")]
        public string ENGINEERCONTENT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETDEPTID")]
        public string ENGINEERLETDEPTID { get; set; }
        /// <summary>
        /// 计划开工时间
        /// </summary>
        /// <returns></returns>
        [Column("PLANSTARTDATE")]
        public DateTime? PLANSTARTDATE { get; set; }

        /// <summary>
        /// 停复工状态 0 复工1：停工 
        /// </summary>
        /// <returns></returns>
        [Column("STOPRETURNSTATE")]
        public string STOPRETURNSTATE { get; set; }
        /// <summary>
        /// 是否部门入口新增：0 是 1 不是
        /// </summary>
        /// <returns></returns>
        [Column("ISDEPTADD")]
        public int IsDeptAdd { get; set; }
        /// <summary>
        /// 外包单位现场负责人
        /// </summary>
        /// <returns></returns>
        [Column("UNITSUPER")]
        public string UnitSuper { get; set; }
        /// <summary>
        /// 外包单位现场负责人Id
        /// </summary>
        /// <returns></returns>
        [Column("UNITSUPERID")]
        public string UnitSuperId { get; set; }
        /// <summary>
        /// 外包单位现场负责人电话
        /// </summary>
        /// <returns></returns>
        [Column("UNITSUPERPHONE")]
        public string UnitSuperPhone { get; set; }

        /// <summary>
        /// 监理单位ID
        /// </summary>
        [Column("SUPERVISORID")]
        public string SupervisorId { get; set; }

        /// <summary>
        /// 监理单位名称
        /// </summary>
        [Column("SUPERVISORNAME")]
        public string SupervisorName { get; set; }

        /// <summary>
        /// 外包单位类型
        /// </summary>
        [Column("DEPTTYPE")]
        public string DeptType { get; set; }

        /// <summary>
        /// 项目经理
        /// </summary>
        [Column("PROJECTMANAGER")]
        public string ProjectManager { get; set; }

        /// <summary>
        /// 项目经理电话
        /// </summary>
        [Column("PROJECTMANAGERTEL")]
        public string ProjectManagerTel { get; set; }

        /// <summary>
        /// 安全负责人
        /// </summary>
        [Column("SAFETYMODERATOR")]
        public string SafetyModerator { get; set; }

        /// <summary>
        /// 安全负责人电话
        /// </summary>
        [Column("SAFETYMODERATORTEL")]
        public string SafetyModeratorTel { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
            this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}