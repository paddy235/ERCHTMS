using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.LaborProtectionManage
{
    /// <summary>
    /// 描 述：劳动防护用品
    /// </summary>
    [Table("BIS_LABORPROTECTION")]
    public class LaborprotectionEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
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
        /// <summary>
        /// 用品名称
        /// </summary>
        /// <returns></returns>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// 用品编号
        /// </summary>
        /// <returns></returns>
        [Column("NO")]
        public string No { get; set; }
        /// <summary>
        /// 用品单位
        /// </summary>
        /// <returns></returns>
        [Column("UNIT")]
        public string Unit { get; set; }
        /// <summary>
        /// 用品型号
        /// </summary>
        /// <returns></returns>
        [Column("MODEL")]
        public string Model { get; set; }
        /// <summary>
        /// 用品类型
        /// </summary>
        /// <returns></returns>
        [Column("TYPE")]
        public string Type { get; set; }
        /// <summary>
        /// 使用期限时长
        /// </summary>
        /// <returns></returns>
        [Column("TIMENUM")]
        public int? TimeNum { get; set; }
        /// <summary>
        /// 使用期限单位 年、月、日
        /// </summary>
        /// <returns></returns>
        [Column("TIMETYPE")]
        public string TimeType { get; set; }
        /// <summary>
        /// 使用说明
        /// </summary>
        /// <returns></returns>
        [Column("NOTE")]
        public string Note { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        /// <returns></returns>
        [Column("LABOROPERATIONUSERNAME")]
        public string LaborOperationUserName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("LABOROPERATIONTIME")]
        public DateTime LaborOperationTime { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
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
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
                    }
        #endregion
    }
}