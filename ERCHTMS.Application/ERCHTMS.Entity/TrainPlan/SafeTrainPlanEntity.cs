using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.TrainPlan
{
    [Table("BIS_SAFETRAINPLAN")]
   public class SafeTrainPlanEntity:BaseEntity
    {

        #region [基本信息]
        /// <summary>
        /// 主键
        /// </summary>
        [Column("ID")]
        public string Id { get; set; }

        /// <summary>
        /// 培训项目
        /// </summary>
        [Column("PROJECTNAME")]
        public string ProjectName { get; set; }

        /// <summary>
        /// 培训时间
        /// </summary>
        [Column("TRAINDATE")]
        public DateTime? TrainDate { get; set; }

        /// <summary>
        /// 培训内容
        /// </summary>
        [Column("TRAINCONTENT")]
        public string TrainContent { get; set; }

        /// <summary>
        /// 培训对象
        /// </summary>
        [Column("PARTICIPANTS")]
        public string Participants { get; set; }

        /// <summary>
        /// 责任部门id
        /// </summary>
        [Column("DEPARTMENTID")]
        public string DepartmentId { get; set; }

        /// <summary>
        /// 责任部门
        /// </summary>
        [Column("DEPARTMENTNAME")]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 责任部门code
        /// </summary>
        [Column("DEPARTMENTCODE")]
        public string DepartmentCode { get; set; }

        /// <summary>
        /// 责任人id
        /// </summary>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        [Column("DUTYUSERNAME")]
        public string DutyUserName { get; set; }

        /// <summary>
        /// 执行人/监督人id
        /// </summary>
        [Column("EXECUTEUSERID")]
        public string ExecuteUserId { get; set; }

        /// <summary>
        /// 执行人/监督人
        /// </summary>
        [Column("EXECUTEUSERNAME")]
        public string ExecuteUserName { get; set; }

        /// <summary>
        /// 状态(0:待下发 1:进行中 2:已完成)
        /// </summary>
        [Column("PROCESSSTATE")]
        public int? ProcessState { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建人部门code
        /// </summary>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }

        /// <summary>
        /// 创建人组织Code
        /// </summary>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }

        /// <summary>
        /// 修改人id
        /// </summary>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        #endregion
        #region [反馈信息]
        /// <summary>
        /// 实际完成时间
        /// </summary>
        [Column("FINISHDATE")]
        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// 培训效果评估
        /// </summary>
        [Column("EFFECTASSESS")]
        public string EffectAssess { get; set; }

        /// <summary>
        /// 反馈人id
        /// </summary>
        [Column("FEEDBACKUSERID")]
        public string FeedbackUserId { get; set; }

        /// <summary>
        /// 反馈人
        /// </summary>
        [Column("FEEDBACKUSERNAME")]
        public string FeedbackUserName { get; set; }

        /// <summary>
        /// 所属部门id
        /// </summary>
        [Column("FEEDBACKDEPTID")]
        public string FeedbackDeptId { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        [Column("FEEDBACKDEPTNAME")]
        public string FeedbackDeptName { get; set; }

        /// <summary>
        /// 反馈时间
        /// </summary>
        [Column("FEEDBACKTIME")]
        public DateTime? FeedbackTime { get; set; }

        /// <summary>
        /// 其他附件id
        /// </summary>
        [Column("OTHERFILESID")]
        public string OtherFilesId { get; set; }
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
