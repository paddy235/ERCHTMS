using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.BreakRulesManage
{
    /// <summary>
    /// 描 述：违章验收管理
    /// </summary>
    [Table("BIS_BRACCEPT")]
    public class BrAcceptEntity : BaseEntity
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
        /// 违章验收人id
        /// </summary>		
        [Column("ACCEPTPERSON")]
        public string AcceptPerson { get; set; } 
        /// <summary>
        /// 违章验收人姓名
        /// </summary>		
        [Column("ACCEPTPERSONNAME")]
        public string AcceptPersonName { get; set; } 
        /// <summary>
        /// 违章验收部门编码
        /// </summary>		
        [Column("ACCEPTDEPARTCODE")]
        public string AcceptDepartCode { get; set; } 
        /// <summary>
        /// 违章验收部门名称
        /// </summary>		
        [Column("ACCEPTDEPARTNAME")]
        public string AcceptDepartName { get; set; } 
        /// <summary>
        /// 违章验收结果
        /// </summary>		
        [Column("ACCEPTRESULT")]
        public string AcceptResult { get; set; } 
        /// <summary>
        /// 违章验收时间
        /// </summary>		
        [Column("ACCEPTDATE")]
        public DateTime? AcceptDate { get; set; } 
        /// <summary>
        /// 违章验收意见
        /// </summary>		
        [Column("ACCEPTIDEA")]
        public string AcceptIdea { get; set; } 
        /// <summary>
        /// 违章验收图片
        /// </summary>		
        [Column("ACCEPTPHOTO")]
        public string AcceptPhoto { get; set; }  

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