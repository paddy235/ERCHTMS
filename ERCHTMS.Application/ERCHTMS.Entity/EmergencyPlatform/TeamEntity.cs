using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急任务
    /// </summary>
    [Table("MAE_TEAM")]
    public class TeamEntity : BaseEntity
    {
        #region 实体成员

        [Column("USERFULLNAME")]
        public string USERFULLNAME { get; set; }

        [Column("DEPARTID")]
        public string DEPARTID { get; set; }
        [Column("DEPARTNAME")]
        public string DEPARTNAME { get; set; }
        [Column("MOBILE")]
        public string MOBILE { get; set; }
        [Column("POSTNAME")]
        public string POSTNAME { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        [Column("TEAMID")]
        public string TEAMID { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 创建人名字
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 创建人的组织机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 最后修改人名字
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string USERID { get; set; }
        /// <summary>
        /// 创建人部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 岗位ID
        /// </summary>
        /// <returns></returns>
        [Column("POSTID")]
        public string POSTID { get; set; }
        /// <summary>
        /// 组织机构
        /// </summary>
        /// <returns></returns>
        [Column("ORGNAME")]
        public string OrgName { get; set; }
        /// <summary>
        /// 组织机构
        /// </summary>
        /// <returns></returns>
        [Column("ORGCODE")]
        public string OrgCode { get; set; }
        /// <summary>
        /// 备注
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
            this.TEAMID = string.IsNullOrEmpty(TEAMID) ? Guid.NewGuid().ToString() : TEAMID;
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
            this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.TEAMID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}