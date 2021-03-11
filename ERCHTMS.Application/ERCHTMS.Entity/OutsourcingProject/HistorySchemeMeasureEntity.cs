using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：历史方案措施管理
    /// </summary>
    [Table("EPG_HISTORYSCHEMEMEASURE")]
    public class HistorySchemeMeasureEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }

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
        /// 是否保存
        /// </summary>
        /// <returns></returns>
        [Column("ISSAVED")]
        public string ISSAVED { get; set; }

        /// <summary>
        /// 流程完成情况
        /// </summary>
        /// <returns></returns>
        [Column("ISOVER")]
        public string ISOVER { get; set; }

        /// <summary>
        /// 编制人
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZER")]
        public string ORGANIZER { get; set; }
        /// <summary>
        /// 编制时间
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZTIME")]
        public DateTime? ORGANIZTIME { get; set; }

        /// <summary>
        /// 关联ID
        /// </summary>
        /// <returns></returns>
        [Column("CONTRACTID")]
        public string CONTRACTID { get; set; } 
        /// <summary>
        /// 工程ID
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTID")]
        public string PROJECTID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        /// <returns></returns>
        [Column("SUBMITDATE")]
        public DateTime? SUBMITDATE { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        /// <returns></returns>
        [Column("SUBMITPERSON")]
        public string SUBMITPERSON { get; set; }
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
        /// 提交内容
        /// </summary>
        /// <returns></returns>
        [Column("SUMMITCONTENT")]
        public string SummitContent { get; set; }

        /// <summary>
        /// 工程编号
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERCODE")]
        public string ENGINEERCODE { get; set; }
        /// <summary>
        /// 工程名称
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERNAME")]
        public string ENGINEERNAME { get; set; }
        /// <summary>
        /// 工程类型
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERTYPE")]
        public string ENGINEERTYPE { get; set; }
        /// <summary>
        /// 所属区域
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERAREANAME")]
        public string ENGINEERAREANAME { get; set; }
        /// <summary>
        /// 工程风险等级
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLEVEL")]
        public string ENGINEERLEVEL { get; set; }
        /// <summary>
        /// 责任部门名称
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETDEPT")]
        public string ENGINEERLETDEPT { get; set; }
        /// <summary>
        /// 责任部门id
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETDEPTID")]
        public string ENGINEERLETDEPTID { get; set; }
        /// <summary>
        /// 责任部门code
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETDEPTCODE")]
        public string ENGINEERLETDEPTCODE { get; set; }
        /// <summary>
        /// 所属区域id
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERAREA")]
        public string ENGINEERAREA { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERCONTENT")]
        public string ENGINEERCONTENT { get; set; }

        /// <summary>
        /// 所属专业
        /// </summary>
        /// <returns></returns>
        [Column("BELONGMAJOR")]
        public string BELONGMAJOR { get; set; }
        /// <summary>
        /// 所属责任部门
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTNAME")]
        public string BELONGDEPTNAME { get; set; }
        /// <summary>
        /// 所属责任部门ID
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTID")]
        public string BELONGDEPTID { get; set; }
        /// <summary>
        /// 所属责任部门code
        /// </summary>
        /// <returns></returns>
        [Column("BELONGCODE")]
        public string BELONGCODE { get; set; }
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
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}