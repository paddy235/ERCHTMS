using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// 描 述：人员GPS关联表
    /// </summary>
    [Table("BIS_PERSONGPS")]
    public class PersongpsEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 进厂时间
        /// </summary>
        /// <returns></returns>
        [Column("INTIME")]
        public DateTime? InTime { get; set; }
        /// <summary>
        /// 关联ID
        /// </summary>
        /// <returns></returns>
        [Column("VID")]
        public string VID { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 0 为拜访车辆随行人员 1为普通外来人员
        /// </summary>
        /// <returns></returns>
        [Column("TYPE")]
        public int? Type { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 是否出厂 0在场内 1已出厂
        /// </summary>
        /// <returns></returns>
        [Column("STATE")]
        public int? State { get; set; }
        /// <summary>
        /// GPSID
        /// </summary>
        /// <returns></returns>
        [Column("GPSID")]
        public string GPSID { get; set; }
        /// <summary>
        /// 出厂时间
        /// </summary>
        /// <returns></returns>
        [Column("OUTTIME")]
        public DateTime? OutTime { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 人员姓名
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
        /// <summary>
        /// GPSID
        /// </summary>
        /// <returns></returns>
        [Column("GPSNAME")]
        public string GPSNAME { get; set; }

        /// <summary>
        /// 是否提交 0未提交 1已提交
        /// </summary>
        /// <returns></returns>
        [Column("ISSUBMIT")]
        public int? Issubmit { get; set; }

     


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