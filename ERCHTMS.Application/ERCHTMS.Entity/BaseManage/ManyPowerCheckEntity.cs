using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;
using ERCHTMS.Code;
using System.ComponentModel;

namespace ERCHTMS.Entity.BaseManage
{
    /// <summary>
    /// 描 述：用户管理
    /// </summary>
    public class ManyPowerCheckEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 序号数值(用于判定是否同时执行)
        /// </summary>
        [Column("SERIALNUM")]
        public int? SERIALNUM { get; set; }
        /// <summary>
        /// 流程节点名称
        /// </summary>
        [Column("FLOWNAME")]
        public string FLOWNAME { get; set; }
        /// <summary>
        /// 模块编号
        /// </summary>
        /// <returns></returns>
        [Column("MODULENO")]
        public string MODULENO { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        [Column("MODULENAME")]
        public string MODULENAME { get; set; }
        /// <summary>
        /// 审核部门编号
        /// </summary>
        [Column("CHECKDEPTID")]
        public string CHECKDEPTID { get; set; }
        /// <summary>
        /// 审核部门Code
        /// </summary>
        [Column("CHECKDEPTCODE")]
        public string CHECKDEPTCODE { get; set; }
        /// <summary>
        /// 审核部门名称
        /// </summary>
        [Column("CHECKDEPTNAME")]
        public string CHECKDEPTNAME { get; set; }
        /// <summary>
        /// 审核部门角色编号，多个用,隔开。
        /// </summary>
        [Column("CHECKROLEID")]
        public string CHECKROLEID { get; set; }
        /// <summary>
        /// 审核部门角色名称，多个用,隔开。
        /// </summary>
        [Column("CHECKROLENAME")]
        public string CHECKROLENAME { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string REMARK { get; set; }
        /// <summary>
        /// 主键
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
        [Column("BELONGMODULECODE")]
        public string BelongModuleCode { get; set; }
        [Column("BELONGMODULE")]
        public string BelongModule { get; set; }

        /// <summary>
        /// 查询审核人应用方式
        /// </summary>
        [Column("APPLYTYPE")]
        public string ApplyType { get; set; }

        /// <summary>
        /// 获取审核人执行脚本
        /// </summary>
        [Column("SCRIPTCURCONTENT")]
        public string ScriptCurcontent { get; set; }

        /// <summary>
        /// 指定审核人
        /// </summary>
        [Column("CHECKUSERNAME")]
        public string CheckUserName { get; set; }

        /// <summary>
        /// 指定审核人ID
        /// </summary>
        [Column("CHECKUSERID")]
        public string CheckUserId { get; set; }

        /// <summary>
        /// 指定审核人账号
        /// </summary>
        [Column("CHECKUSERACCOUNT")]
        public string CheckUserAccount { get; set; }

        /// <summary>
        /// 下一步审核流程
        /// </summary>
        [NotMapped]
        public ManyPowerCheckEntity NextStepFlowEntity { get; set; }

        /// <summary>
        /// 是否选择专业
        /// </summary>
        [Column("CHOOSEMAJOR")]
        public string ChooseMajor { get; set; }

        /// <summary>
        /// 专业类别
        /// </summary>
        [Column("SPECIALTYTYPE")]
        public string SpecialtyType { get; set; }

        /// <summary>
        /// 选择审核人标题
        /// </summary>
        [Column("CHOOSEPERSONTITLE")]
        public string ChoosePersonTitle { get; set; }

        /// <summary>
        /// 选择审核人提示语
        /// </summary>
        [Column("CHOOSEPERSONWARN")]
        public string ChoosePersonWarn { get; set; }

        /// <summary>
        /// 选择部门范围
        /// </summary>
        [Column("CHOOSEDEPTRANGE")]
        public string ChooseDeptRange { get; set; }
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