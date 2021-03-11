using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：流程转向配置实例表
    /// </summary>
    [Table("BIS_WFSETTING")]
    public class WfSettingEntity : BaseEntity
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
        /// 流程配置实例ID(FK)
        /// </summary>
        /// <returns></returns>
        [Column("INSTANCEID")]
        public string INSTANCEID { get; set; }
        /// <summary>
        /// 流程转向配置名
        /// </summary>
        /// <returns></returns>
        [Column("SETTINGNAME")]
        public string SETTINGNAME { get; set; }
        /// <summary>
        /// 起始流程
        /// </summary>
        /// <returns></returns>
        [Column("STARTFLOW")]
        public string STARTFLOW { get; set; }
        /// <summary>
        /// 目标流程
        /// </summary>
        /// <returns></returns>
        [Column("ENDFLOW")]
        public string ENDFLOW { get; set; }
        /// <summary>
        /// 提交形式(提交、上报、退回)
        /// </summary>
        /// <returns></returns>
        [Column("SUBMITTYPE")]
        public string SUBMITTYPE { get; set; }
        /// <summary>
        /// 是否自动处理当前流程(是、否)
        /// </summary>
        /// <returns></returns>
        [Column("ISAUTOHANDLE")]
        public string ISAUTOHANDLE { get; set; }
        /// <summary>
        /// 是否更改流程状态
        /// </summary>
        /// <returns></returns>
        [Column("ISUPDATEFLOW")]
        public string ISUPDATEFLOW { get; set; }
        /// <summary>
        /// 更改流程Flag序号
        /// </summary>
        /// <returns></returns>
        [Column("WFFLAG")]
        public string WFFLAG { get; set; }
        /// <summary>
        /// 是否需要执行脚本获取目标参与者(是、否)
        /// </summary>
        /// <returns></returns>
        [Column("ISEXCUTESQL")]
        public string ISEXCUTESQL { get; set; }
        /// <summary>
        /// 脚本语言(用于业务处理中，获取对应参与人的脚本语句，select changeperson  from ... where id ={0})
        /// </summary>
        /// <returns></returns>
        [Column("SCRIPTCONTENT")]
        public string SCRIPTCONTENT { get; set; }

        /// <summary>
        /// 是否需要执行脚本获取目标参与者(是、否)(起始流程)
        /// </summary>
        /// <returns></returns>
        [Column("ISEXCUTECURSQL")]
        public string ISEXCUTECURSQL { get; set; }
        /// <summary>
        /// 脚本语言(用于业务处理中，获取对应参与人的脚本语句，select changeperson  from ... where id ={0})(起始流程)
        /// </summary>
        /// <returns></returns>
        [Column("SCRIPTCURCONTENT")]
        public string SCRIPTCURCONTENT { get; set; } 
         /// <summary>
        /// 是否结束流程
        /// </summary>
        /// <returns></returns>
        [Column("ISENDPOINT")]
        public string ISENDPOINT { get; set; }
        /// <summary>
        /// 顺序号
        /// </summary>
        /// <returns></returns>
        [Column("SERIALNUMBER")]
        public int? SERIALNUMBER { get; set; }
        /// <summary>
        /// 起始优先级
        /// </summary>
        /// <returns></returns>
        [Column("STARTLEVEL")]
        public string STARTLEVEL { get; set; }
        /// <summary>
        /// 目标优先级
        /// </summary>
        /// <returns></returns>
        [Column("ENDLEVEL")]
        public string ENDLEVEL { get; set; } 
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARKS")]
        public string REMARKS { get; set; }
        /// <summary>
        /// 流程项目编码
        /// </summary>
        /// <returns></returns>
        [Column("FLOWCODE")]
        public string FLOWCODE { get; set; }
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