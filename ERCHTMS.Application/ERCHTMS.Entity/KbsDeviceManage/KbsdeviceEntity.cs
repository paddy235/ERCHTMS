using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.KbsDeviceManage
{
    /// <summary>
    /// 描 述：康巴什门禁管理
    /// </summary>
    [Table("BIS_KBSDEVICE")]
    public class KbsdeviceEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
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
        /// 门禁ID
        /// </summary>
        /// <returns></returns>
        [Column("DEVICEID")]
        public string DeviceId { get; set; }
        /// <summary>
        /// 控制器ID
        /// </summary>
        /// <returns></returns>
        [Column("CONTROLLERID")]
        public string ControllerId { get; set; }
        /// <summary>
        /// 进出类型 0表示进门设备 1表示出门设备
        /// </summary>
        /// <returns></returns>
        [Column("OUTTYPE")]
        public int? OutType { get; set; }
        /// <summary>
        /// 门禁名称
        /// </summary>
        /// <returns></returns>
        [Column("DEVICENAME")]
        public string DeviceName { get; set; }
        /// <summary>
        /// 门禁型号
        /// </summary>
        /// <returns></returns>
        [Column("DEVICEMODEL")]
        public string DeviceModel { get; set; }
        /// <summary>
        /// 区域ID
        /// </summary>
        /// <returns></returns>
        [Column("AREAID")]
        public string AreaId { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        /// <returns></returns>
        [Column("AREANAME")]
        public string AreaName { get; set; }
        /// <summary>
        /// 区域Code
        /// </summary>
        /// <returns></returns>
        [Column("AREACODE")]
        public string AreaCode { get; set; }
        /// <summary>
        /// 楼层编号
        /// </summary>
        /// <returns></returns>
        [Column("FLOORNO")]
        public string FloorNo { get; set; }
        /// <summary>
        /// 门禁IP
        /// </summary>
        /// <returns></returns>
        [Column("DEVICEIP")]
        public string DeviceIP { get; set; }
        /// <summary>
        /// 门禁坐标
        /// </summary>
        /// <returns></returns>
        [Column("DEVICEPOINT")]
        public string DevicePoint { get; set; }
        /// <summary>
        /// 操作人名称
        /// </summary>
        /// <returns></returns>
        [Column("OPERUSERNAME")]
        public string OperUserName { get; set; }

        /// <summary>
        /// 状态 在线/离线
        /// </summary>
        [Column("STATE")]
        public string State { get; set; }
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