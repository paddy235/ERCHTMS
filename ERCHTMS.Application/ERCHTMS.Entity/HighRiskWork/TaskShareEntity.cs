using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：任务分配表
    /// </summary>
    [Table("BIS_TASKSHARE")]
    public class TaskShareEntity : BaseEntity
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
        /// 工程name
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTNAME")]
        public string ProjectName { get; set; }
        /// <summary>
        /// 作业单位name
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// 工程开工时间
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTWORKTIME")]
        public DateTime? ProjectWorkTime { get; set; }
        /// <summary>
        /// 部门旁站结束时间
        /// </summary>
        /// <returns></returns>
        [Column("DEPTENDTIME")]
        public DateTime? DeptEndTime { get; set; }
        /// <summary>
        /// 旁站监督部门name
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISEDEPTNAME")]
        public string SuperviseDeptName { get; set; }
        /// <summary>
        /// 作业单位id
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }

        /// <summary>
        /// 部门旁站开始时间
        /// </summary>
        /// <returns></returns>
        [Column("DEPTSTARTTIME")]
        public DateTime? DeptStartTime { get; set; }
        /// <summary>
        /// 作业单位code
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// 旁站监督部门code
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISEDEPTCODE")]
        public string SuperviseDeptCode { get; set; }
        /// <summary>
        /// 工程id
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTID")]
        public string ProjectId { get; set; }
        /// <summary>
        /// 任务类型(0:部门旁站任务 1:班组旁站任务 2:人员旁站任务)
        /// </summary>
        /// <returns></returns>
        [Column("TASKTYPE")]
        public string TaskType { get; set; }
        /// <summary>
        /// 旁站监督部门id
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISEDEPTID")]
        public string SuperviseDeptId { get; set; }
        /// <summary>
        /// 作业单位类别(0:单位内部 1:外包单位)
        /// </summary>
        /// <returns></returns>
        [Column("DEPTTYPE")]
        public string DeptType { get; set; }

        /// <summary>
        /// 流程步骤(0:厂级部门分配中 1:部门分配中 2:班组分配中,3:已完成分配)
        /// </summary>
        /// <returns></returns>
        [Column("FLOWSTEP")]
        public string FlowStep { get; set; }

        /// <summary>
        /// 是否提交(0:未提交 1:已提交)
        /// </summary>
        /// <returns></returns>
        [Column("ISSUBMIT")]
        public string IsSubmit { get; set; }

        /// <summary>
        /// 流程角色名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLENAME")]
        public string FlowRoleName { get; set; }

        /// <summary>
        /// 流程部门编码/ID 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPT")]
        public string FlowDept { get; set; }

        /// <summary>
        /// 流程部门名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPTNAME")]
        public string FlowDeptName { get; set; }

        /// <summary>
        /// 作业信息
        /// </summary>
        public List<SuperviseWorkInfoEntity> WorkSpecs { get; set; }

        /// <summary>
        /// 班组任务分配信息
        /// </summary>
        public List<TeamsInfoEntity> TeamSpec { get; set; }

        /// <summary>
        /// 人员任务分配信息
        /// </summary>
        public List<StaffInfoEntity> StaffSpec { get; set; }


        /// <summary>
        /// 删除人员ids
        /// </summary>
        public string DelIds { get; set; }

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