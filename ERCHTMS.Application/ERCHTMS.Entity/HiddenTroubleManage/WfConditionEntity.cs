using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：流程配置条件表
    /// </summary>
    [Table("BIS_WFCONDITION")]
    public class WfConditionEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 流程配置附加说明
        /// </summary>
        /// <returns></returns>
        [Column("EXPLAINS")]
        public string EXPLAINS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
        /// <summary>
        /// 当前选择的部门形式(本部门、本单位、上级部门、上一级流程参与部门、创建部门、指定部门)
        /// </summary>
        /// <returns></returns>
        [Column("CHOOSETYPE")]
        public string CHOOSETYPE { get; set; }
        /// <summary>
        /// 部门性质(班组、专业、部门、厂级、公司级、省级、所有)
        /// </summary>
        /// <returns></returns>
        [Column("DEPTTYPE")]
        public string DEPTTYPE { get; set; }
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
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
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
        /// 角色性质(普通人员、专工、安全员、负责人、安全领导、所有)
        /// </summary>
        /// <returns></returns>
        [Column("ROLETYPE")]
        public string ROLETYPE { get; set; }
        /// <summary>
        /// 角色性质(普通人员、专工、安全员、负责人、安全领导、所有)
        /// </summary>
        /// <returns></returns>
        [Column("ROLECODE")]
        public string ROLECODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 所属机构单位
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZEID")]
        public string ORGANIZEID { get; set; }
        /// <summary>
        /// 所属机构单位
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZENAME")]
        public string ORGANIZENAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 流程转向类型(起始流程、目标流程) 
        /// </summary>
        /// <returns></returns>
        [Column("SETTINGTYPE")]
        public string SETTINGTYPE { get; set; }
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
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 角色性质判定规则
        /// </summary>
        /// <returns></returns>
        [Column("ROLERULE")]
        public string ROLERULE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARKS")]
        public string REMARKS { get; set; }
        /// <summary>
        /// 流程转向配置id(FK)
        /// </summary>
        /// <returns></returns>
        [Column("SETTINGID")]
        public string SETTINGID { get; set; }
        /// <summary>
        /// 是否执行脚本获取
        /// </summary> 
        /// <returns></returns>
        [Column("ISEXECSQL")]
        public string ISEXECSQL { get; set; }

        /// <summary>
        /// 要执行的脚本
        /// </summary> 
        /// <returns></returns>
        [Column("SQLCONTENT")]
        public string SQLCONTENT { get; set; }
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