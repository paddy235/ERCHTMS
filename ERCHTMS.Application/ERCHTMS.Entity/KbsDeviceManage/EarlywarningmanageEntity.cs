using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.KbsDeviceManage
{
    /// <summary>
    /// 描 述：预警类型管理
    /// </summary>
    [Table("BIS_EARLYWARNINGMANAGE")]
    public class EarlywarningmanageEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 备用
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string REMARK { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 是否启动
        /// </summary>
        /// <returns></returns>
        [Column("ISENABLE")]
        public int? Isenable { get; set; }
        /// <summary>
        /// 预警结果
        /// </summary>
        /// <returns></returns>
        [Column("WARNINGRESULT")]
        public string Warningresult { get; set; }
        /// <summary>
        /// 指标单位
        /// </summary>
        /// <returns></returns>
        [Column("INDEXUNIT")]
        public string Indexunit { get; set; }
        /// <summary>
        /// 预警条件
        /// </summary>
        /// <returns></returns>
        [Column("CONDITION")]
        public string Condition { get; set; }
        /// <summary>
        /// 条件指标
        /// </summary>
        /// <returns></returns>
        [Column("INDEXNOM")]
        public int? Indexnom { get; set; }
         /// <summary>
        /// 函数名
        /// </summary>
        /// <returns></returns>
        [Column("FUNCTIONNAME")]
        public string FunctionName { get; set; }
        
     
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Isenable = 1;
            this.ID = string.IsNullOrEmpty(ID)?Guid.NewGuid().ToString():ID;
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
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
        }
        #endregion
    }
}