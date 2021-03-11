using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.Observerecord
{
    /// <summary>
    /// 描 述：观察类别表
    /// </summary>
    [Table("BIS_OBSERVECATEGORY")]
    public class ObservecategoryEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
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
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 观察类别名称
        /// </summary>
        /// <returns></returns>
        [Column("OBSERVETYPENAME")]
        public string ObservetypeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 观察类别 1:员工反应 2 个人防护装备 3程序和标准 4员工的位置 5工具与设备 6 人体工效学 7环境整洁
        /// </summary>
        /// <returns></returns>
        [Column("OBSERVETYPE")]
        public string Observetype { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 是否全部安全：1全部安全 0 存在问题
        /// </summary>
        /// <returns></returns>
        [Column("ISALLSAFETY")]
        public int? IsallSafety { get; set; }
        /// <summary>
        /// 存在问题
        /// </summary>
        /// <returns></returns>
        [Column("EXISTPROBLEM")]
        public string ExistProblem { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 观察记录Id
        /// </summary>
        /// <returns></returns>
        [Column("RECORDID")]
        public string Recordid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 存在问题CODE
        /// </summary>
        /// <returns></returns>
        [Column("EXISTPROBLEMCODE")]
        public string ExistProblemCode { get; set; }
        /// <summary>
        /// 备注字段
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
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