using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急演练
    /// </summary>
    [Table("MAE_DRILLPLAN")]
    public class DrillplanEntity : BaseEntity
    {
        #region 实体成员

        //预案Id
        [Column("RPLANID")]
        public string RPLANID { get; set; }

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
        [Column("NAME")]
        public string NAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTID")]
        public string DEPARTID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTNAME")]
        public string DEPARTNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DRILLTYPE")]
        public string DRILLTYPE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DRILLMODE")]
        public string DRILLMODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("PLANTIME")]
        public DateTime? PLANTIME { get; set; }
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
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
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
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DRILLTYPENAME")]
        public string DRILLTYPENAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DRILLMODENAME")]
        public string DRILLMODENAME { get; set; }
        /// <summary>
        /// 执行人id
        /// </summary>
        /// <returns></returns>
        [Column("EXECUTEPERSONID")]
        public string EXECUTEPERSONID { get; set; }
        /// <summary>
        /// 执行人姓名
        /// </summary>
        /// <returns></returns>
        [Column("EXECUTEPERSONNAME")]
        public string EXECUTEPERSONNAME { get; set; }
        /// <summary>
        /// 组织部门Id
        /// </summary>
        [Column("ORGDEPTID")]
        public string OrgDeptId { get; set; }
        /// <summary>
        /// 组织部门
        /// </summary>
        [Column("ORGDEPT")]
        public string OrgDept { get; set; }
        /// <summary>
        /// 组织部门CODE
        /// </summary>
        [Column("ORGDEPTCODE")]
        public string OrgDeptCode { get; set; }
        /// <summary>
        /// 演练计划费用
        /// </summary>
        [Column("DRILLCOST")]
        public string DrillCost { get; set; }
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