using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：流程配置条件附加表
    /// </summary>
    [Table("BIS_WFCONDITIONADDTION")]
    public class WfConditionAddtionEntity : BaseEntity
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
        /// 流程配置条件ID(FK)
        /// </summary>
        /// <returns></returns>
        [Column("WFCONDITIONID")]
        public string WFCONDITIONID { get; set; }
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
        /// 指定部门名称
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DEPTNAME { get; set; }
        /// <summary>
        /// 指定部门id
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DEPTID { get; set; }
        /// <summary>
        /// 指定部门编码
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DEPTCODE { get; set; }
        /// <summary>
        /// 是否指定部门角色
        /// </summary>
        /// <returns></returns>
        [Column("ISHROLE")]
        public string ISHROLE { get; set; }
        /// <summary>
        /// 指定部门角色名称
        /// </summary>
        /// <returns></returns>
        [Column("ROLENAME")]
        public string ROLENAME { get; set; }
        /// <summary>
        /// 指定部门角色编码
        /// </summary>
        /// <returns></returns>
        [Column("ROLECODE")]
        public string ROLECODE { get; set; }
        /// <summary>
        /// 是否指定人员
        /// </summary>
        /// <returns></returns>
        [Column("ISHUSER")]
        public string ISHUSER { get; set; }
        /// <summary>
        /// 指定的人员名称
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string USERNAME { get; set; }
        /// <summary>
        /// 指定人员的账户
        /// </summary>
        /// <returns></returns>
        [Column("USERACCOUNT")]
        public string USERACCOUNT { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARKS")]
        public string REMARKS { get; set; }
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