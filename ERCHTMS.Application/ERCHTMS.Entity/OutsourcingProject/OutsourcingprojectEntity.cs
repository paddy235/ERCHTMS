using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包单位基础信息表
    /// </summary>
    [Table("EPG_OUTSOURCINGPROJECT")]
    public class OutsourcingprojectEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 法人代表传真
        /// </summary>
        /// <returns></returns>
        [Column("LEGALREPFAX")]
        public string LEGALREPFAX { get; set; }
        /// <summary>
        /// 服务结束时间(最后一个工程的实际完工时间)
        /// </summary>
        /// <returns></returns>
        [Column("SERVICESENDTIME")]
        public DateTime? SERVICESENDTIME { get; set; }
        /// <summary>
        /// 联系人传真
        /// </summary>
        /// <returns></returns>
        [Column("LINKMANFAX")]
        public string LINKMANFAX { get; set; }
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
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 外包单位名称
        /// </summary>
        /// <returns></returns>
        [Column("OUTSOURCINGNAME")]
        public string OUTSOURCINGNAME { get; set; }
        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        /// <returns></returns>
        [Column("CREDITCODE")]
        public string CREDITCODE { get; set; }
        /// <summary>
        /// 入离场状态：0：入场 1：离场
        /// </summary>
        /// <returns></returns>
        [Column("OUTORIN")]
        public string OUTORIN { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        /// <returns></returns>
        [Column("EMAIL")]
        public string EMAIL { get; set; }
        /// <summary>
        /// 单位Id
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTID")]
        public string OUTPROJECTID { get; set; }
        /// <summary>
        /// 外包单位概况
        /// </summary>
        /// <returns></returns>
        [Column("GENERALSITUATION")]
        public string GENERALSITUATION { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 法人代表电话
        /// </summary>
        /// <returns></returns>
        [Column("LEGALREPPHONE")]
        public string LEGALREPPHONE { get; set; }
        /// <summary>
        /// 服务开始时间(第一个工程的实际开工时间)
        /// </summary>
        /// <returns></returns>
        [Column("SERVICESSTARTTIME")]
        public DateTime? SERVICESSTARTTIME { get; set; }
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
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 法人代表
        /// </summary>
        /// <returns></returns>
        [Column("LEGALREP")]
        public string LEGALREP { get; set; }
        /// <summary>
        /// 黑名单状态：0：否 1 是 
        /// </summary>
        /// <returns></returns>
        [Column("BLACKLISTSTATE")]
        public string BLACKLISTSTATE { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        /// <returns></returns>
        [Column("LINKMAN")]
        public string LINKMAN { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        /// <returns></returns>
        [Column("LINKMANPHONE")]
        public string LINKMANPHONE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 企业地址
        /// </summary>
        /// <returns></returns>
        [Column("ADDRESS")]
        public string ADDRESS { get; set; }
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
        /// 离场时间
        /// </summary>
        /// <returns></returns>
        [Column("LEAVETIME")]
        public DateTime? LEAVETIME { get; set; }
        /// <summary>
        /// 入场时间
        /// </summary>
        /// <returns></returns>
        [Column("OUTINTIME")]
        public DateTime? OUTINTIME { get; set; }

        /// <summary>
        /// 合同人数
        /// </summary>
        [Column("CONTRACTPERSONNUM")]
        public int? ContractPersonNum { get; set; }
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