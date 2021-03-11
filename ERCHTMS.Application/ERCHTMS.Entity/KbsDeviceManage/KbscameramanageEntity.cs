using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.KbsDeviceManage
{
    /// <summary>
    /// 描 述：康巴什摄像头管理
    /// </summary>
    [Table("BIS_KBSCAMERAMANAGE")]
    public class KbscameramanageEntity : BaseEntity
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
        /// 摄像头ID
        /// </summary>
        /// <returns></returns>
        [Column("CAMERAID")]
        public string CameraId { get; set; }
        /// <summary>
        /// 摄像头名称
        /// </summary>
        /// <returns></returns>
        [Column("CAMERANAME")]
        public string CameraName { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        /// <returns></returns>
        [Column("SORT")]
        public int? Sort { get; set; }
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
        /// 区域CODE
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
        /// 摄像头IP
        /// </summary>
        /// <returns></returns>
        [Column("CAMERAIP")]
        public string CameraIP { get; set; }
        /// <summary>
        /// 摄像头坐标
        /// </summary>
        /// <returns></returns>
        [Column("CAMERAPOINT")]
        public string CameraPoint { get; set; }
        /// <summary>
        /// 操作人名称
        /// </summary>
        /// <returns></returns>
        [Column("OPERUSERNAME")]
        public string OperuserName { get; set; }
        /// <summary>
        /// 摄像头类别
        /// </summary>
        /// <returns></returns>
        [Column("CAMERATYPE")]
        public string CameraType { get; set; }
        /// <summary>
        /// 摄像头类别ID
        /// </summary>
        /// <returns></returns>
        [Column("CAMERATYPEID")]
        public int? CameraTypeId { get; set; }

        /// <summary>
        /// 状态 在线/离线
        /// </summary>
        [Column("STATE")]
        public string State { get; set; }

        /// <summary>
        /// 监控区域坐标
        /// </summary>
        [Column("MONITORINGAREA")]
        public string MonitoringArea { get; set; }
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