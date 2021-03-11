using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：任务分配班组
    /// </summary>
    [Table("BIS_TEAMSINFO")]
    public class TeamsInfoEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 创建用户id
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
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
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
        /// 班组name
        /// </summary>
        /// <returns></returns>
        [Column("TEAMNAME")]
        public string TeamName { get; set; }
        /// <summary>
        /// 班组code
        /// </summary>
        /// <returns></returns>
        [Column("TEAMCODE")]
        public string TeamCode { get; set; }
        /// <summary>
        /// 班组id
        /// </summary>
        /// <returns></returns>
        [Column("TEAMID")]
        public string TeamId { get; set; }
        /// <summary>
        /// 旁站班组开始时间
        /// </summary>
        /// <returns></returns>
        [Column("TEAMSTARTTIME")]
        public DateTime? TeamStartTime { get; set; }
        /// <summary>
        /// 旁站班组结束时间
        /// </summary>
        /// <returns></returns>
        [Column("TEAMENDTIME")]
        public DateTime? TeamEndTime { get; set; }
        /// <summary>
        /// 任务作业信息作业名称
        /// </summary>
        /// <returns></returns>
        [Column("WORKINFONAME")]
        public string WorkInfoName { get; set; }
        /// <summary>
        /// 任务作业信息id
        /// </summary>
        /// <returns></returns>
        [Column("WORKINFOID")]
        public string WorkInfoId { get; set; }
        /// <summary>
        /// 任务分配id
        /// </summary>
        /// <returns></returns>
        [Column("TASKSHAREID")]
        public string TaskShareId { get; set; }

        /// <summary>
        /// 是否提交(0:否 1:是)
        /// </summary>
        /// <returns></returns>
        [Column("DATAISSUBMIT")]
        public string DataIsSubmit { get; set; }

        /// <summary>
        /// 是否任务完成(任务完成不需推送班组)
        /// </summary>
        /// <returns></returns>
        [Column("ISACCOMPLISH")]
        public string IsAccomplish { get; set; }
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