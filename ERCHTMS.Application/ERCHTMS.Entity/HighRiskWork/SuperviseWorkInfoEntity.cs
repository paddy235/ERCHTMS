using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：旁站监督作业信息
    /// </summary>
    [Table("BIS_SUPERVISEWORKINFO")]
    public class SuperviseWorkInfoEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 作业内容
        /// </summary>
        /// <returns></returns>
        [Column("WORKCONTENT")]
        public string WorkContent { get; set; }
        /// <summary>
        /// 作业单位
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTID")]
        public string WorkDeptId { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 工程
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGID")]
        public string EngineeringId { get; set; }
        /// <summary>
        /// 作业地点
        /// </summary>
        /// <returns></returns>
        [Column("WORKPLACE")]
        public string WorkPlace { get; set; }
        /// <summary>
        /// 作业单位
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTNAME")]
        public string WorkDeptName { get; set; }
        /// <summary>
        /// 作业类别
        /// </summary>
        /// <returns></returns>
        [Column("WORKINFOTYPE")]
        public string WorkInfoType { get; set; }
        /// <summary>
        /// 作业类别id
        /// </summary>
        /// <returns></returns>
        [Column("WORKINFOTYPEID")]
        public string WorkInfoTypeId { get; set; }
        /// <summary>
        /// 作业单位
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTCODE")]
        public string WorkDeptCode { get; set; }
        /// <summary>
        /// 作业单位类别(0:单位内部 1:外包单位)
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTTYPE")]
        public string WorkDeptType { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 监督任务id
        /// </summary>
        /// <returns></returns>
        [Column("TASKSHAREID")]
        public string TaskShareId { get; set; }
        /// <summary>
        /// 工程名称
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGNAME")]
        public string EngineeringName { get; set; }
        /// <summary>
        /// 作业名称
        /// </summary>
        /// <returns></returns>
        [Column("WORKNAME")]
        public string WorkName { get; set; }
        /// <summary>
        /// 作业开始时间
        /// </summary>
        /// <returns></returns>
        [Column("WORKSTARTTIME")]
        public DateTime? WorkStartTime { get; set; }
        /// <summary>
        /// 作业结束时间
        /// </summary>
        /// <returns></returns>
        [Column("WORKENDTIME")]
        public DateTime? WorkEndTime { get; set; }
        /// <summary>
        /// 作业区域
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREANAME")]
        public string WorkAreaName { get; set; }
        /// <summary>
        /// 作业区域code
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREACODE")]
        public string WorkAreaCode { get; set; }
        /// <summary>
        /// 工作票号
        /// </summary>
        /// <returns></returns>
        [Column("WORKTICKETNO")]
        public string WorkTicketNo { get; set; }

        /// <summary>
        /// 作业人员
        /// </summary>
        /// <returns></returns>
        [Column("WORKUSERIDS")]
        public string WorkUserIds { get; set; }

        /// <summary>
        /// 作业人员
        /// </summary>
        /// <returns></returns>
        [Column("WORKUSERNAMES")]
        public string WorkUserNames { get; set; }
        
        /// <summary>
        /// 手动输入的作业类别
        /// </summary>
        /// <returns></returns>
        [Column("HANDTYPE")]
        public string HandType { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        /// <returns></returns>
        [Column("WORKPROJECTNAME")]
        public string WorkProjectName { get; set; }

        
        #region 因旁站监督员可改作业类别
        /// <summary>
        /// 作业类别
        /// </summary>
        /// <returns></returns>
        [Column("WORKINFOTYPE1")]
        public string WorkInfoType1 { get; set; }
        /// <summary>
        /// 作业类别id
        /// </summary>
        /// <returns></returns>
        [Column("WORKINFOTYPEID1")]
        public string WorkInfoTypeId1 { get; set; }

        /// <summary>
        /// 手动输入的作业类别
        /// </summary>
        /// <returns></returns>
        [Column("HANDTYPE1")]
        public string HandType1 { get; set; }
        #endregion

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            //this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            //this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            //this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            //this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
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