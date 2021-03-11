using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患复查验证信息表
    /// </summary>
    [Table("BIS_HTRECHECK")]
    public class HtReCheckEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 复查验证意见
        /// </summary>
        /// <returns></returns>
        [Column("RECHECKIDEA")]
        public string RECHECKIDEA { get; set; }
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
        /// 隐患编号
        /// </summary>
        /// <returns></returns>
        [Column("HIDCODE")]
        public string HIDCODE { get; set; }
        /// <summary>
        /// 复查验证单位编码
        /// </summary>
        /// <returns></returns>
        [Column("RECHECKDEPARTCODE")]
        public string RECHECKDEPARTCODE { get; set; }
        /// <summary>
        /// 复查验证单位名称
        /// </summary>
        /// <returns></returns>
        [Column("RECHECKDEPARTNAME")]
        public string RECHECKDEPARTNAME { get; set; }
        /// <summary>
        /// 复查验证人
        /// </summary>
        /// <returns></returns>
        [Column("RECHECKPERSON")]
        public string RECHECKPERSON { get; set; }
        /// <summary>
        /// 复查验证人姓名
        /// </summary>
        /// <returns></returns>
        [Column("RECHECKPERSONNAME")]
        public string RECHECKPERSONNAME { get; set; }
        /// <summary>
        /// 复查验证时间
        /// </summary>
        /// <returns></returns>
        [Column("RECHECKDATE")]
        public DateTime? RECHECKDATE { get; set; }
        /// <summary>
        /// 复查验证结果
        /// </summary>
        /// <returns></returns>
        [Column("RECHECKSTATUS")]
        public string RECHECKSTATUS { get; set; }
        /// <summary>
        /// 复查验证照片
        /// </summary>
        /// <returns></returns>
        [Column("RECHECKPHOTO")]
        public string RECHECKPHOTO { get; set; }
        /// <summary>
        /// 应用标记(web or  android  or ios)
        /// </summary>
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