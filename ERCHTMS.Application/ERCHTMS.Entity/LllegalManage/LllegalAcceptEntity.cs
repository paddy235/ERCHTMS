using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.LllegalManage
{
    /// <summary>
    /// 描 述：违章验收信息
    /// </summary>
    [Table("BIS_LLLEGALACCEPT")]
    public class LllegalAcceptEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 应用标记(web or  android  or ios)
        /// </summary>
        [Column("APPSIGN")]
        public string APPSIGN { get; set; }
        /// <summary>
        /// 验收确认人ID
        /// </summary>
        [Column("CONFIRMUSERID")]
        public string CONFIRMUSERID { get; set; }
        /// <summary>
        /// 验收确认人姓名
        /// </summary>
        [Column("CONFIRMUSERNAME")]
        public string CONFIRMUSERNAME { get; set; }
        /// <summary>
        /// 是否省公司验收
        /// </summary>
        [Column("ISGRPACCEPT")]
        public string ISGRPACCEPT { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
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
        /// 验收部门
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTDEPTCODE")]
        public string ACCEPTDEPTCODE { get; set; }
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
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
        /// <summary>
        /// 验收意见
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTMIND")]
        public string ACCEPTMIND { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// 验收部门
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTDEPTNAME")]
        public string ACCEPTDEPTNAME { get; set; }
        /// <summary>
        /// 验收人
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTPEOPLEID")]
        public string ACCEPTPEOPLEID { get; set; }
        /// <summary>
        /// 验收时间
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTTIME")]
        public DateTime? ACCEPTTIME { get; set; }
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
        /// 验收图片
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTPIC")]
        public string ACCEPTPIC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 验收人
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTPEOPLE")]
        public string ACCEPTPEOPLE { get; set; }
        /// <summary>
        /// 验收结果
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTRESULT")]
        public string ACCEPTRESULT { get; set; }
        /// <summary>
        /// 违章id
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALID")]
        public string LLLEGALID { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = DateTime.Now;
            if (string.IsNullOrEmpty(this.CREATEUSERID))
            {
                this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            }
            if (string.IsNullOrEmpty(this.CREATEUSERNAME))
            {
                this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
            }
            if (string.IsNullOrEmpty(this.CREATEUSERDEPTCODE))
            {
                this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            }
            if (string.IsNullOrEmpty(this.CREATEUSERORGCODE))
            {
                this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
            }
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