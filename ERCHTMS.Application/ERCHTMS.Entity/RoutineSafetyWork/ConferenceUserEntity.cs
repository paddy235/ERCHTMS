using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：安全会议参会人员表
    /// </summary>
    [Table("BIS_CONFERENCEUSER")]
    public class ConferenceUserEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 审核状态(默认为0)(0:未申请请假,1:请假审批中,2:请假已批准,3请假未批准)
        /// </summary>
        /// <returns></returns>
        [Column("REVIEWSTATE")]
        public string ReviewState { get; set; }
        /// <summary>
        /// 审核人ID
        /// </summary>
        /// <returns></returns>
        [Column("REVIEWUSERID")]
        public string ReviewUserID { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        /// <returns></returns>
        [Column("REVIEWUSER")]
        public string ReviewUser { get; set; }
        /// <summary>
        /// 请假原因
        /// </summary>
        /// <returns></returns>
        [Column("REASON")]
        public string Reason { get; set; }
        /// <summary>
        /// 是否签到(0是，1否)
        /// </summary>
        /// <returns></returns>
        [Column("ISSIGN")]
        public string Issign { get; set; }
        /// <summary>
        /// 签名照片路径
        /// </summary>
        /// <returns></returns>
        [Column("PHOTOURL")]
        public string PhotoUrl { get; set; }
        /// <summary>
        /// 会议记录ID
        /// </summary>
        /// <returns></returns>
        [Column("CONFERENCEID")]
        public string ConferenceID { get; set; }
        /// <summary>
        /// 所属部门ID
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
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
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
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
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
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