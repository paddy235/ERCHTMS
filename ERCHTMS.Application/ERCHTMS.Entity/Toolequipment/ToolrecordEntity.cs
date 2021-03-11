using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.ToolEquipmentManage
{
    /// <summary>
    /// 描 述：电动工器具试验记录
    /// </summary>
    [Table("BIS_TOOLRECORD")]
    public class ToolrecordEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
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
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
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
        /// 下次试验日期
        /// </summary>
        /// <returns></returns>
        [Column("NEXTCHECKDATE")]
        public DateTime? NextCheckDate { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNAME")]
        public string EquipmentName { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNO")]
        public string EquipmentNo { get; set; }
        /// <summary>
        /// 试验人员ID
        /// </summary>
        /// <returns></returns>
        [Column("OPERUSERID")]
        public string OperUserId { get; set; }
        /// <summary>
        /// 电压等级
        /// </summary>
        /// <returns></returns>
        [Column("VOLTAGELEVEL")]
        public string VoltageLevel { get; set; }
        /// <summary>
        /// 试验人员
        /// </summary>
        /// <returns></returns>
        [Column("OPERUSER")]
        public string OperUser { get; set; }
        /// <summary>
        /// 评价
        /// </summary>
        /// <returns></returns>
        [Column("APPRAISE")]
        public string Appraise { get; set; }
        /// <summary>
        /// 试验电压
        /// </summary>
        /// <returns></returns>
        [Column("TRIALVOLTAGE")]
        public string TrialVoltage { get; set; }
        /// <summary>
        /// 试验日期
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATE")]
        public DateTime? CheckDate { get; set; }
        /// <summary>
        /// 工器具id
        /// </summary>
        /// <returns></returns>
        [Column("TOOLEQUIPMENTID")]
        public string ToolEquipmentId { get; set; }


        /// <summary>
        /// 规格型号
        /// </summary>
        /// <returns></returns>
        [Column("SPECIFICATION")]
        public string  Specification { get; set; }


        /// <summary>
        /// 检查的具体项目
        /// </summary>
        /// <returns></returns>
        [Column("CHECKPROJECT")]
        public string CheckProject { get; set; }
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