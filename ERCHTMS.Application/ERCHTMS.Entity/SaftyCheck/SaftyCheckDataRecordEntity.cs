using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ERCHTMS.Entity.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查记录
    /// </summary>
    [Table("BIS_SAFTYCHECKDATARECORD")]
    public class SaftyCheckDataRecordEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 检查名称
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATARECORDNAME")]
        public string CheckDataRecordName { get; set; }
        /// <summary>
        /// 检查类型
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATATYPE")]
        public int? CheckDataType { get; set; }
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
        /// 修改日期
        /// </summary>		
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>		
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>		
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 要求检查开始时间
        /// </summary>
        /// <returns></returns>
        [Column("CHECKBEGINTIME")]
        public DateTime? CheckBeginTime { get; set; }
        /// <summary>
        /// 要求检查结束时间
        /// </summary>
        /// <returns></returns>
        [Column("CHECKENDTIME")]
        public DateTime? CheckEndTime { get; set; }

        /// <summary>
        /// 检查开始时间
        /// </summary>
        /// <returns></returns>
        [Column("STARTDATE")]
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 检查结束时间
        /// </summary>
        /// <returns></returns>
        [Column("ENDDATE")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 检查级别
        /// </summary>
        /// <returns></returns>
        [Column("CHECKLEVEL")]
        public string CheckLevel { get; set; }

        /// <summary>
        /// 上级检查级别
        /// </summary>
        /// <returns></returns>
        [Column("SJCHECKLEVEL")]
        public string SJCheckLevel { get; set; }
        /// <summary>
        /// 检查组成员账号（多个逗号分隔）
        /// </summary>
        /// <returns></returns>
        [Column("CHECKLEVELID")]
        public string CheckLevelID { get; set; }
        /// <summary>
        /// 检查人
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMAN")]
        public string CheckMan { get; set; }
        /// <summary>
        /// 检查人主键
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMANID")]
        public string CheckManID { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTID")]
        public string BelongDeptID { get; set; }
        /// <summary>
        /// 所属部门主键
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPT")]
        public string BelongDept { get; set; }
        /// <summary>
        /// 检查部门主键
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTID")]
        public string CheckDeptID { get; set; }
        /// <summary>
        /// 检查部门
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPT")]
        public string CheckDept { get; set; }
        /// <summary>
        /// 被检查单位主键
        /// </summary>
        /// <returns></returns>
        [Column("CHECKEDDEPARTID")]
        public string CheckedDepartID { get; set; }
        /// <summary>
        /// 被检查单位
        /// </summary>
        /// <returns></returns>
        [Column("CHECKEDDEPART")]
        public string CheckedDepart { get; set; }
        /// <summary>
        /// 是否同步电厂可见（0：否，1：是）
        /// </summary>
        [Column("ISSYNVIEW")]
        public string IsSynView { get; set; }
        /// <summary>
        /// 含有风险点个数
        /// </summary>
        /// <returns></returns>
        [Column("COUNT")]
        public int Count { get; set; }
        /// <summary>
        /// 违章数量
        /// </summary>
        [NotMapped]
        public int? WzCount { get; set; }
        /// <summary>
        /// 问题数量
        /// </summary>
        [NotMapped]
        public int? WtCount { get; set; }

        /// <summary>
        /// 含有风险点个数
        /// </summary>
        /// <returns></returns>

        [NotMapped]
        [DefaultValue(0)]
        public decimal? Count1 { get; set; }
        /// <summary>
        /// 违章数量
        /// </summary>
        [NotMapped]
        [DefaultValue(0)]
        public decimal? WzCount1 { get; set; }
        /// <summary>
        /// 问题数量
        /// </summary>
        [NotMapped]
        [DefaultValue(0)]
        public decimal? WtCount1 { get; set; }

        /// <summary>
        /// 已检查风险点个数
        /// </summary>
        /// <returns></returns>
        [Column("SOLVECOUNT")]
        public double SolveCount { get; set; }
        /// <summary>
        /// 检查组长
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMANAGEMAN")]
        public string CheckManageMan { get; set; }

        /// <summary>
        /// 检查组长ID
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMANAGEMANID")]
        public string CheckManageManID { get; set; }

        /// <summary>
        /// 检查人所在部门code或者检查部门code
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTCODE")]
        public string CheckDeptCode { get; set; }

        /// <summary>
        /// 已检察人员
        /// </summary>
        /// <returns></returns>
        [Column("SOLVEPERSON")]
        public string SolvePerson { get; set; }
        /// <summary>
        /// 检查成员
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERS")]
        public string CheckUsers { get; set; }
        /// <summary>
        /// 检查成员Id
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERIDS")]
        public string CheckUserIds { get; set; }

        /// <summary>
        /// 已检察人员姓名
        /// </summary>
        /// <returns></returns>
        [Column("SOLVEPERSONNAME")]
        public string SolvePersonName { get; set; }

        /// <summary>
        ///是否省公司下发检查任务
        /// </summary>
        /// <returns></returns>
        [Column("DATATYPE")]
        public int? DataType { get; set; }

        /// <summary>
        ///任务接收人（多个用逗号分隔）
        /// </summary>
        /// <returns></returns>
        [Column("RECEIVEUSERS")]
        public string ReceiveUsers { get; set; }
        /// <summary>
        ///任务接收人Id（多个用逗号分隔）
        /// </summary>
        /// <returns></returns>
        [Column("RECEIVEUSERIDS")]
        public string ReceiveUserIds { get; set; }

        /// <summary>
        ///检查负责人
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSER")]
        public string DutyUser { get; set; }
        /// <summary>
        ///检查负责人Id
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        ///负责人单位
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPT")]
        public string DutyDept { get; set; }


        /// <summary>
        ///任务分配人
        /// </summary>
        /// <returns></returns>
        [Column("ALLOTUSER")]
        public string AllotUser { get; set; }
        /// <summary>
        ///任务分配人单位
        /// </summary>
        /// <returns></returns>
        [Column("ALLOTDEPT")]
        public string AllotDept { get; set; }
        /// <summary>
        ///任务分配时间
        /// </summary>
        /// <returns></returns>
        [Column("ALLOTTIME")]
        public DateTime? AllotTime { get; set; }

        /// <summary>
        ///检查要求
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        ///状态（0：待分配，1：待完善，2：已完善）
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        [DefaultValue(0)]
        public int Status { get; set; }
        /// <summary>
        ///是否提交（0：否，1：是）
        /// </summary>
        /// <returns></returns>
        [Column("ISSUBMIT")]
        [DefaultValue(0)]
        public int IsSubmit { get; set; }

        /// <summary>
        ///关联检查主记录Id(本表)
        /// </summary>
        /// <returns></returns>
        [Column("RID")]
        public string RId { get; set; }
        /// <summary>
        ///是否周期性计划（0：否，1：是）
        /// </summary>
        /// <returns></returns>
        [Column("ISAUTO")]
        public int? IsAuto { get; set; }
        /// <summary>
        ///周期类型（0：按天，1：按周，2：按月）
        /// </summary>
        /// <returns></returns>
        [Column("AUTOTYPE")]
        public int? AutoType { get; set; }
        /// <summary>
        ///是否跳过双休（0：否，1：是）
        /// </summary>
        /// <returns></returns>
        [Column("ISSKIP")]
        public int? IsSkip { get; set; }
        /// <summary>
        ///周期显示
        /// </summary>
        /// <returns></returns>
        [Column("DISPLAY")]
        public string Display { get; set; }
        /// <summary>
        ///星期（多个用英文逗号分隔）
        /// </summary>
        /// <returns></returns>
        [Column("WEEKS")]
        public string Weeks { get; set; }
        /// <summary>
        ///日期或星期（0：按日期，1：按星期）
        /// </summary>
        /// <returns></returns>
        [Column("SELTYPE")]
        public int? SelType { get; set; }
        /// <summary>
        ///第几周（多个用英文逗号分隔）
        /// </summary>
        /// <returns></returns>
        [Column("THWEEKS")]
        public string ThWeeks { get; set; }
        /// <summary>
        ///日期（多个用英文逗号分隔）
        /// </summary>
        /// <returns></returns>
        [Column("DAYS")]
        public string Days { get; set; }
        /// <summary>
        ///月份（多个用英文逗号分隔）
        /// </summary>
        /// <returns></returns>
        [Column("MONTHS")]
        public string Months { get; set; }
        /// <summary>
        ///周期
        /// </summary>
        /// <returns></returns>
        [Column("ROUNDS")]
        public string Rounds { get; set; }
        /// <summary>
        ///是否结束周期性计划（1：中止，0：执行）
        /// </summary>
        /// <returns></returns>
        [Column("ISOVER")]
        public int? IsOver { get; set; }

        /// <summary>
        ///检查区域
        /// </summary>
        /// <returns></returns>
        [Column("AREANAME")]
        public string AreaName { get; set; }
        [Required]
        [DisplayName("检查目的")]
        [MaxLength(10)]
        /// <summary>
        ///检查目的
        /// </summary>
        /// <returns></returns>
        [Column("AIM")]
        public string Aim { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ?Guid.NewGuid().ToString(): ID;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
           this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
           this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
           this.DataType = DataType != 1 ? 0 : DataType;
           this.IsOver = IsOver != 1 ? 0 : IsOver;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        public void insertInto(SaftyCheckDataRecordEntity se, Operator user)
        {
            se.ID = Guid.NewGuid().ToString();
            se.CreateDate = DateTime.Now;
            se.CreateUserId = user.UserId;
            se.CreateUserName = user.UserName;
            se.CreateUserDeptCode = user.DeptCode;
            se.CreateUserOrgCode = user.OrganizeCode;
        }
        #endregion
    }
}