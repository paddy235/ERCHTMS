using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SafeReward
{
    /// <summary>
    /// 描 述：安全奖励
    /// </summary>
    [Table("BIS_SAFEREWARD")]
    public class SaferewardEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 专业负责人name
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYPRINCIPALNAME")]
        public string SpecialtyPrincipalName { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 专业负责人是否同意
        /// </summary>
        /// <returns></returns>
        [Column("ISAGREEPRINCIPAL")]
        public string IsAgreePrincipal { get; set; }
        /// <summary>
        /// 被奖励人员id
        /// </summary>
        /// <returns></returns>
        [Column("REWARDUSERID")]
        public string RewardUserId { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 部门负责人name
        /// </summary>
        /// <returns></returns>
        [Column("DEPTPRINCIPALNAME")]
        public string DeptPrincipalName { get; set; }
        /// <summary>
        /// 分管领导id
        /// </summary>
        /// <returns></returns>
        [Column("LEADERSHIPID")]
        public string LeaderShipId { get; set; }
        /// <summary>
        /// 申请状态(1.专业意见,2.部门意见,3.EHS部意见,4.分管领导,5.总经理)
        /// </summary>
        /// <returns></returns>
        [Column("APPLYSTATE")]
        public string ApplyState { get; set; }
        /// <summary>
        /// 总经理id
        /// </summary>
        /// <returns></returns>
        [Column("CEOID")]
        public string CeoId { get; set; }
        /// <summary>
        /// 流程状态(0正在处理.,1.已处理,2.未处理)
        /// </summary>
        /// <returns></returns>
        [Column("FLOWSTATE")]
        public string FlowState { get; set; }
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
        /// 总经理name
        /// </summary>
        /// <returns></returns>
        [Column("CEONAME")]
        public string CeoName { get; set; }
        /// <summary>
        /// 被奖励单位
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTCODE")]
        public string ApplyDeptCode { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 部门负责人是否同意
        /// </summary>
        /// <returns></returns>
        [Column("ISAGREEDEPT")]
        public string IsAgreeDept { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        [Column("SAFEREWARDCODE")]
        public string SafeRewardCode { get; set; }
        /// <summary>
        /// 被奖励单位
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTID")]
        public string ApplyDeptId { get; set; }
        /// <summary>
        /// 被奖励人员name
        /// </summary>
        /// <returns></returns>
        [Column("REWARDUSERNAME")]
        public string RewardUserName { get; set; }
        /// <summary>
        /// 被奖励单位
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTNAME")]
        public string ApplyDeptName { get; set; }
        /// <summary>
        /// 总经理是否同意
        /// </summary>
        /// <returns></returns>
        [Column("ISAGREECEO")]
        public string IsAgreeCeo { get; set; }
        /// <summary>
        /// 分管领导意见
        /// </summary>
        /// <returns></returns>
        [Column("LEADERSHIPOPINION")]
        public string LeaderShipOpinion { get; set; }
        /// <summary>
        /// 分管领导是否同意
        /// </summary>
        /// <returns></returns>
        [Column("ISAGREELEADERSHIPID")]
        public string IsAgreeLeaderShipId { get; set; }
        /// <summary>
        /// 申请人name
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERNAME")]
        public string ApplyUserName { get; set; }

        /// <summary>
        /// 申请人部门id
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERDEPTID")]
        public string ApplyUserDeptId { get; set; }


        /// <summary>
        /// 申请人部门name
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERDEPTNAME")]
        public string ApplyUserDeptName { get; set; }
        /// <summary>
        /// 申请奖励金额
        /// </summary>
        /// <returns></returns>
        [Column("APPLYREWARDRMB")]
        public string ApplyRewardRmb { get; set; }
        /// <summary>
        /// 部门负责人id
        /// </summary>
        /// <returns></returns>
        [Column("DEPTPRINCIPALID")]
        public string DeptPrincipalId { get; set; }
        /// <summary>
        /// 总经理意见
        /// </summary>
        /// <returns></returns>
        [Column("CEOOPINION")]
        public string CeoOpinion { get; set; }
        /// <summary>
        /// 分管领导name
        /// </summary>
        /// <returns></returns>
        [Column("LEADERSHIPNAME")]
        public string LeaderShipName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// EHS部负责人是否同意
        /// </summary>
        /// <returns></returns>
        [Column("ISAGREEEHSDEPT")]
        public string IsAgreeEhsDept { get; set; }
        /// <summary>
        /// EHS部负责人意见
        /// </summary>
        /// <returns></returns>
        [Column("EHSDEPTOPINION")]
        public string EhsDeptOpinion { get; set; }
        /// <summary>
        /// EHS部负责人name
        /// </summary>
        /// <returns></returns>
        [Column("EHSDEPTPRINCIPALNAME")]
        public string EhsDeptPrincipalName { get; set; }
        /// <summary>
        /// EHS部负责人id
        /// </summary>
        /// <returns></returns>
        [Column("EHSDEPTPRINCIPALID")]
        public string EhsDeptPrincipalId { get; set; }
        /// <summary>
        /// 专业负责人id
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYPRINCIPALID")]
        public string SpecialtyPrincipalId { get; set; }
        /// <summary>
        /// 部门负责人意见
        /// </summary>
        /// <returns></returns>
        [Column("DEPTPRINCIPALOPINION")]
        public string DeptPrincipalOpinion { get; set; }
        /// <summary>
        /// 奖励事件描述
        /// </summary>
        /// <returns></returns>
        [Column("REWARDREMARK")]
        public string RewardRemark { get; set; }
        /// <summary>
        /// 专业负责人意见
        /// </summary>
        /// <returns></returns>
        [Column("PRINCIPALOPINION")]
        public string PrincipalOpinion { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 申请人id
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERID")]
        public string ApplyUserId { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        /// <returns></returns>
        [Column("APPLYTIME")]
        public DateTime? ApplyTime { get; set; }


        /// <summary>
        /// 审批人id
        /// </summary>
        /// <returns></returns>
        [Column("APPROVERPEOPLEIDS")]
        public string ApproverPeopleIds { get; set; }

        /// <summary>
        /// 审批人name
        /// </summary>
        /// <returns></returns>
        [Column("APPROVERPEOPLENAMES")]
        public string ApproverPeopleNames { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        [Column("BELONGDEPT")]
        public string BelongDept { get; set;}

        /// <summary>
        /// 所属部门ID
        /// </summary>
        [Column("BELONGDEPTID")]
        public string BelongDeptId { get; set; }

        /// <summary>
        /// 奖励依据
        /// </summary>
        [Column("REWARDACCORD")]
        public string RewardAccord { get; set; }

        /// <summary>
        /// 专业意见
        /// </summary>
        [Column("SPECIALTYOPINION")]
        public string SpecialtyOpinion { get; set; }

        /// <summary>
        /// 奖励金额
        /// </summary>
        [Column("REWARDMONEY")]
        public int? RewardMoney { get; set; }
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