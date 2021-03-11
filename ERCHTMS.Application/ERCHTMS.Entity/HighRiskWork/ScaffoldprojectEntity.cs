using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：1.验收项目
    /// </summary>
    [Table("BIS_SCAFFOLDPROJECT")]
    public class ScaffoldprojectEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }

        /// <summary>
        /// 验收结果
        /// </summary>
        /// <returns></returns>
        [Column("RESULT")]
        public string Result { get; set; }
        /// <summary>
        /// 验收结果-不符合
        /// </summary>
        /// <returns></returns>
        [Column("RESULTNO")]
        public string ResultNo { get; set; }
        /// <summary>
        /// 验收人
        /// </summary>
        /// <returns></returns>
        [Column("CHECKPERSONS")]
        public string CheckPersons { get; set; }

        /// <summary>
        /// 验收项目ID
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTID")]
        public string ProjectId { get; set; }
        /// <summary>
        /// 验收项目
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTNAME")]
        public string ProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 验收结果-符合
        /// </summary>
        /// <returns></returns>
        [Column("RESULTYES")]
        public string ResultYes { get; set; }
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
        /// 脚手架信息ID
        /// </summary>
        /// <returns></returns>
        [Column("SCAFFOLDID")]
        public string ScaffoldId { get; set; }
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
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 验收人id
        /// </summary>
        /// <returns></returns>
        [Column("CHECKPERSONSID")]
        public string CheckPersonsId { get; set; }

        /// <summary>
        /// 签名图片地址
        /// </summary>
        /// <returns></returns>
        [Column("SIGNPIC")]
        public string SignPic { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreateDate = this.CreateDate == null ? DateTime.Now : this.CreateDate;
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