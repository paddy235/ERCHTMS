using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病危害告知卡
    /// </summary>
    [Table("BIS_STAFFINFORMCARD")]
    public class StaffinformcardEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 告知卡名称
        /// </summary>
        /// <returns></returns>
        [Column("INFORMCARDNAME")]
        public string InformCardName { get; set; }
        /// <summary>
        /// 告知卡值
        /// </summary>
        /// <returns></returns>
        [Column("INFORMCARDVALUE")]
        public string InformCardValue { get; set; }
        /// <summary>
        /// 告知卡设置位置
        /// </summary>
        /// <returns></returns>
        [Column("INFORMACARDPOSITION")]
        public string InformaCardPosition { get; set; }
        /// <summary>
        /// 设置时间
        /// </summary>
        /// <returns></returns>
        [Column("SETTINGTIME")]
        public DateTime? SettingTime { get; set; }
        /// <summary>
        /// 告知卡类型 0为本单位 1位告知卡库
        /// </summary>
        /// <returns></returns>
        [Column("CARDTYPE")]
        public int? CardType { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
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
                    }
        #endregion
    }
}