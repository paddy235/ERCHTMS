using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// 描 述：门禁设备管理
    /// </summary>
    [Table("BIS_HIKDEVICE")]
    public class HikdeviceEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 设备在平台上配置的IP
        /// </summary>
        /// <returns></returns>
        [Column("DEVICEIP")]
        public string DeviceIP { get; set; }
        /// <summary>
        /// 设备在安防平台中的ID
        /// </summary>
        /// <returns></returns>
        [Column("HIKID")]
        public string HikID { get; set; }
        /// <summary>
        /// 设备归属区域名称（一号岗、生活岗、三号岗）
        /// </summary>
        /// <returns></returns>
        [Column("AREANAME")]
        public string AreaName { get; set; }
        /// <summary>
        /// 进出类型 0表示进门设备 1表示出门设备
        /// </summary>
        /// <returns></returns>
        [Column("OUTTYPE")]
        public int? OutType { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        /// <returns></returns>
        [Column("DEVICENAME")]
        public string DeviceName { get; set; }
        /// <summary>
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        /// <returns></returns>
        [Column("DEVICETYPE")]
        public string DeviceType { get; set; }
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