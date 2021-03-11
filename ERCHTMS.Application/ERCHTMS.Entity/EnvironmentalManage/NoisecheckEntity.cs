using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EnvironmentalManage
{
    /// <summary>
    /// 描 述：噪音检测
    /// </summary>
    [Table("BIS_NOISECHECK")]
    public class NoisecheckEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
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
        /// 检测人员id
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERID")]
        public string CheckUserId { get; set; }

        /// <summary>
        /// 检测人员
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERNAME")]
        public string CheckUserName { get; set; }

        /// <summary>
        /// 测试日期
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATE")]
        public DateTime? CheckDate { get; set; }
        #endregion

        [Column("ZJ1")]
        public string zj1 { get; set; }
        [Column("ZJ2")]
        public string zj2 { get; set; }
        [Column("ZJ3")]
        public string zj3 { get; set; }
        [Column("ZJ4")]
        public string zj4 { get; set; }
        [Column("ZJ5")]
        public string zj5 { get; set; }
        [Column("ZJ6")]
        public string zj6 { get; set; }

        [Column("YJ1")]
        public string yj1 { get; set; }
        [Column("YJ2")]
        public string yj2 { get; set; }
        [Column("YJ3")]
        public string yj3 { get; set; }
        [Column("YJ4")]
        public string yj4 { get; set; }
        [Column("YJ5")]
        public string yj5 { get; set; }
        [Column("YJ6")]
        public string yj6 { get; set; }
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