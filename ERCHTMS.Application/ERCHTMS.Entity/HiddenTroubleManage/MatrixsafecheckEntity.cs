using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：矩阵安全检查计划
    /// </summary>
    [Table("BIS_MATRIXSAFECHECK")]
    public class MatrixsafecheckEntity : BaseEntity
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
        /// 检查内容id ，多选逗号分隔
        /// </summary>
        /// <returns></returns>
        [Column("CONTENTID")]
        public string CONTENTID { get; set; }
        /// <summary>
        /// 检查内容
        /// </summary>
        /// <returns></returns>
        [Column("CONTENT")]
        public string CONTENT { get; set; }
        /// <summary>
        /// 检查时间
        /// </summary>
        /// <returns></returns>
        [Column("CHECKTIME")]
        public DateTime? CHECKTIME { get; set; }
        /// <summary>
        /// 检查部门id
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPT")]
        public string CHECKDEPT { get; set; }
        /// <summary>
        /// 检查部门code
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTCODE")]
        public string CHECKDEPTCODE { get; set; }
        /// <summary>
        /// 检查部门名称
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTNAME")]
        public string CHECKDEPTNAME { get; set; }
        /// <summary>
        /// 检查人员id
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSER")]
        public string CHECKUSER { get; set; }
        /// <summary>
        /// 检查人员code
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERCODE")]
        public string CHECKUSERCODE { get; set; }
        /// <summary>
        /// 检查人员名称
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERNAME")]
        public string CHECKUSERNAME { get; set; }
        /// <summary>
        /// 0：未完成 1:已完成
        /// </summary>
        /// <returns></returns>
        [Column("ISOVER")]
        public int? ISOVER { get; set; }
        /// <summary>
        /// 安全检查id
        /// </summary>
        /// <returns></returns>
        [Column("CHECKID")]
        public string CHECKID { get; set; }

        /// <summary>
        /// 检查内容编号，多选逗号分隔
        /// </summary>
        [Column("CONTENTNUM")]
        public string CONTENTNUM { get; set; }

        /// <summary>
        /// 检查部门编号，多选逗号分隔
        /// </summary>
        [Column("CHECKDEPTNUM")]
        public string CHECKDEPTNUM { get; set; }

        /// <summary>
        /// 检查部门选择的ID
        /// </summary>
        [Column("CHECKDEPTSEL")]
        public string CHECKDEPTSEL { get; set; }

        /// <summary>
        /// 检查人员所在部门
        /// </summary>
        [Column("CHECKUSERDEPT")]
        public string CHECKUSERDEPT { get; set; }
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