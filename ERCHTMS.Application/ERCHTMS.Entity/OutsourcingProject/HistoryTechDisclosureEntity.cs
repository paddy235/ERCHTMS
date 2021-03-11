using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全技术交底
    /// </summary>
    [Table("EPG_HISTORYTECHDISCLOSURE")]
    public class HistoryTechDisclosureEntity : BaseEntity
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
        /// 交底名称
        /// </summary>
        /// <returns></returns>
        [Column("DISCLOSURENAME")]
        public string DISCLOSURENAME { get; set; }
        /// <summary>
        /// 交底类型
        /// </summary>
        /// <returns></returns>
        [Column("DISCLOSURETYPE")]
        public string DISCLOSURETYPE { get; set; }
        /// <summary>
        /// 交底时间
        /// </summary>
        /// <returns></returns>
        [Column("DISCLOSUREDATE")]
        public DateTime? DISCLOSUREDATE { get; set; }
        /// <summary>
        /// 交底地点
        /// </summary>
        /// <returns></returns>
        [Column("DISCLOSUREPLACE")]
        public string DISCLOSUREPLACE { get; set; }
        /// <summary>
        /// 交底人
        /// </summary>
        /// <returns></returns>
        [Column("DISCLOSUREPERSON")]
        public string DISCLOSUREPERSON { get; set; }
        /// <summary>
        /// 交底人ID
        /// </summary>
        /// <returns></returns>
        [Column("DISCLOSUREPERSONID")]
        public string DISCLOSUREPERSONID { get; set; }
        /// <summary>
        /// 接收交底人
        /// </summary>
        /// <returns></returns>
        [Column("RECEIVEPERSON")]
        public string RECEIVEPERSON { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("RECEIVEPERSONID")]
        public string RECEIVEPERSONID { get; set; }
        /// <summary>
        /// 交底人数
        /// </summary>
        /// <returns></returns>
        [Column("DISCLOSUREPERSONNUM")]
        public string DISCLOSUREPERSONNUM { get; set; }
        /// <summary>
        /// 工程ID
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTID")]
        public string PROJECTID { get; set; }
        /// <summary>
        /// 交底内容
        /// </summary>
        /// <returns></returns>
        [Column("DISCLOSURECONTENT")]
        public string DISCLOSURECONTENT { get; set; }

        /// <summary>
        /// 外包工程名称
        /// </summary>
        [Column("ENGINEERNAME")]
        public string ENGINEERNAME { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        [Column("ENGINEERCODE")]
        public string ENGINEERCODE { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        [Column("ENGINEERTYPE")]
        public string ENGINEERTYPE { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        [Column("ENGINEERAREANAME")]
        public string EngAreaName { get; set; }

        /// <summary>
        /// 区域Id
        /// </summary>
        [Column("ENGINEERAREA")]
        public string ENGINEERAREA { get; set; }

        /// <summary>
        /// 项目级别
        /// </summary>
        [Column("ENGINEERLEVEL")]
        public string ENGINEERLEVEL { get; set; }
        
        /// <summary>
        /// 项目责任单位id
        /// </summary>
        [Column("ENGINEERLETDEPT")]
        public string ENGINEERLETDEPT { get; set; }

        /// <summary>
        /// 项目责任单位名称
        /// </summary>
        [Column("ENGINEERLETDEPTNAME")]
        public string ENGINEERLETDEPTNAME { get; set; }

        /// <summary>
        /// 项目单位编码
        /// </summary>
        [Column("ENGINEERLETDEPTCODE")]
        public string ENGINEERLETDEPTCODE { get; set; }

        /// <summary>
        /// 项目工作内容
        /// </summary>
        [Column("ENGINEERCONTENT")]
        public string ENGINEERCONTENT { get; set; }

        /// <summary>
        /// 交底专业
        /// </summary>
        [Column("DISCLOSUREMAJOR")]
        public string DISCLOSUREMAJOR { get; set; }

        /// <summary>
        /// 交底部门
        /// </summary>
        [Column("DISCLOSUREMAJORDEPT")]
        public string DISCLOSUREMAJORDEPT { get; set; }

        /// <summary>
        /// 交底部门Id
        /// </summary>
        [Column("DISCLOSUREMAJORDEPTID")]
        public string DISCLOSUREMAJORDEPTID { get; set; }

        /// <summary>
        /// 交底部门Code
        /// </summary>
        [Column("DISCLOSUREMAJORDEPTCODE")]
        public string DISCLOSUREMAJORDEPTCODE { get; set; }

        /// <summary>
        /// 是否提交  0：保存 1：提交
        /// </summary>
        [Column("ISSUBMIT")]
        public int? ISSUBMIT { get; set; }

        /// <summary>
        /// 状态值 0：保存 1：审核中 2：审核不通过  3：审核通过
        /// </summary>
        [Column("STATUS")]
        public int? STATUS { get; set; }

        /// <summary>
        /// 流程节点id
        /// </summary>
        [Column("FLOWID")]
        public string FLOWID { get; set; }

        /// <summary>
        /// 关联安全技术交底主记录
        /// </summary>
        [Column("RECID")]
        public string RecId { get; set; }
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