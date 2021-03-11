using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：五定安全检查
    /// </summary>
    [Table("BIS_FIVESAFETYCHECK")]
    public class FivesafetycheckEntity : BaseEntity
    {
        #region 实体成员
       
        /// <summary>
        /// 检查区域
        /// </summary>
        /// <returns></returns>
        [Column("CHECKAREANAME")]
        public string CHECKAREANAME { get; set; }
        /// <summary>
        /// 检查组组长ID
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMANAGEMANID")]
        public string CHECKMANAGEMANID { get; set; }
        /// <summary>
        /// 检查组组长
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMANAGEMAN")]
        public string CHECKMANAGEMAN { get; set; }
        /// <summary>
        /// 检查组员id 多个逗号分隔
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERSID")]
        public string CHECKUSERSID { get; set; }
        /// <summary>
        /// 检查组员
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERS")]
        public string CHECKUSERS { get; set; }
        /// <summary>
        /// 检查部门ID 多个逗号分隔
        /// </summary>
        /// <returns></returns>
        [Column("CHECKEDDEPARTID")]
        public string CHECKEDDEPARTID { get; set; }
        /// <summary>
        /// 检查部门名称
        /// </summary>
        /// <returns></returns>
        [Column("CHECKEDDEPART")]
        public string CHECKEDDEPART { get; set; }
        /// <summary>
        /// 检查级别ID
        /// </summary>
        /// <returns></returns>
        [Column("CHECKLEVELID")]
        public string CHECKLEVELID { get; set; }
        /// <summary>
        /// 检查级别名称
        /// </summary>
        /// <returns></returns>
        [Column("CHECKLEVEL")]
        public string CHECKLEVEL { get; set; }
        /// <summary>
        /// 检查类型id
        /// </summary>
        /// <returns></returns>
        [Column("CHECKTYPEID")]
        public string CHECKTYPEID { get; set; }
        /// <summary>
        /// 检查类型
        /// </summary>
        /// <returns></returns>
        [Column("CHECKTYPE")]
        public string CHECKTYPE { get; set; }
        /// <summary>
        /// 检查结束时间
        /// </summary>
        /// <returns></returns>
        [Column("CHECKENDDATE")]
        public DateTime? CHECKENDDATE { get; set; }
        /// <summary>
        /// 检查开始时间
        /// </summary>
        /// <returns></returns>
        [Column("CHECKBEGINDATE")]
        public DateTime? CHECKBEGINDATE { get; set; }
        /// <summary>
        /// 检查名称
        /// </summary>
        /// <returns></returns>
        [Column("CHECKNAME")]
        public string CHECKNAME { get; set; }
        /// <summary>
        /// 流程完成情况
        /// </summary>
        /// <returns></returns>
        [Column("ISOVER")]
        public int? ISOVER { get; set; }
        /// <summary>
        /// 是否保存成功
        /// </summary>
        /// <returns></returns>
        [Column("ISSAVED")]
        public int? ISSAVED { get; set; }
        /// <summary>
        /// 流程名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWNAME")]
        public string FLOWNAME { get; set; }
        /// <summary>
        /// 流程角色编码/ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLE")]
        public string FLOWROLE { get; set; }
        /// <summary>
        /// 流程角色名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLENAME")]
        public string FLOWROLENAME { get; set; }
        /// <summary>
        /// 流程部门编码/ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPT")]
        public string FLOWDEPT { get; set; }
        /// <summary>
        /// 流程部门名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPTNAME")]
        public string FLOWDEPTNAME { get; set; }
        /// <summary>
        /// 流程Id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FLOWID { get; set; }
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
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
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
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
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
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// 会签部门人员ID 多个逗号分隔
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTALL")]
        public string CHECKDEPTALL { get; set; }

        /// <summary>
        /// 会签部门ID 多个逗号分隔
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERALL")]
        public string CHECKUSERALL { get; set; }

        /// <summary>
        /// 检查部门ID 多个逗号分隔(脚本使用，防止后续需要找上级，直接修改这里即可)
        /// </summary>
        /// <returns></returns>
        [Column("CHECKEDDEPARTIDALL")]
        public string CHECKEDDEPARTIDALL { get; set; }
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