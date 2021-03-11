using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：协议
    /// </summary>
    [Table("EPG_PROTOCOL")]
    public class ProtocolEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 工程ID
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTID")]
        public string PROJECTID { get; set; }
        /// <summary>
        /// 签订时间
        /// </summary>
        /// <returns></returns>
        [Column("SIGNDATE")]
        public DateTime? SIGNDATE { get; set; }
        /// <summary>
        /// 签订地点
        /// </summary>
        /// <returns></returns>
        [Column("SIGNPLACE")]
        public string SIGNPLACE { get; set; }
        /// <summary>
        /// 乙方签订ID
        /// </summary>
        /// <returns></returns>
        [Column("SECONDPARTYID")]
        public string SECONDPARTYID { get; set; }
        /// <summary>
        /// 甲方签订人ID
        /// </summary>
        /// <returns></returns>
        [Column("FIRSTPARTYID")]
        public string FIRSTPARTYID { get; set; }
        /// <summary>
        /// 乙方签订人
        /// </summary>
        /// <returns></returns>
        [Column("SECONDPARTY")]
        public string SECONDPARTY { get; set; }
        /// <summary>
        /// 甲方签订人
        /// </summary>
        /// <returns></returns>
        [Column("FIRSTPARTY")]
        public string FIRSTPARTY { get; set; }
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
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
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
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
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
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
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
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        ///是否发送0否，1是
        /// </summary>
        /// <returns></returns>
        [Column("ISSEND")]
        public string ISSEND { get; set; }
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