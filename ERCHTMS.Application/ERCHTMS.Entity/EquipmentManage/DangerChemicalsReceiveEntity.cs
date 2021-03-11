using ERCHTMS.Code;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERCHTMS.Entity.EquipmentManage
{
    /// <summary>
    /// 描 述：危险化学品领用
    /// </summary>
    [Table("XLD_DANGEROUSCHEMICALRECEIVE")]
    public class DangerChemicalsReceiveEntity : BaseEntity
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
        /// 危化品ID
        /// </summary>
        [Column("MAINID")]
        public string MainId { get; set; }
        /// <summary>
        /// 用途
        /// </summary>
        [Column("PURPOSE")]
        public string Purpose { get; set; }
        /// <summary>
        /// 领用数量
        /// </summary>
        [Column("RECEIVENUM")]
        public string ReceiveNum { get; set; }
        /// <summary>
        /// 领用数量单位
        /// </summary>
        [Column("RECEIVEUNIT")]
        public string ReceiveUnit { get; set; }
        /// <summary>
        /// 领用人ID
        /// </summary>
        [Column("RECEIVEUSERID")]
        public string ReceiveUserId { get; set; }
        /// <summary>
        /// 领用人
        /// </summary>
        [Column("RECEIVEUSER")]
        public string ReceiveUser { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        [Column("SIGNIMG")]
        public string SignImg { get; set; }
        
        #endregion

        #region 流程字段        
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
        /// 审核流程Id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }


        /// <summary>
        /// 发放数量
        /// </summary>
        [Column("GRANTNUM")]
        public string GrantNum { get; set; }
        /// <summary>
        /// 发放数量单位
        /// </summary>
        [Column("GRANTUNIT")]
        public string GrantUnit { get; set; }
        /// <summary>
        /// 发放人ID
        /// </summary>
        [Column("GRANTUSERID")]
        public string GrantUserId { get; set; }
        /// <summary>
        /// 发放人
        /// </summary>
        [Column("GRANTUSER")]
        public string GrantUser { get; set; }
        /// <summary>
        /// 发放意见
        /// </summary>
        [Column("GRANTOPINION")]
        public string GrantOpinion { get; set; }
        /// <summary>
        /// 发放日期
        /// </summary>
        /// <returns></returns>
        [Column("GRANTDATE")]
        public DateTime? GrantDate { get; set; }
        /// <summary>
        /// 发放人签名
        /// </summary>
        [Column("GRANTSIGNIMG")]
        public string GrantSignImg { get; set; }
        /// <summary>
        /// 发放状态 0未开始发放  1发放中 2发放完成
        /// </summary>
        [Column("GRANTSTATE")]
        public int? GrantState { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }
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
        /// 存放地点
        /// </summary>
        [Column("SITE")]
        public string Site { get; set; }
        /// <summary>
        /// 实际发放库存数量
        /// </summary>
        [Column("PRACTICALNUM")]
        public string PracticalNum { get; set; }

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