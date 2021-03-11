using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.LllegalManage
{
    /// <summary>
    /// 描 述：违章责任人处罚记录表
    /// </summary>
    [Table("BIS_LLLEGALPUNISH")]
    public class LllegalPunishEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 应用标记(web or  android  or ios)
        /// </summary>
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
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// 标记(用于区分是记录还是核准附带的考核信息)
        /// </summary>
        /// <returns></returns>
        [Column("MARK")]
        public string MARK { get; set; }
        /// <summary>
        /// 违章扣分
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALPOINT")]
        public decimal? LLLEGALPOINT { get; set; }
        /// <summary>
        /// 违章责任人id
        /// </summary>
        /// <returns></returns>
        [Column("PERSONINCHARGEID")]
        public string PERSONINCHARGEID { get; set; }
        /// <summary>
        /// 违章id
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALID")]
        public string LLLEGALID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
        /// <summary>
        /// 核准id
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEID")]
        public string APPROVEID { get; set; }
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
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 违章责任人姓名
        /// </summary>
        /// <returns></returns>
        [Column("PERSONINCHARGENAME")]
        public string PERSONINCHARGENAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 经济处罚
        /// </summary>
        /// <returns></returns>
        [Column("ECONOMICSPUNISH")]
        public decimal? ECONOMICSPUNISH { get; set; }
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
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 待岗
        /// </summary>
        /// <returns></returns>
        [Column("AWAITJOB")]
        public decimal? AWAITJOB { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 教育培训
        /// </summary>
        /// <returns></returns>
        [Column("EDUCATION")]
        public decimal? EDUCATION { get; set; }
        /// <summary>
        /// EHS绩效考核分
        /// </summary>
        /// <returns></returns>
        [Column("PERFORMANCEPOINT")]
        public decimal? PERFORMANCEPOINT { get; set; }
        /// <summary>
        /// EHS考核对象 （人员 or 单位）
        /// </summary>
        /// <returns></returns>
        [Column("ASSESSOBJECT")]
        public string ASSESSOBJECT { get; set; }
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