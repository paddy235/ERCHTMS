using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.BreakRulesManage
{
    /// <summary>
    /// 描 述：违章核准管理
    /// </summary>
    [Table("BIS_BRAPPROVE")]
    public class BrApproveEntity : BaseEntity  
    {
        #region 实体成员
        /// <summary>
        /// 创建日期
        /// </summary>		
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>		
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>		
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>		
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 创建用户部门编码
        /// </summary>		
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建用户机构编码
        /// </summary>		
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }  
        /// <summary>
        /// 修改日期
        /// </summary>		
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>		
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>		
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 违章编码
        /// </summary>		
        [Column("BRCODE")]
        public string BrCode { get; set; }
        /// <summary>
        /// 违章核准结果
        /// </summary>		
        [Column("APPROVERESULT")]
        public string ApproveResult { get; set; }
        /// <summary>
        /// 违章不予核准原因
        /// </summary>		
        [Column("APPROVEREASON")]
        public string ApproveReason { get; set; }
        /// <summary>
        /// 违章核准人id
        /// </summary>		
        [Column("APPROVEPERSON")]
        public string ApprovePerson { get; set; }
        /// <summary>
        /// 违章核准人姓名
        /// </summary>		
        [Column("APPROVEPERSONNAME")]
        public string ApprovePersonName { get; set; } 
        /// <summary>
        /// 违章核准部门编码
        /// </summary>		
        [Column("APPROVEDEPARTCODE")]
        public string ApproveDepartCode { get; set; } 
        /// <summary>
        /// 违章核准部门名称
        /// </summary>		
        [Column("APPROVEDEPARTNAME")]
        public string ApproveDepartName { get; set; } 
        /// <summary>
        /// 违章核准时间
        /// </summary>		
        [Column("APPROVEDATE")]
        public DateTime? ApproveDate { get; set; } 

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            Operator oper = OperatorProvider.Provider.Current();
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = oper.UserId;
            this.CreateUserName = oper.UserName; 
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            Operator oper = OperatorProvider.Provider.Current();
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = oper.UserId;
            this.ModifyUserName = oper.UserName;
        }
        #endregion
    }
}