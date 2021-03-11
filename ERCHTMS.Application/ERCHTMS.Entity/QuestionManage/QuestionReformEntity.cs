using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.QuestionManage
{
    /// <summary>
    /// 描 述：问题整改信息
    /// </summary>
    [Table("BIS_QUESTIONREFORM")]
    public class QuestionReformEntity : BaseEntity
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
        /// 问题id
        /// </summary>
        /// <returns></returns>
        [Column("QUESTIONID")]
        public string QUESTIONID { get; set; }
        /// <summary>
        /// 整改责任单位ID
        /// </summary>
        /// <returns></returns>
        [Column("REFORMDEPTID")]
        public string REFORMDEPTID { get; set; }
        /// <summary>
        /// 整改责任单位编码
        /// </summary>
        /// <returns></returns>
        [Column("REFORMDEPTCODE")]
        public string REFORMDEPTCODE { get; set; }
        /// <summary>
        /// 整改责任单位名称
        /// </summary>
        /// <returns></returns>
        [Column("REFORMDEPTNAME")]
        public string REFORMDEPTNAME { get; set; }
        /// <summary>
        /// 联责单位ID
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPTID")]
        public string DUTYDEPTID { get; set; }
        /// <summary>
        /// 联责单位编码
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPTCODE")]
        public string DUTYDEPTCODE { get; set; }
        /// <summary>
        /// 联责单位名称
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPTNAME")]
        public string DUTYDEPTNAME { get; set; }
        /// <summary>
        /// 整改责任人姓名
        /// </summary>
        /// <returns></returns>
        [Column("REFORMPEOPLENAME")]
        public string REFORMPEOPLENAME { get; set; }
        /// <summary>
        /// 整改责任人账户
        /// </summary>
        /// <returns></returns>
        [Column("REFORMPEOPLE")]
        public string REFORMPEOPLE { get; set; }
        /// <summary>
        /// 整改人电话
        /// </summary>
        /// <returns></returns>
        [Column("REFORMTEL")]
        public string REFORMTEL { get; set; }
        /// <summary>
        /// 计划完成日期
        /// </summary>
        /// <returns></returns>
        [Column("REFORMPLANDATE")]
        public DateTime? REFORMPLANDATE { get; set; }
        /// <summary>
        /// 整改情况描述
        /// </summary>
        /// <returns></returns>
        [Column("REFORMDESCRIBE")]
        public string REFORMDESCRIBE { get; set; }
        /// <summary>
        /// 整改措施
        /// </summary>
        /// <returns></returns>
        [Column("REFORMMEASURE")]
        public string REFORMMEASURE { get; set; }
        /// <summary>
        /// 完成情况
        /// </summary>
        /// <returns></returns>
        [Column("REFORMSTATUS")]
        public string REFORMSTATUS { get; set; }
        /// <summary>
        /// 未完成原因说明
        /// </summary>
        /// <returns></returns>
        [Column("REFORMREASON")]
        public string REFORMREASON { get; set; }
        /// <summary>
        /// 整改责任人签名
        /// </summary>
        /// <returns></returns>
        [Column("REFORMSIGN")]
        public string REFORMSIGN { get; set; }
        /// <summary>
        /// 整改完成日期
        /// </summary>
        /// <returns></returns>
        [Column("REFORMFINISHDATE")]
        public DateTime? REFORMFINISHDATE { get; set; }
        /// <summary>
        /// 整改图片
        /// </summary>
        /// <returns></returns>
        [Column("REFORMPIC")]
        public string REFORMPIC { get; set; }
        /// <summary>
        /// 移动端处理标记
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