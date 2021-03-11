using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.AssessmentManage
{
    /// <summary>
    /// 描 述：自评计划
    /// </summary>
    [Table("BIS_ASSESSMENTPLAN")]
    public class AssessmentPlanEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 计划名称
        /// </summary>
        /// <returns></returns>
        [Column("PLANNAME")]
        public string PlanName { get; set; }
        /// <summary>
        /// 自评时间
        /// </summary>
        /// <returns></returns>
        [Column("SELFASSESSMENTDATE")]
        public DateTime? SelfAssessmentDate { get; set; }
        /// <summary>
        /// 自评组长
        /// </summary>
        /// <returns></returns>
        [Column("TEAMLEADER")]
        public string TeamLeader { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public string Status { get; set; }
        /// <summary>
        /// 组长填写的综述
        /// </summary>
        /// <returns></returns>
        [Column("LEADERSUM")]
        public string LeaderSum { get; set; }
        /// <summary>
        /// 是否锁定
        /// </summary>
        /// <returns></returns>
        [Column("ISLOCK")]
        public string IsLock { get; set; }
        /// <summary>
        /// 备用
        /// </summary>
        /// <returns></returns>
        [Column("RESERVE")]
        public string Reserve { get; set; }
        /// <summary>
        /// 自评组长名称
        /// </summary>
        /// <returns></returns>
        [Column("TEAMLEADERNAME")]
        public string TeamLeaderName { get; set; }
        /// <summary>
        /// 自评标准
        /// </summary>
        /// <returns></returns>
        [Column("ASSESSSTANDARD")]
        public string AssessStandard { get; set; }
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