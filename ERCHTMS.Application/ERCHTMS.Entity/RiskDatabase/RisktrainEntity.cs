using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.RiskDatabase
{
    /// <summary>
    /// 描 述：风险预知训练
    /// </summary>
    [Table("BIS_RISKTRAIN")]
    public class RisktrainEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 备用字段
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 状态(0：未完成，1:已完成)
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public int Status { get; set; }
        /// <summary>
        /// 作业人员Id,多个用逗号分隔
        /// </summary>
        /// <returns></returns>
        [Column("USERIDS")]
        public string UserIds { get; set; }
        /// <summary>
        /// 作业人员
        /// </summary>
        /// <returns></returns>
        [Column("WORKUSERS")]
        public string WorkUsers { get; set; }
        /// <summary>
        /// 作业任务描述
        /// </summary>
        /// <returns></returns>
        [Column("TASKCONTENT")]
        public string TaskContent { get; set; }
        /// <summary>
        /// 区域编码
        /// </summary>
        /// <returns></returns>
        [Column("AREACODE")]
        public string AreaCode { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        /// <returns></returns>
        [Column("AREANAME")]
        public string AreaName { get; set; }
        /// <summary>
        /// 区域Id
        /// </summary>
        /// <returns></returns>
        [Column("AREAID")]
        public string AreaId { get; set; }
        /// <summary>
        /// 工器具准备
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSPREPARE")]
        public string ToolsPrepare { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        /// <returns></returns>
        [Column("POSTNAME")]
        public string PostName { get; set; }
        /// <summary>
        /// 岗位Id
        /// </summary>
        /// <returns></returns>
        [Column("POSTID")]
        public string PostId { get; set; }
        /// <summary>
        /// 作业类别
        /// </summary>
        /// <returns></returns>
        [Column("WORKTYPE")]
        public string WorkType { get; set; }
        /// <summary>
        /// 工作任务
        /// </summary>
        /// <returns></returns>
        [Column("TASKNAME")]
        public string TaskName { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
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
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
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
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 创建用户所属单位名称
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTNAME")]
        public string CreateUserDeptName { get; set; }

        /// <summary>
        /// 作业负责人
        /// </summary>
        /// <returns></returns>
        [Column("WORKFZR")]
        public string WorkFzr { get; set; }
        /// <summary>
        /// 作业负责人Id
        /// </summary>
        /// <returns></returns>
        [Column("WORKFZRID")]
        public string WorkFzrId { get; set; }
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
        /// 作业单位
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNIT")]
        public string WorkUnit { get; set; }
        /// <summary>
        /// 作业单位
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNITID")]
        public string WorkUnitId { get; set; }
        /// <summary>
        /// 作业单位
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNITCODE")]
        public string WorkUnitCode { get; set; }
        /// <summary>
        /// 作业地点
        /// </summary>
        /// <returns></returns>
        [Column("WORKPLACE")]
        public string WorkPlace { get; set; }
        /// <summary>
        /// 工作票号
        /// </summary>
        /// <returns></returns>
        [Column("WORKNUM")]
        public string WorkNum { get; set; }
         /// <summary>
        /// 是否提交
        /// </summary>
        /// <returns></returns>
        [Column("ISCOMMIT")]
        public string IsCommit { get; set; }
            [Column("TRAINTYPE")]
        public string TrainType { get; set; }
             [Column("TRAINLIBWORKID")]
            public string TrainlibWorkId { get; set; }
        
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
            this.CreateUserDeptName = string.IsNullOrEmpty(OperatorProvider.Provider.Current().DeptName) ? OperatorProvider.Provider.Current().OrganizeName : OperatorProvider.Provider.Current().DeptName;
            this.UserIds = ","+UserIds.Trim(',')+",";
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
            this.UserIds = "," + UserIds.Trim(',') + ",";
        }
        #endregion
    }
}