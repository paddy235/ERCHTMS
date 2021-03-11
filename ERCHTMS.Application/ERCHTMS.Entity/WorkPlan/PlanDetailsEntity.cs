using ERCHTMS.Code;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERCHTMS.Entity.WorkPlan
{
    /// <summary>
    /// 描 述：EHS计划申请明细表
    /// </summary>
    [Table("HRS_PLANDETAILS")]
    public class PlanDetailsEntity : BaseEntity
    {
        #region 默认字段
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
        #endregion

        #region 实体成员 
        /// <summary>
        /// 工作内容
        /// </summary>
        [Column("CONTENTS")]
        public string Contents { get; set; }
        /// <summary>
        /// 责任人id
        /// </summary>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        /// 责任人姓名
        /// </summary>
        [Column("DUTYUSERNAME")]
        public string DutyUserName { get; set; }
        /// <summary>
        /// 责任部门id
        /// </summary>
        [Column("DUTYDEPARTID")]
        public string DutyDepartId { get; set; }
        /// <summary>
        /// 责任部门
        /// </summary>
        [Column("DUTYDEPARTNAME")]
        public string DutyDepartName { get; set; }
        /// <summary>
        /// 计划完成日期
        /// </summary>
        [Column("PLANFINDATE")]
        public DateTime? PlanFinDate { get; set; }
        /// <summary>
        /// 实际完成日期
        /// </summary>
        [Column("REALFINDATE")]
        public DateTime? RealFinDate { get; set; }
        /// <summary>
        /// 管理标准id
        /// </summary>
        [Column("STDID")]
        public string StdId { get; set; }
        /// <summary>
        /// 管理标准名称
        /// </summary>
        [Column("STDNAME")]
        public string StdName { get; set; }
        /// <summary>
        /// 是否取消计划
        /// </summary>
        [Column("ISCANCEL")]
        public string IsCancel { get; set; }
        /// <summary>
        /// 变动原因
        /// </summary>
        [Column("CHANGEREASON")]
        public string ChangeReason { get; set; }
        /// <summary>
        /// 计划id
        /// </summary>
        [Column("APPLYID")]
        public string ApplyId { get; set; }
        /// <summary>
        /// 引用记录id
        /// </summary>
        [Column("BASEID")]
        public string BaseId { get; set; }
        /// <summary>
        /// 记录是否变化
        /// </summary>
        [Column("ISCHANGED")]
        public int IsChanged { get; set; }
        /// <summary>
        /// 标记
        /// </summary>
        [Column("ISMARK")]
        public int IsMark { get; set; }
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
            //this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
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