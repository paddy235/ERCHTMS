using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.LllegalManage
{
    /// <summary>
    /// 描 述：违章整改延期对象
    /// </summary>
    [Table("BIS_LLLEGALEXTENSION")]
    public class LllegalExtensionEntity : BaseEntity
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
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 成功标识
        /// </summary>
        /// <returns></returns>
        [Column("HANDLESIGN")]
        public string HANDLESIGN { get; set; }
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
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
        /// <summary>
        /// 延期结果
        /// </summary>
        /// <returns></returns>
        [Column("POSTPONERESULT")]
        public string POSTPONERESULT { get; set; }
        /// <summary>
        /// 违章ID
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALID")]
        public string LLLEGALID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        /// <returns></returns>
        [Column("HANDLEDATE")]
        public DateTime? HANDLEDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("APPSIGN")]
        public string APPSIGN { get; set; }
        /// <summary>
        /// 申请延期理由
        /// </summary>
        /// <returns></returns>
        [Column("POSTPONEREASON")]
        public string POSTPONEREASON { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 处理人ID
        /// </summary>
        /// <returns></returns>
        [Column("HANDLEUSERID")]
        public string HANDLEUSERID { get; set; }
        /// <summary>
        /// 处理人部门名称
        /// </summary>
        /// <returns></returns>
        [Column("HANDLEDEPTNAME")]
        public string HANDLEDEPTNAME { get; set; }
        /// <summary>
        /// 关联ID
        /// </summary>
        /// <returns></returns>
        [Column("HANDLEID")]
        public string HANDLEID { get; set; }
        /// <summary>
        /// 延期意见
        /// </summary>
        /// <returns></returns>
        [Column("POSTPONEOPINION")]
        public string POSTPONEOPINION { get; set; }
        /// <summary>
        /// 处理人部门编码
        /// </summary>
        /// <returns></returns>
        [Column("HANDLEDEPTCODE")]
        public string HANDLEDEPTCODE { get; set; }
        /// <summary>
        /// 处理人姓名
        /// </summary>
        /// <returns></returns>
        [Column("HANDLEUSERNAME")]
        public string HANDLEUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 处理类型 0 申请 1 审核 2 批复
        /// </summary>
        /// <returns></returns>
        [Column("HANDLETYPE")]
        public string HANDLETYPE { get; set; }
        /// <summary>
        /// 延期天数
        /// </summary>
        /// <returns></returns>
        [Column("POSTPONEDAYS")]
        public string POSTPONEDAYS { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = Guid.NewGuid().ToString();
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
            this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
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