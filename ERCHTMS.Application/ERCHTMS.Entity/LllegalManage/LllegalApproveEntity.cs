using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.LllegalManage
{
    /// <summary>
    /// 描 述：违章核准信息
    /// </summary>
    [Table("BIS_LLLEGALAPPROVE")]
    public class LllegalApproveEntity : BaseEntity
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
        /// 违章id
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALID")]
        public string LLLEGALID { get; set; }
        /// <summary>
        /// 核准人id
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEPERSONID")]
        public string APPROVEPERSONID { get; set; }
        /// <summary>
        /// 核准人
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEPERSON")]
        public string APPROVEPERSON { get; set; }
        /// <summary>
        /// 核准部门编码
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEDEPTCODE")]
        public string APPROVEDEPTCODE { get; set; }
        /// <summary>
        /// 核准部门名称
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEDEPTNAME")]
        public string APPROVEDEPTNAME { get; set; }
        /// <summary>
        /// 核准时间
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEDATE")]
        public DateTime? APPROVEDATE { get; set; }
        /// <summary>
        /// 核准结果
        /// </summary>
        /// <returns></returns>
        [Column("APPROVERESULT")]
        public string APPROVERESULT { get; set; }
        /// <summary>
        /// 核准原因
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEREASON")]
        public string APPROVEREASON { get; set; }

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