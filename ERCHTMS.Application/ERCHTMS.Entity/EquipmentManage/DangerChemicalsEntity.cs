using ERCHTMS.Code;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERCHTMS.Entity.EquipmentManage
{
    /// <summary>
    /// 描 述：危险化学品库存
    /// </summary>
    [Table("XLD_DANGEROUSCHEMICAL")]
    public class DangerChemicalsEntity : BaseEntity
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
        /// 名称
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        [Column("ALIAS")]
        public string Alias { get; set; }
        /// <summary>
        /// CAS号
        /// </summary>
        [Column("CAS")]
        public string Cas { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        [Column("SPECIFICATION")]
        public string Specification { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        [Column("INVENTORY")]
        public string Inventory { get; set; }
        /// <summary>
        /// 库存单位
        /// </summary>
        [Column("UNIT")]
        public string Unit { get; set; }
        /// <summary>
        /// 危险品类型
        /// </summary>
        [Column("RISKTYPE")]
        public string RiskType { get; set; }
        /// <summary>
        /// 生产日期
        /// </summary>
        [Column("PRODUCTIONDATE")]
        public DateTime? ProductionDate { get; set; }
        /// <summary>
        /// 存放日期
        /// </summary>
        [Column("DEPOSITDATE")]
        public DateTime? DepositDate { get; set; }
        /// <summary>
        /// 存放地点
        /// </summary>
        [Column("SITE")]
        public string Site { get; set; }
        /// <summary>
        /// 存放期限
        /// </summary>
        [Column("DEADLINE")]
        public int? Deadline { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        [Column("DUTYUSER")]
        public string DutyUser { get; set; }
        /// <summary>
        /// 责任人ID
        /// </summary>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        /// 责任部门
        /// </summary>
        [Column("DUTYDEPT")]
        public string DutyDept { get; set; }
        /// <summary>
        /// 责任部门编号
        /// </summary>
        [Column("DUTYDEPTCODE")]
        public string DutyDeptCode { get; set; }
        /// <summary>
        /// 是否存放现场
        /// </summary>
        [Column("ISSCENE")]
        public string IsScene { get; set; }
        /// <summary>
        /// 主要安全风险
        /// </summary>
        [Column("MAINRISK")]
        public string MainRisk { get; set; }
        /// <summary>
        /// 采取的防范措施
        /// </summary>
        [Column("MEASURE")]
        public string Measure { get; set; }
        /// <summary>
        /// 发放人
        /// </summary>
        [Column("GRANTPERSON")]
        public string GrantPerson { get; set; }
        /// <summary>
        /// 发放人ID
        /// </summary>
        [Column("GRANTPERSONID")]
        public string GrantPersonId { get; set; }

        /// <summary>
        /// 规格单位
        /// </summary>
        [Column("SPECIFICATIONUNIT")]
        public string SpecificationUnit { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Column("AMOUNT")]
        public string Amount { get; set; }
        /// <summary>
        /// 数量单位
        /// </summary>
        [Column("AMOUNTUNIT")]
        public string AmountUnit { get; set; }
        /// <summary>
        /// 是否删除   0否  1 是
        /// </summary>
        [Column("ISDELETE")]
        public int? IsDelete { get; set; }

        /// <summary>
        /// 最大存储量
        /// </summary>
        [Column("MAXNUM")]
        public string MAXNUM { get; set; }

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