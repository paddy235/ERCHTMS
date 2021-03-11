using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SystemManage
{
    /// <summary>
    /// 描 述：流程业务实例
    /// </summary>
    [Table("SYS_WFTBINSTANCE")]
    public class WfTBInstanceEntity : BaseEntity
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
        [Column("OPERDATE")]
        public DateTime? OPERDATE { get; set; }
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
        [Column("OPERUSER")]
        public string OPERUSER { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSER")]
        public string CREATEUSER { get; set; }
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
        [Column("INSTANCECONTENT")]
        public string INSTANCECONTENT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("OBJECTID")]
        public string OBJECTID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("STATE")]
        public int? STATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("PRIORITY")]
        public int? PRIORITY { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CURRENTDETAILID")]
        public string CURRENTDETAILID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("PARENTID")]
        public string PARENTID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("PROCESSID")]
        public string PROCESSID { get; set; }
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
        [Column("CREATEUSERDEPTID")]
        public string CREATEUSERDEPTID { get; set; }
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
        [Column("OPERATEDUSER")]
        public string OPERATEDUSER { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DATAFROM")]
        public string DATAFROM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("UPLOADDATE")]
        public DateTime? UPLOADDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("COMMONDATE")]
        public DateTime? COMMONDATE { get; set; }
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
            this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            this.CREATEUSERDEPTID = OperatorProvider.Provider.Current().DeptId;
            this.CREATEUSER = OperatorProvider.Provider.Current().Account;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.OPERUSER = OperatorProvider.Provider.Current().Account;
            this.OPERDATE = DateTime.Now;
            this.ID = keyValue;
        }
        #endregion
    }
}