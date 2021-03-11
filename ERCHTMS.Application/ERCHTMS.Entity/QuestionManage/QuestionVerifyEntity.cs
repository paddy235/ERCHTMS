using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.QuestionManage
{
    /// <summary>
    /// 描 述：问题验证信息表
    /// </summary>
    [Table("BIS_QUESTIONVERIFY")]
    public class QuestionVerifyEntity : BaseEntity
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
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 验证意见
        /// </summary>
        /// <returns></returns>
        [Column("VERIFYOPINION")]
        public string VERIFYOPINION { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 验证人姓名
        /// </summary>
        /// <returns></returns>
        [Column("VERIFYPEOPLENAME")]
        public string VERIFYPEOPLENAME { get; set; }
        /// <summary>
        /// 验证人签名
        /// </summary>
        /// <returns></returns>
        [Column("VERIFYSIGN")]
        public string VERIFYSIGN { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
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
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 验证人账户
        /// </summary>
        /// <returns></returns>
        [Column("VERIFYPEOPLE")]
        public string VERIFYPEOPLE { get; set; }
        /// <summary>
        /// 问题id
        /// </summary>
        /// <returns></returns>
        [Column("QUESTIONID")]
        public string QUESTIONID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
        /// <summary>
        /// 验证日期
        /// </summary>
        /// <returns></returns>
        [Column("VERIFYDATE")]
        public DateTime? VERIFYDATE { get; set; }
        /// <summary>
        /// 验证部门id
        /// </summary>
        /// <returns></returns>
        [Column("VERIFYDEPTID")]
        public string VERIFYDEPTID { get; set; }
        /// <summary>
        /// 验证部门编码
        /// </summary>
        /// <returns></returns>
        [Column("VERIFYDEPTCODE")]
        public string VERIFYDEPTCODE { get; set; }
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
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 验证结果
        /// </summary>
        /// <returns></returns>
        [Column("VERIFYRESULT")]
        public string VERIFYRESULT { get; set; }
        /// <summary>
        /// 验证部门名称
        /// </summary>
        /// <returns></returns>
        [Column("VERIFYDEPTNAME")]
        public string VERIFYDEPTNAME { get; set; }
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