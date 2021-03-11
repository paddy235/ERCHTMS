using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.QuestionManage
{
    /// <summary>
    /// 描 述：发现问题基本信息表
    /// </summary>
    [Table("BIS_FINDQUESTIONINFO")]
    public class FindQuestionInfoEntity : BaseEntity
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
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// 所属单位id
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZEID")]
        public string ORGANIZEID { get; set; }
        /// <summary>
        /// 所属单位名称
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZENAME")]
        public string ORGANIZENAME { get; set; }
        /// <summary>
        /// 所属部门id
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DEPTID { get; set; }
        /// <summary>
        /// 所属部门名称
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DEPTNAME { get; set; }
        /// <summary>
        /// 问题内容
        /// </summary>
        /// <returns></returns>
        [Column("QUESTIONCONTENT")]
        public string QUESTIONCONTENT { get; set; }
        /// <summary>
        /// 问题图片
        /// </summary>
        /// <returns></returns>
        [Column("QUESTIONPIC")]
        public string QUESTIONPIC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWSTATE")]
        public string FLOWSTATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("APPSIGN")]
        public string APPSIGN { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
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
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}