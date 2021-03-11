using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.StandardSystem
{
    /// <summary>
    /// 描 述：标准体系
    /// </summary>
    [Table("HRS_STANDARDSYSTEM")]
    public class StandardsystemEntity : BaseEntity
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
        /// 文件名称
        /// </summary>
        /// <returns></returns>
        [Column("FILENAME")]
        public string FILENAME { get; set; }
        /// <summary>
        /// 类别编号
        /// </summary>
        /// <returns></returns>
        [Column("CATEGORYCODE")]
        public string CATEGORYCODE { get; set; }
        /// <summary>
        /// 岗位编码
        /// </summary>
        /// <returns></returns>
        [Column("STATIONID")]
        public string STATIONID { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        /// <returns></returns>
        [Column("STATIONNAME")]
        public string STATIONNAME { get; set; }
        /// <summary>
        /// 对应元素
        /// </summary>
        /// <returns></returns>
        [Column("RELEVANTELEMENTNAME")]
        public string RELEVANTELEMENTNAME { get; set; }
        /// <summary>
        /// 对应元素
        /// </summary>
        /// <returns></returns>
        [Column("RELEVANTELEMENTID")]
        public string RELEVANTELEMENTID { get; set; }
        /// <summary>
        /// 实施日期
        /// </summary>
        /// <returns></returns>
        [Column("CARRYDATE")]
        public DateTime? CARRYDATE { get; set; }
        /// <summary>
        /// 标准类别
        /// </summary>
        /// <returns></returns>
        [Column("STANDARDTYPE")]
        public string STANDARDTYPE { get; set; }

        /// <summary>
        /// 查阅频次
        /// </summary>
        [Column("CONSULTNUM")]
        public int CONSULTNUM { get; set; }
        /// <summary>
        /// 发文字号/编号
        /// </summary>
        /// <returns></returns>
        [Column("DISPATCHCODE")]
        public string DISPATCHCODE { get; set; }
        /// <summary>
        /// 颁布部门
        /// </summary>
        /// <returns></returns>
        [Column("PUBLISHDEPT")]
        public string PUBLISHDEPT { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string CATEGORYNAME { get; set; }

        /// <summary>
        /// 发布部门名称
        /// </summary>
        public string CREATEUSERDEPTNAME { get; set; }

        /// <summary>
        /// 时效性编码
        /// </summary>
        public string TIMELINESS { get; set; }

        /// <summary>
        /// 时效性名称
        /// </summary>
        public string TIMELINESSNAME { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID)?Guid.NewGuid().ToString(): ID;
            this.CONSULTNUM = 0;
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