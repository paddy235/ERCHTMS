using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.DangerousJob
{
    /// <summary>
    /// 描 述：危险作业流程表
    /// </summary>
    [Table("BIS_DANGEROUSJOBFLOW")]
    public class DangerousJobFlowEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? Autoid { get; set; }
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
        /// 逐级审核模块编号
        /// </summary>
        /// <returns></returns>
        [Column("MODULENO")]
        public string ModuleNo { get; set; }
        /// <summary>
        /// 基础节点id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWNAME")]
        public string FlowName { get; set; }
        /// <summary>
        /// 流程步骤
        /// </summary>
        /// <returns></returns>
        [Column("FLOWSTEP")]
        public int? FlowStep { get; set; }
        /// <summary>
        /// 关联业务数据id
        /// </summary>
        /// <returns></returns>
        [Column("BUSINESSID")]
        public string BusinessId { get; set; }
        /// <summary>
        /// 当前步骤处理标示(0部门加角色，1执行脚本获取业务某个字段，2指定审核人，3业务选择审核人)
        /// </summary>
        /// <returns></returns>
        [Column("PROCESSORFLAG")]
        public string ProcessorFlag { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }
        /// <summary>
        /// 部门code
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        /// <returns></returns>
        [Column("ROLENAME")]
        public string RoleName { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        /// <returns></returns>
        [Column("ROLEID")]
        public string RoleId { get; set; }
        /// <summary>
        /// 审核人id
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 审核人账号
        /// </summary>
        /// <returns></returns>
        [Column("USERACCOUNT")]
        public string UserAccount { get; set; }
        /// <summary>
        /// 审核人名称
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
        /// <summary>
        /// 选择人提示语
        /// </summary>
        /// <returns></returns>
        [Column("PRESENTATION")]
        public string Presentation { get; set; }
       
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
