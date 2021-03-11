using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.BreakRulesManage
{
    /// <summary>
    /// 描 述：违章考核管理
    /// </summary>
    [Table("BIS_BRASSESS")]
    public class BrAssessEntity : BaseEntity
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
        /// 违章考核人id
        /// </summary>		
        [Column("ASSESSPERSON")]
        public string AssessPerson { get; set; }
        /// <summary>
        /// 违章考核人姓名
        /// </summary>		
        [Column("ASSESSPERSONNAME")]
        public string AssessPersonName { get; set; }
        /// <summary>
        /// 违章考核部门编码
        /// </summary>		
        [Column("ASSESSDEPARTCODE")]
        public string AssessDepartCode { get; set; }
        /// <summary>
        /// 违章考核部门名称
        /// </summary>		
        [Column("ASSESSDEPARTNAME")]
        public string AssessDepartName { get; set; }
        /// <summary>
        /// 违章考核方式
        /// </summary>		
        [Column("ASSESSWAY")]
        public string AssessWay { get; set; }
        /// <summary>
        /// 违章考核金额(处罚金额)
        /// </summary>		
        [Column("ASSESSMONEY")]
        public decimal? AssessMoney { get; set; }

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