using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：起重吊装作业安全措施
    /// </summary>
    [Table("BIS_LIFTHOISTSAFETY")]
    public class LifthoistsafetyEntity : BaseEntity
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
        [JsonIgnore]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        [JsonIgnore]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        [JsonIgnore]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        [JsonIgnore]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        [JsonIgnore]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        [JsonIgnore]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODITYUSERID")]
        [JsonIgnore]
        public string MODITYUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODITYUSERNAME")]
        [JsonIgnore]
        public string MODITYUSERNAME { get; set; }
        /// <summary>
        /// 起吊证ID
        /// </summary>
        /// <returns></returns>
        [Column("LIFTHOISTCERTID")]
        public string LIFTHOISTCERTID { get; set; }
        /// <summary>
        /// 项名
        /// </summary>
        /// <returns></returns>
        [Column("ITEMNAME")]
        public string ITEMNAME { get; set; }
        /// <summary>
        /// 项值
        /// </summary>
        /// <returns></returns>
        [Column("ITEMVALUE")]
        public string ITEMVALUE { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column("SORTNUM")]
        public int? SORTNUM { get; set; }
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
            this.MODITYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODITYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}