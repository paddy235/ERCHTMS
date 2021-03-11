using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.QuestionManage
{
    /// <summary>
    /// 描 述：问题基本信息表
    /// </summary>
    [Table("BIS_QUESTIONINFO")]
    public class QuestionInfoEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
        /// <summary>
        /// 所属单位名称
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTNAME")]
        public string BELONGDEPTNAME { get; set; }
        /// <summary>
        /// 问题编号
        /// </summary>
        /// <returns></returns>
        [Column("QUESTIONNUMBER")] 
        public string QUESTIONNUMBER { get; set; }
        /// <summary>
        /// 问题地点
        /// </summary>
        /// <returns></returns>
        [Column("QUESTIONADDRESS")]
        public string QUESTIONADDRESS { get; set; }
        /// <summary>
        /// 问题描述
        /// </summary>
        /// <returns></returns>
        [Column("QUESTIONDESCRIBE")]
        public string QUESTIONDESCRIBE { get; set; }
        /// <summary>
        /// 问题图片
        /// </summary>
        /// <returns></returns>
        [Column("QUESTIONPIC")]
        public string QUESTIONPIC { get; set; }
        /// <summary>
        /// 检查人姓名
        /// </summary>
        /// <returns></returns>
        [Column("CHECKPERSONNAME")]
        public string CHECKPERSONNAME { get; set; }
        /// <summary>
        /// 检查人id
        /// </summary>
        /// <returns></returns>
        [Column("CHECKPERSONID")]
        public string CHECKPERSONID { get; set; }
        /// <summary>
        /// 检查单位id
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTID")]
        public string CHECKDEPTID { get; set; }
        /// <summary>
        /// 检查单位名称
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTNAME")]
        public string CHECKDEPTNAME { get; set; }
        /// <summary>
        /// 检查类型
        /// </summary>
        /// <returns></returns>
        [Column("CHECKTYPE")]
        public string CHECKTYPE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CHECKNAME")]
        public string CHECKNAME { get; set; }
        /// <summary>
        /// 检查id
        /// </summary>
        /// <returns></returns>
        [Column("CHECKID")]
        public string CHECKID { get; set; }
        /// <summary>
        /// 检查日期
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATE")]
        public DateTime? CHECKDATE { get; set; }
        /// <summary>
        /// 检查重点内容
        /// </summary>
        /// <returns></returns>
        [Column("CHECKCONTENT")]
        public string CHECKCONTENT { get; set; }
        /// <summary>
        /// 关联应用id
        /// </summary>
        /// <returns></returns>
        [Column("RELEVANCEID")]
        public string RELEVANCEID { get; set; }
        /// <summary>
        /// 关联应用id
        /// </summary>
        /// <returns></returns>
        [Column("CORRELATIONID")]
        public string CORRELATIONID { get; set; }
        /// <summary>
        /// 流程状态
        /// </summary>
        /// <returns></returns>
        [Column("FLOWSTATE")]
        public string FLOWSTATE { get; set; }
        /// <summary>
        /// 流程标识
        /// </summary>
        /// <returns></returns>
        [Column("QFLAG")]
        public string QFLAG { get; set; }
        /// <summary>
        /// 移动端处理标记
        /// </summary>
        /// <returns></returns>
        [Column("APPSIGN")]
        public string APPSIGN { get; set; }
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
        [Column("BELONGDEPTID")]
        public string BELONGDEPTID { get; set; }
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