using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// 描 述：车辆基础信息表
    /// </summary>
    [Table("BIS_CARINFO")]
    public class CarinfoEntity : BaseEntity
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
        /// 车牌号
        /// </summary>
        /// <returns></returns>
        [Column("CARNO")]
        public string CarNo { get; set; }
        /// <summary>
        /// 驾驶人
        /// </summary>
        /// <returns></returns>
        [Column("DIRVER")]
        public string Dirver { get; set; }

        /// <summary>
        /// 驾驶人Id
        /// </summary>
        /// <returns></returns>
        [Column("DIRVERID")]
        public string DirverId { get; set; }

        /// <summary>
        /// 司机电话
        /// </summary>
        /// <returns></returns>
        [Column("PHONE")]
        public string Phone { get; set; }
        /// <summary>
        /// 最近年检日期
        /// </summary>
        /// <returns></returns>
        [Column("INSPERCTIONDATE")]
        public DateTime? InsperctionDate { get; set; }
        /// <summary>
        /// 下次年检日期
        /// </summary>
        /// <returns></returns>
        [Column("NEXTINSPERCTIONDATE")]
        public DateTime? NextInsperctionDate { get; set; }
        /// <summary>
        /// 车辆类型 0为电厂班车 1为私家车 2为商务公车 3为拜访车辆 4为物料车辆 5为危化品车辆 6临时通行车辆
        /// </summary>
        /// <returns></returns>
        [Column("TYPE")]
        public int Type { get; set; }
        /// <summary>
        /// 荷载人数
        /// </summary>
        /// <returns></returns>
        [Column("NUMBERLIMIT")]
        public int? NumberLimit { get; set; }
        /// <summary>
        /// 驾驶证图片地址
        /// </summary>
        /// <returns></returns>
        [Column("DRIVERLICENSEURL")]
        public string DriverLicenseUrl { get; set; }
        /// <summary>
        /// 行驶证图片地址
        /// </summary>
        /// <returns></returns>
        [Column("DRIVINGLICENSEURL")]
        public string DrivingLicenseUrl { get; set; }
        /// <summary>
        /// 车辆型号
        /// </summary>
        /// <returns></returns>
        [Column("MODEL")]
        public string Model { get; set; }
        /// <summary>
        /// 定位设备ID
        /// </summary>
        /// <returns></returns>
        [Column("GPSID")]
        public string GpsId { get; set; }
        /// <summary>
        /// 定位设备名称
        /// </summary>
        /// <returns></returns>
        [Column("GPSNAME")]
        public string GpsName { get; set; }

        /// <summary>
        /// 是否授权
        /// </summary>
        [Column("ISAUTHORIZED")]
        public int? IsAuthorized { get; set; }

        /// <summary>
        /// 授权用户Id
        /// </summary>
        [Column("AUTHUSERID")]
        public string AuthUserId { get; set; }

        /// <summary>
        /// 授权用户名称
        /// </summary>
        [Column("AUTHUSERNAME")]
        public string AuthUserName { get; set; }

        /// <summary>
        /// 是否启用0否1是
        /// </summary>
        [Column("ISENABLE")]
        public int IsEnable { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 通行门岗ID
        /// </summary>
        [Column("CURRENTGID")]
        public string Currentgid { get; set; }
        /// <summary>
        /// 通行门岗名称
        /// </summary>
        [Column("CURRENTGNAME")]
        public string Currentgname { get; set; }

        /// <summary>
        /// 审批状态 0未通过 1通过
        /// </summary>
        [Column("STATE")]
        public string State { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [Column("DEPTNAME")]
        public string Deptname { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        /// <returns></returns>
        [Column("STARTTIME")]
        public DateTime? Starttime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        /// <returns></returns>
        [Column("ENDTIME")]
        public DateTime? Endtime { get; set; }



        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CreateDate = DateTime.Now;
            this.IsEnable = 0;
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