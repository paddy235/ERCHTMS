using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.Observerecord
{
    /// <summary>
    /// 描 述：观察记录表
    /// </summary>
    [Table("BIS_OBSERVERECORD")]
    public class ObserverecordEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 作业区域Id
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREAID")]
        public string WorkAreaId { get; set; }
        /// <summary>
        /// 作业地点
        /// </summary>
        /// <returns></returns>
        [Column("WORKPLACE")]
        public string WorkPlace { get; set; }
        /// <summary>
        /// 观察结束时间
        /// </summary>
        /// <returns></returns>
        [Column("OBSENDTIME")]
        public DateTime? ObsEndTime { get; set; }
        /// <summary>
        /// 观察起始时间
        /// </summary>
        /// <returns></returns>
        [Column("OBSSTARTTIME")]
        public DateTime? ObsStartTime { get; set; }
        /// <summary>
        /// 观察人员
        /// </summary>
        /// <returns></returns>
        [Column("OBSPERSON")]
        public string ObsPerson { get; set; }
        /// <summary>
        /// 观察人员Id
        /// </summary>
        /// <returns></returns>
        [Column("OBSPERSONID")]
        public string ObsPersonId { get; set; }
        /// <summary>
        /// 观察依据类型
        /// </summary>
        /// <returns></returns>
        [Column("OBSGIST")]
        public string ObsGist { get; set; }
        /// <summary>
        /// 观察依据
        /// </summary>
        /// <returns></returns>
        [Column("OBSGISTVALUE")]
        public string ObsGistValue { get; set; }
        /// <summary>
        /// 观察计划Id
        /// </summary>
        /// <returns></returns>
        [Column("OBSPLANID")]
        public string ObsPlanId { get; set; }
        /// <summary>
        /// 沟通时间
        /// </summary>
        /// <returns></returns>
        [Column("LINKUPTIME")]
        public DateTime? LinkUpTime { get; set; }
        /// <summary>
        /// 沟通地点
        /// </summary>
        /// <returns></returns>
        [Column("LINKUPPLACE")]
        public string LinkUpPlace { get; set; }
        /// <summary>
        /// 参加人员
        /// </summary>
        /// <returns></returns>
        [Column("LINKPEOPLE")]
        public string LinkPeople { get; set; }
        /// <summary>
        /// 参加人员Id
        /// </summary>
        /// <returns></returns>
        [Column("LINKPEOPLEID")]
        public string LinkPeopleId { get; set; }
        /// <summary>
        /// 沟通事项
        /// </summary>
        /// <returns></returns>
        [Column("LINKUPCONTENT")]
        public string LinkUpContent { get; set; }
        /// <summary>
        /// 沟通结果
        /// </summary>
        /// <returns></returns>
        [Column("LINKUPRESULT")]
        public string LinkUpResult { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("LINKUPREMARK")]
        public string LinkUpRemark { get; set; }
        /// <summary>
        /// 验收结论
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTRESULT")]
        public string AcceptResult { get; set; }
        /// <summary>
        /// 验收人员
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTPERSON")]
        public string AcceptPerson { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTPERSONID")]
        public string AcceptPersonId { get; set; }
        /// <summary>
        /// 验收时间
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTTIME")]
        public DateTime? AcceptTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("RECORDREMARK")]
        public string RecordRemark { get; set; }
        /// <summary>
        /// 作业区域
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREA")]
        public string WorkArea { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 登记部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPT")]
        public string CreateUserDept { get; set; }
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
        /// 作业名称
        /// </summary>
        /// <returns></returns>
        [Column("WORKNAME")]
        public string WorkName { get; set; }
        /// <summary>
        /// 作业部门
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNIT")]
        public string WorkUnit { get; set; }
        /// <summary>
        /// 作业部门Id
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNITID")]
        public string WorkUnitId { get; set; }
        /// <summary>
        /// 作业部门Code
        /// </summary>
        /// <returns></returns>
         [Column("WORKUNITCODE")]
        public string WorkUnitCode { get; set; }
        /// <summary>
        /// 作业人员
        /// </summary>
        /// <returns></returns>
        [Column("WORKPEOPLE")]
        public string WorkPeople { get; set; }
        /// <summary>
        /// 作业人员Id
        /// </summary>
        /// <returns></returns>
        [Column("WORKPEOPLEID")]
        public string WorkPeopleId { get; set; }
         [Column("ISCOMMIT")]
        public int IsCommit { get; set; }
        /// <summary>
        /// 观察计划任务分解Id
        /// </summary>
           [Column("OBSPLANFJID")]
         public string ObsPlanfjid { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
           this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
           this.CreateUserDept = OperatorProvider.Provider.Current().DeptName;
           this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
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
        #endregion
    }
}