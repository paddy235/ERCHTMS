using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// 描 述：门禁点权限表
    /// </summary>
    [Table("BIS_HIKACCESSAURHORITY")]
    public class HikaccessaurhorityEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 权限结束时间
        /// </summary>
        /// <returns></returns>
        [Column("ENDTIME")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 设备在安防平台中的ID
        /// </summary>
        /// <returns></returns>
        [Column("HIKID")]
        public string HikId { get; set; }
        /// <summary>
        /// 进出类型 0表示进门设备 1表示出门设备
        /// </summary>
        /// <returns></returns>
        [Column("OUTTYPE")]
        public int? OutType { get; set; }
        /// <summary>
        /// 门禁控制器ID
        /// </summary>
        /// <returns></returns>
        [Column("PARENTID")]
        public string ParentId { get; set; }
        /// <summary>
        /// 0为部门 1为用户
        /// </summary>
        /// <returns></returns>
        [Column("TYPE")]
        public int Type { get; set; }
        /// <summary>
        /// 权限开始时间
        /// </summary>
        /// <returns></returns>
        [Column("STARTTIME")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        /// <returns></returns>
        [Column("DEVICENAME")]
        public string DeviceName { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 所属部门或用户关联ID
        /// </summary>
        /// <returns></returns>
        [Column("RID")]
        public string RID { get; set; }
        /// <summary>
        /// 设备归属区域名称
        /// </summary>
        /// <returns></returns>
        [Column("AREANAME")]
        public string AreaName { get; set; }
        /// <summary>
        /// 设备归属区域ID
        /// </summary>
        /// <returns></returns>
        [Column("AREAID")]
        public string AreaId { get; set; }
        /// <summary>
        /// 所属部门或用户关联名称
        /// </summary>
        /// <returns></returns>
        [Column("RNAME")]
        public string RName { get; set; }
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
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 门禁点通道号
        /// </summary>
        /// <returns></returns>
        [Column("HIKNOS")]
        public int HikNos { get; set; }
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