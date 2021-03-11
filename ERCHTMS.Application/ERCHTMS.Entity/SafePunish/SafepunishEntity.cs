using System;
using System.ComponentModel;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SafePunish
{
    /// <summary>
    /// 描 述：安全惩罚
    /// </summary>
    [Table("BIS_SAFEPUNISH")]
    public class SafepunishEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
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
        /// 惩罚编号
        /// </summary>
        /// <returns></returns>
        [Column("SAFEPUNISHCODE")]
        public string SafePunishCode { get; set; }
        /// <summary>
        /// 申请人id
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERID")]
        public string ApplyUserId { get; set; }
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
        /// 惩罚时间
        /// </summary>
        /// <returns></returns>
        [Column("APPLYTIME")]
        public DateTime? ApplyTime { get; set; }
        /// <summary>
        /// 惩罚总金额
        /// </summary>
        /// <returns></returns>
        [Column("APPLYPUNISHRMB")]
        public string ApplyPunishRmb { get; set; }
        /// <summary>
        /// 惩罚总积分
        /// </summary>
        /// <returns></returns>
        [Column("APPLYPUNISHSCORE")]
        public string ApplyPunishScore { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHTYPE")]
        public string PunishType { get; set; }

        /// <summary>
        /// 惩罚对象
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHOBJECTNAMES")]
        public string PunishObjectNames { get; set; }
        
        /// <summary>
        /// 惩罚类别（1：事故事件；2：其他）
        /// </summary>
        /// <returns></returns>
        [Column("AMERCETYPE")]
        public string AmerceType { get; set; }


        /// <summary>
        /// 被考核单位id
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTID")]
        public string ApplyDeptId { get; set; }
   
        /// <summary>
        /// 被考核单位code
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTCODE")]
        public string ApplyDeptCode { get; set; }
    
        /// <summary>
        /// 被考核单位name
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTNAME")]
        public string ApplyDeptName { get; set; }


        /// <summary>
        /// 被考核人员id
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHUSERID")]
        public string PunishUserId { get; set; }
     
	
        /// <summary>
        /// 被考核人员name
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHUSERNAME")]
        public string PunishUserName { get; set; }

        /// <summary>
        /// 考核金额
        /// </summary>
        /// <returns></returns>
        [Column("AMERCEAMOUNT")]
        public string AmerceAmount { get; set; }



        /// <summary>
        /// 外包单位名称
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHDEPTNAME")]
        public string PunishDeptName { get; set; }
        /// <summary>
        /// 外包单位名称
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHDEPTID")]
        public string PunishDeptId { get; set; }
        /// <summary>
        /// 惩罚具体事项
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHREMARK")]
        public string PunishRemark { get; set; }
        /// <summary>
        /// 流程状态(0正在处理.,1.已处理,2.未处理)
        /// </summary>
        /// <returns></returns>
        [Column("FLOWSTATE")]
        public string FlowState { get; set; }
        /// <summary>
        /// 申请状态(1.专业意见,2.部门意见)
        /// </summary>
        /// <returns></returns>
        [Column("APPLYSTATE")]
        public string ApplyState { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        /// <returns></returns>
        [Column("APPROVERPEOPLEIDS")]
        public string ApproverPeopleIds { get; set; }
        /// <summary>
        /// 审批人名称
        /// </summary>
        /// <returns></returns>
        [Column("APPROVERPEOPLENAMES")]
        public string ApproverPeopleNames { get; set; }



        /// <summary>
        /// 专业主管
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYMANAGERID")]
        public string SpecialtyManagerId { get; set; }


        /// <summary>
        /// 部门主管
        /// </summary>
        /// <returns></returns>
        [Column("DEPTMANAGERID")]
        public string DeptManagerId { get; set; }

        /// <summary>
        /// 专业负责人id
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYPRINCIPALID")]
        public string SpecialtyPrincipalId { get; set; }

        /// <summary>
        /// 专业负责人name
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYPRINCIPALNAME")]
        public string SpecialtyPrincipalName { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        [Column("BELONGDEPT")]
        public string BelongDept { get; set; }


        /// <summary>
        /// 所属部门ID
        /// </summary>
        [Column("BELONGDEPTID")]
        public string BelongDeptId { get; set; }

        /// <summary>
        /// 考核类型    
        /// </summary>
        [Column("EXAMINETYPE")]
        public string ExamineType { get; set; }

        /// <summary>
        /// 考核依据
        /// </summary>
        [Column("PUNISHACCORD")]
        public string PunishAccord { get; set; }

        /// <summary>
        /// 专业意见
        /// </summary>
        [Column("SPECIALTYOPINION")]
        public string SpecialtyOpinion { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            if (string.IsNullOrEmpty( this.ApplyPunishRmb))
            {
                this.ApplyPunishRmb = "0";
            }
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
            if (string.IsNullOrEmpty(this.ApplyPunishRmb))
            {
                this.ApplyPunishRmb = "0";
            }
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}