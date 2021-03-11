using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.FireManage
{
    /// <summary>
    /// 描 述：消防队伍
    /// </summary>
    [Table("HRS_FIRETROOPS")]
    public class FireTroopsEntity : BaseEntity
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
        public int? AutoId { get; set; }
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
        /// 姓名
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        
        /// <summary>
        /// 部门
        /// </summary>
        /// <returns></returns>
        [Column("DEPT")]
        public string Dept { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        /// <returns></returns>
        [Column("SEX")]
        public string Sex { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        /// <returns></returns>
        [Column("IDENTITYCARD")]
        public string IdentityCard { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        /// <returns></returns>
        [Column("QUARTERS")]
        public string Quarters { get; set; }
        /// <summary>
        /// 职务ID
        /// </summary>
        /// <returns></returns>
        [Column("QUARTERSID")]
        public string QuartersId { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        /// <returns></returns>
        [Column("PHONE")]
        public string Phone { get; set; }
        /// <summary>
        /// 学历
        /// </summary>
        /// <returns></returns>
        [Column("DEGREES")]
        public string Degrees { get; set; }
        /// <summary>
        /// 学历ID
        /// </summary>
        /// <returns></returns>
        [Column("DEGREESID")]
        public string DegreesId { get; set; }
        /// <summary>
        /// 户籍所在地
        /// </summary>
        /// <returns></returns>
        [Column("PLACEDOMICILE")]
        public string PlaceDomicile { get; set; }
        /// <summary>
        /// 持证情况
        /// </summary>
        /// <returns></returns>
        [Column("CERTIFICATES")]
        public string Certificates { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        /// <returns></returns>
        [Column("SORTCODE")]
        public int? SortCode { get; set; }

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