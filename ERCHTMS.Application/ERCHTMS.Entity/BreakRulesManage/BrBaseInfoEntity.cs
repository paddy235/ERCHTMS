using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.BreakRulesManage
{
    /// <summary>
    /// 描 述：违章信息管理
    /// </summary>
    [Table("BIS_BRBASEINFO")]
    public class BrBaseInfoEntity : BaseEntity 
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
        /// 违章部门编码
        /// </summary>		
        [Column("BRDEPARTCODE")]
        public string BrDepartCode { get; set; }
        /// <summary>
        /// 违章部门名称
        /// </summary>		
        [Column("BRDEPARTNAME")]
        public string BrDepartName { get; set; }
        /// <summary>
        /// 违章类型
        /// </summary>		
        [Column("BRTYPE")]
        public string BrType { get; set; }
        /// <summary>
        /// 违章班组编码
        /// </summary>		
        [Column("BRCLASSCODE")]
        public string BrClassCode { get; set; }
        /// <summary>
        /// 违章班组名称
        /// </summary>		
        [Column("BRCLASSNAME")]
        public string BrClassName { get; set; }
        /// <summary>
        /// 违章级别
        /// </summary>		
        [Column("BRGRADE")]
        public string BrGrade { get; set; } 
        /// <summary>
        /// 违章人员id
        /// </summary>		
        [Column("BRPERSON")]
        public string BrPerson { get; set; } 
        /// <summary>
        /// 违章人员姓名
        /// </summary>		
        [Column("BRPERSONNAME")]
        public string BrPersonName { get; set; } 
        /// <summary>
        /// 违章时间
        /// </summary>		
        [Column("BRDATE")]
        public DateTime? BrDate { get; set; }
        /// <summary>
        /// 违章地址
        /// </summary>		
        [Column("BRADDRESS")]
        public string BrAddress { get; set; }
        /// <summary>
        /// 违章描述
        /// </summary>		
        [Column("BRDESCRIBE")]
        public string BrDescribe { get; set; }
        /// <summary>
        /// 违章照片
        /// </summary>		
        [Column("BRPHOTO")]
        public string BrPhoto { get; set; }  
 
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