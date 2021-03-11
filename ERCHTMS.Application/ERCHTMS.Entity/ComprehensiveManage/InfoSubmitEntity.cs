using ERCHTMS.Code;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERCHTMS.Entity.ComprehensiveManage
{
    /// <summary>
    /// 描 述：信息报送
    /// </summary>
    [Table("HRS_INFOSUBMIT")]
    public class InfoSubmitEntity : BaseEntity
    {
        #region 默认字段
        /// <summary>
        /// 主键
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
        #endregion

        #region 实体成员   
        /// <summary>
        /// 报送信息名称
        /// </summary>
        [Column("INFONAME")]
        public string InfoName { get; set; }
        /// <summary>
        /// 要求
        /// </summary>
        [Column("REQUIRE")]
        public string Require { get; set; }
        /// <summary>
        /// 报送开始时间
        /// </summary>
        [Column("STARTTIME")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 报送结束时间
        /// </summary>
        [Column("ENDTIME")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 报送人id
        /// </summary>
        [Column("SUBMITUSERID")]
        public string SubmitUserId { get; set; }
        /// <summary>
        /// 已报送人id
        /// </summary>
        [Column("SUBMITEDUSERID")]
        public string SubmitedUserId { get; set; }
        /// <summary>
        /// 报送人帐号
        /// </summary>
        [Column("SUBMITUSERACCOUNT")]
        public string SubmitUserAccount { get; set; }
        /// <summary>
        /// 报送人姓名
        /// </summary>
        [Column("SUBMITUSERNAME")]
        public string SubmitUserName { get; set; }
        /// <summary>
        /// 报送部门id
        /// </summary>
        [Column("SUBMITDEPARTID")]
        public string SubmitDepartId { get; set; }
        /// <summary>
        /// 报送部门名称
        /// </summary>
        [Column("SUBMITDEPARTNAME")]
        public string SubmitDepartName { get; set; }
        /// <summary>
        /// 报送完成情况（百分比）
        /// </summary>
        [Column("PCT")]
        public decimal? Pct { get; set; }
        /// <summary>
        /// 剩余未报送人数
        /// </summary>
        [Column("REMNUM")]
        public int? Remnum { get; set; }
        /// <summary>
        /// 剩余未报送人姓名
        /// </summary>
        [Column("REMUSERNAME")]
        public string RemUserName { get; set; }
        /// <summary>
        /// 剩余未报送人部门
        /// </summary>
        [Column("REMDEPARTNAME")]
        public string RemDepartName { get; set; }
        /// <summary>
        /// 是否发送(值：是、否)
        /// </summary>
        [Column("ISSUBMIT")]
        public string IsSubmit { get; set; }
        /// <summary>
        /// 类型(月报、季报、半年报、年报)
        /// </summary>
        [Column("INFOTYPE")]
        public string InfoType { get; set; }
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