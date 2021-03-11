using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EvaluateManage
{
    /// <summary>
    /// 描 述：合规性评价
    /// </summary>
    [Table("HRS_EVALUATE")]
    public class EvaluateEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
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
        /// 工作标题
        /// </summary>
        /// <returns></returns>
        [Column("WORKTITLE")]
        public string WorkTitle { get; set; }
        /// <summary>
        /// 评价人
        /// </summary>
        /// <returns></returns>
        [Column("APPRAISERUSER")]
        public string AppraiserUser { get; set; }
        /// <summary>
        /// 评价人ID
        /// </summary>
        /// <returns></returns>
        [Column("APPRAISERUSERID")]
        public string AppraiserUserId { get; set; }
        /// <summary>
        /// 评价部门
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPT")]
        public string DutyDept { get; set; }
        /// <summary>
        /// 评价部门编码
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPTCODE")]
        public string DutyDeptCode { get; set; }
        /// <summary>
        /// 评价状态
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATESTATE")]
        public int? EvaluateState { get; set; }

        /// <summary>
        /// 评价时间
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEDATE")]
        public DateTime? EvaluateDate { get; set; }

        /// <summary>
        /// 评价计划ID
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEPLANID")]
        public string EvaluatePlanId { get; set; }
        /// <summary>
        /// 整改状态
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFYSTATE")]
        public int? RectifyState { get; set; }
        /// <summary>
        /// 整改人
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFYPERSON")]
        public string RectifyPerson { get; set; }
        /// <summary>
        /// 整改人ID
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFYPERSONID")]
        public string RectifyPersonId { get; set; }

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