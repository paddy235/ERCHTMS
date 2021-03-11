using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.NosaManage
{
    /// <summary>
    /// 描 述：工作任务
    /// </summary>
    [Table("HRS_NOSAWORKS")]
    public class NosaworksEntity : BaseEntity
    {
        #region 默认字段
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
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
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
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        #endregion

        #region 实体成员
        /// <summary>
        /// 元素负责人id
        /// </summary>
        /// <returns></returns>
        [Column("ELEDUTYUSERID")]
        public string EleDutyUserId { get; set; }
        /// <summary>
        /// 已提交责任人姓名
        /// </summary>
        /// <returns></returns>
        [Column("SUBMITUSERNAME")]
        public string SubmitUserName { get; set; }
        /// <summary>
        /// 责任人id
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        /// 元素负责人名称
        /// </summary>
        /// <returns></returns>
        [Column("ELEDUTYUSERNAME")]
        public string EleDutyUserName { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        /// <returns></returns>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// 频次
        /// </summary>
        /// <returns></returns>
        [Column("RATENUM")]
        public string RateNum { get; set; }
        /// <summary>
        /// 已提交责任人id
        /// </summary>
        /// <returns></returns>
        [Column("SUBMITUSERID")]
        public string SubmitUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ELENO")]
        public string EleNo { get; set; }
        /// <summary>
        /// 任务结束时间
        /// </summary>
        /// <returns></returns>
        [Column("ENDDATE")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 元素责任部门id
        /// </summary>
        /// <returns></returns>
        [Column("ELEDUTYDEPARTID")]
        public string EleDutyDepartId { get; set; }
        /// <summary>
        /// 依据
        /// </summary>
        /// <returns></returns>
        [Column("ACCORDING")]
        public string According { get; set; }
        /// <summary>
        /// 元素id
        /// </summary>
        /// <returns></returns>
        [Column("ELEID")]
        public string EleId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 建议
        /// </summary>
        /// <returns></returns>
        [Column("ADVISE")]
        public string Advise { get; set; }
        /// <summary>
        /// 元素名称
        /// </summary>
        /// <returns></returns>
        [Column("ELENAME")]
        public string EleName { get; set; }
        /// <summary>
        /// 元素责任名称
        /// </summary>
        /// <returns></returns>
        [Column("ELEDUTYDEPARTNAME")]
        public string EleDutyDepartName { get; set; }
        /// <summary>
        /// 责任人姓名
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERNAME")]
        public string DutyUserName { get; set; }
        /// <summary>
        /// 责任部门名称
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPARTNAME")]
        public string DutyDepartName { get; set; }
        /// <summary>
        /// 责任人姓名html
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERHTML")]
        public string DutyUserHtml { get; set; }
        /// <summary>
        /// 责任部门名称html
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPARTHTML")]
        public string DutyDepartHtml { get; set; }
        /// <summary>
        /// 责任部门id
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPARTID")]
        public string DutyDepartId { get; set; }
        /// <summary>
        /// 是否提交（值：是、否）
        /// </summary>
        /// <returns></returns>
        [Column("ISSUBMITED")]
        public string IsSubmited { get; set; }
        /// <summary>
        /// 完成进度
        /// </summary>
        /// <returns></returns>
        [Column("PCT")]
        public decimal? Pct { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = this.CREATEDATE.HasValue ? this.CREATEDATE : DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
            this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
            this.DutyUserHtml = this.DutyUserName;
            this.DutyDepartHtml = this.DutyDepartName;
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