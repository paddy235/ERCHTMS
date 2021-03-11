using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EmergencyPlatform
{
    /// <summary>
    /// 描 述：演练评估表
    /// </summary>
    [Table("MAE_DRILLASSESS")]
    public class DrillassessEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 评价人员ID
        /// </summary>
        /// <returns></returns>
        [Column("VALUATEPERSON")]
        public string ValuatePerson { get; set; }
        /// <summary>
        /// 演练地点
        /// </summary>
        /// <returns></returns>
        [Column("DRILLPLACE")]
        public string DrillPlace { get; set; }
        /// <summary>
        /// 措施
        /// </summary>
        /// <returns></returns>
        [Column("MEASURE")]
        public string Measure { get; set; }
        /// <summary>
        /// 充分性
        /// </summary>
        /// <returns></returns>
        [Column("FULLABLE")]
        public string Fullable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 实战效果评价
        /// </summary>
        /// <returns></returns>
        [Column("EFFECTEVALUATE")]
        public string EffecteValuate { get; set; }
        /// <summary>
        /// 问题
        /// </summary>
        /// <returns></returns>
        [Column("PROBLEM")]
        public string Problem { get; set; }
        /// <summary>
        /// 总指挥
        /// </summary>
        /// <returns></returns>
        [Column("TOPPERSON")]
        public string TopPerson { get; set; }
        /// <summary>
        /// 演练名称
        /// </summary>
        /// <returns></returns>
        [Column("DRILLNAME")]
        public string DrillName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 整体组织
        /// </summary>
        /// <returns></returns>
        [Column("WHOLEORGANIZE")]
        public string WholeOrganize { get; set; }
        /// <summary>
        /// 物资到位人员职责
        /// </summary>
        /// <returns></returns>
        [Column("SITESUPPLIESDUTY")]
        public string SiteSuppliesDuty { get; set; }
        /// <summary>
        /// 演练类别
        /// </summary>
        /// <returns></returns>
        [Column("DRILLTYPE")]
        public string DrillType { get; set; }
        /// <summary>
        /// 评价人员姓名
        /// </summary>
        /// <returns></returns>
        [Column("VALUATEPERSONNAME")]
        public string ValuatePersonName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 适宜性
        /// </summary>
        /// <returns></returns>
        [Column("SUITABLE")]
        public string Suitable { get; set; }
        /// <summary>
        /// 关联应急演练ID
        /// </summary>
        /// <returns></returns>
        [Column("DRILLID")]
        public string DrillId { get; set; }
        /// <summary>
        /// 组织分工
        /// </summary>
        /// <returns></returns>
        [Column("DIVIDEWORK")]
        public string DivideWork { get; set; }
        /// <summary>
        /// 演练时间
        /// </summary>
        /// <returns></returns>
        [Column("DRILLTIME")]
        public DateTime? DrillTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 警戒撤离配合
        /// </summary>
        /// <returns></returns>
        [Column("EVACUATE")]
        public string Evacuate { get; set; }
        /// <summary>
        /// 人员到位
        /// </summary>
        /// <returns></returns>
        [Column("PERSONSTANDBY")]
        public string PersonStandBy { get; set; }
        /// <summary>
        /// 报告上级
        /// </summary>
        /// <returns></returns>
        [Column("REPORTSUPERIOR")]
        public string ReportSuperior { get; set; }
        /// <summary>
        /// 演练内容
        /// </summary>
        /// <returns></returns>
        [Column("DRILLCONTENT")]
        public string DrillContent { get; set; }
        /// <summary>
        /// 救援、后援配合
        /// </summary>
        /// <returns></returns>
        [Column("RESCUE")]
        public string Rescue { get; set; }
        /// <summary>
        /// 现场物资
        /// </summary>
        /// <returns></returns>
        [Column("SITESUPPLIES")]
        public string SiteSupplies { get; set; }
        /// <summary>
        /// 组织部门
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZEDEPT")]
        public string OrganizeDept { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 人员到位职责
        /// </summary>
        /// <returns></returns>
        [Column("PERSONSTANDBYDUTY")]
        public string PersonStandByDuty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
           this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
           this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}