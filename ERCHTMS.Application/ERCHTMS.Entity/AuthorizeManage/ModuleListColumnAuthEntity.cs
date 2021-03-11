using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.AuthorizeManage
{
    /// <summary>
    /// 描 述：应用模块列表的列查看权限设置表
    /// </summary>
    [Table("BASE_MODULELISTCOLUMNAUTH")]
    public class ModuleListColumnAuthEntity : BaseEntity
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
        /// 模块id
        /// </summary>
        /// <returns></returns>
        [Column("MODULEID")]
        public string MODULEID { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        /// <returns></returns>
        [Column("MODULENAME")]
        public string MODULENAME { get; set; }
        /// <summary>
        /// 列表主键字段
        /// </summary>
        /// <returns></returns>
        [Column("LISTKEYFIELD")]
        public string LISTKEYFIELD { get; set; } 
        /// <summary>
        /// 列表中的列名集合项listkeyfield
        /// </summary>
        /// <returns></returns>
        [Column("LISTCOLUMNNAME")]
        public string LISTCOLUMNNAME { get; set; }
        /// <summary>
        /// 列表中的列字段集合项
        /// </summary>
        /// <returns></returns>
        [Column("LISTCOLUMNFIELDS")]
        public string LISTCOLUMNFIELDS { get; set; }
        /// <summary>
        /// 默认列表中的列名集合项
        /// </summary>
        /// <returns></returns>
        [Column("DEFAULTCOLUMNNAME")]
        public string DEFAULTCOLUMNNAME { get; set; }
        /// <summary>
        /// 默认列表中的列字段集合项
        /// </summary>
        /// <returns></returns>
        [Column("DEFAULTCOLUMNFIELDS")]
        public string DEFAULTCOLUMNFIELDS { get; set; }
        /// <summary>
        /// 归属用户id
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string USERID { get; set; }
        /// <summary>
        /// 归属用户名称
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string USERNAME { get; set; }
        /// <summary>
        /// 列表状态
        /// </summary>
        /// <returns></returns>
        [Column("LISTTYPE")]
        public int? LISTTYPE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string REMARK { get; set; }
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