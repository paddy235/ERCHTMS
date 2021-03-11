using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.BreakRulesManage
{
    /// <summary>
    /// 描 述：违章整改管理
    /// </summary>
    [Table("BIS_BRREFORM")]
    public class BrReformEntity : BaseEntity 
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
        /// 违章整改责任人id
        /// </summary>		
        [Column("REFORMPERSON")]
        public string ReformPerson { get; set; }
        /// <summary>
        /// 违章整改责任人姓名
        /// </summary>		
        [Column("REFORMPERSONNAME")]
        public string ReformPersonName { get; set; }
        /// <summary>
        /// 违章整改责任单位编码
        /// </summary>		
        [Column("REFORMDEPARTCODE")]
        public string ReformDepartCode { get; set; }
        /// <summary>
        /// 违章整改责任单位名称
        /// </summary>		
        [Column("REFORMDEPARTNAME")]
        public string ReformDepartName { get; set; }
        /// <summary>
        /// 违章整改要求
        /// </summary>		
        [Column("REFORMREQUIRE")]
        public string ReformRequire { get; set; }
        /// <summary>
        /// 违章整改截止时间
        /// </summary>		
        [Column("REFORMDEADLINE")]
        public DateTime? ReformDeadline { get; set; }
        /// <summary>
        /// 违章整改完成时间
        /// </summary>		
        [Column("REFORMFINISHDATE")]
        public DateTime? ReformFinishDate { get; set; }
        /// <summary>
        /// 违章整改措施
        /// </summary>		
        [Column("REFORMMEASURE")]
        public string ReformMeasure { get; set; }
        /// <summary>
        /// 违章整改情况描述
        /// </summary>		
        [Column("REFORMDESCRIBE")]
        public string ReformDescribe { get; set; }
        /// <summary>
        /// 违章整改结果
        /// </summary>		
        [Column("REFORMRESULT")]
        public string ReformResult { get; set; } 
        /// <summary>
        /// 违章整改图片
        /// </summary>		
        [Column("REFORMPHOTO")]
        public string ReformPhoto { get; set; }
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