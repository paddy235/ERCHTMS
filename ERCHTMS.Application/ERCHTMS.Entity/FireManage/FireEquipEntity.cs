using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.FireManage
{
    /// <summary>
    /// 描 述：消防队伍配备
    /// </summary>
    [Table("HRS_FIREEQUIP")]
    public class FireEquipEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AutoId { get; set; }
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
        /// 设备名称
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNAME")]
        public string EquipmentName { get; set; }

        /// <summary>
        /// 用途
        /// </summary>
        /// <returns></returns>
        [Column("PURPOSE")]
        public string Purpose { get; set; }
        /// <summary>
        /// 一级站配备
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPONE")]
        public string EquipOne { get; set; }
        /// <summary>
        /// 一级站配备单位
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPUNITONE")]
        public string EquipUnitOne { get; set; }
        /// <summary>
        /// 一级站备份比
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPRATIOONE")]
        public string EquipRatioOne { get; set; }
        /// <summary>
        /// 一级站实际配备数量
        /// </summary>
        /// <returns></returns>
        [Column("PRACTICALEQUIPONE")]
        public string PracticalEquipOne { get; set; }
        /// <summary>
        /// 一级站实际配备数量单位
        /// </summary>
        /// <returns></returns>
        [Column("PRACTICALEQUIPUNITONE")]
        public string PracticalEquipUnitOne { get; set; }
        /// <summary>
        /// 一级站备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARKONE")]
        public string RemarkOne { get; set; }
        /// <summary>
        /// 二级站配备
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPTWO")]
        public string EquipTwo { get; set; }
        /// <summary>
        /// 二级站配备单位
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPUNITTWO")]
        public string EquipUnitTwo { get; set; }
        /// <summary>
        /// 二级站备份比
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPRATIOTWO")]
        public string EquipRatioTwo { get; set; }
        /// <summary>
        /// 二级站实际配备数量
        /// </summary>
        /// <returns></returns>
        [Column("PRACTICALEQUIPTWO")]
        public string PracticalEquipTwo { get; set; }
        /// <summary>
        /// 二级站实际配备数量单位
        /// </summary>
        /// <returns></returns>
        [Column("PRACTICALEQUIPUNITTWO")]
        public string PracticalEquipUnitTwo { get; set; }
        /// <summary>
        /// 二级站备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARKTWO")]
        public string RemarkTwo { get; set; }

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