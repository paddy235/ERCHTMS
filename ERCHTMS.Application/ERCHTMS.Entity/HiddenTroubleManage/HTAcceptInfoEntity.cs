using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患验收信息表
    /// </summary>
    [Table("BIS_HTACCEPTINFO")]
    public class HTAcceptInfoEntity : BaseEntity
    {
        #region 实体成员
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
        ///  是否省级验收
        /// </summary>
        /// <returns></returns>
        [Column("ISUPACCEPT")]
        public string ISUPACCEPT { get; set; }
        /// <summary>
        /// 验收单位编码
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTDEPARTCODE")]
        public string ACCEPTDEPARTCODE { get; set; }
        /// <summary>
        /// 验收单位名称
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTDEPARTNAME")]
        public string ACCEPTDEPARTNAME { get; set; }
        /// <summary>
        /// 验收人
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTPERSON")]
        public string ACCEPTPERSON { get; set; }
        /// <summary>
        /// 验收人姓名
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTPERSONNAME")]
        public string ACCEPTPERSONNAME { get; set; }
        /// <summary>
        /// 验收日期
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTDATE")]
        public DateTime? ACCEPTDATE { get; set; }
        /// <summary>
        /// 验收情况
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTSTATUS")]
        public string ACCEPTSTATUS { get; set; }
        /// <summary>
        /// 未验收通过相片
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTPHOTO")]
        public string ACCEPTPHOTO { get; set; }
        /// <summary>
        /// 验收意见
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTIDEA")]
        public string ACCEPTIDEA { get; set; }
        /// <summary>
        /// 销号日期
        /// </summary>
        /// <returns></returns>
        [Column("DAMAGEDATE")]
        public DateTime? DAMAGEDATE { get; set; }
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
        /// 自动增量
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int AUTOID { get; set; }


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
            this.ID = Guid.NewGuid().ToString();
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