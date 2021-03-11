using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// 描 述：危害因素车辆表
    /// </summary>
    [Table("BIS_HAZARDOUSCAR")]
    public class HazardouscarEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 驾驶证图片地址
        /// </summary>
        /// <returns></returns>
        [Column("DRIVERLICENSEURL")]
        public string DriverLicenseUrl { get; set; }
        /// <summary>
        /// 危化品ID
        /// </summary>
        /// <returns></returns>
        [Column("HAZARDOUSID")]
        public string HazardousId { get; set; }
        /// <summary>
        /// 处理人姓名
        /// </summary>
        /// <returns></returns>
        [Column("PROCESSINGNAME")]
        public string ProcessingName { get; set; }
        /// <summary>
        /// 入场时间
        /// </summary>
        /// <returns></returns>
        [Column("INTIME")]
        public DateTime? InTime { get; set; }
        /// <summary>
        /// 定位设备ID
        /// </summary>
        /// <returns></returns>
        [Column("GPSID")]
        public string GPSID { get; set; }
        /// <summary>
        /// 司机电话
        /// </summary>
        /// <returns></returns>
        [Column("PHONE")]
        public string Phone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("NOTE")]
        public string Note { get; set; }
        /// <summary>
        /// 驾驶人
        /// </summary>
        /// <returns></returns>
        [Column("DIRVER")]
        public string Dirver { get; set; }
        /// <summary>
        /// 定位设备名称
        /// </summary>
        /// <returns></returns>
        [Column("GPSNAME")]
        public string GPSNAME { get; set; }
        /// <summary>
        /// 拜访人数
        /// </summary>
        /// <returns></returns>
        [Column("ACCOMPANYINGNUMBER")]
        public int? AccompanyingNumber { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 危化品名称
        /// </summary>
        /// <returns></returns>
        [Column("HAZARDOUSNAME")]
        public string HazardousName { get; set; }
        /// <summary>
        /// 处理人签名图片
        /// </summary>
        /// <returns></returns>
        [Column("PROCESSINGSIGN")]
        public string ProcessingSign { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        /// <returns></returns>
        [Column("CARNO")]
        public string CarNo { get; set; }
        /// <summary>
        /// 危化品流程序号0为默认 1为已确认车辆  2为已交接车辆
        /// </summary>
        /// <returns></returns>
        [Column("HAZARDOUSPROCESS")]
        public int? HazardousProcess { get; set; }
        /// <summary>
        /// 交接人ID
        /// </summary>
        /// <returns></returns>
        [Column("HANDOVERID")]
        public string HandoverId { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 出厂时间
        /// </summary>
        /// <returns></returns>
        [Column("OUTTIME")]
        public DateTime? OutTime { get; set; }
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
        /// 随行人员
        /// </summary>
        /// <returns></returns>
        [Column("ACCOMPANYINGPERSON")]
        public string AccompanyingPerson { get; set; }
        /// <summary>
        /// 处理人ID
        /// </summary>
        /// <returns></returns>
        [Column("PROCESSINGID")]
        public string ProcessingId { get; set; }
        /// <summary>
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 记录状态 0为已开票 1为已提交待审批 2为已录入GPS数据 3为审批通过 4为已出厂 99为拒绝入场
        /// </summary>
        /// <returns></returns>
        [Column("STATE")]
        public int? State { get; set; }
        /// <summary>
        /// 行驶证图片地址
        /// </summary>
        /// <returns></returns>
        [Column("DRIVINGLICENSEURL")]
        public string DrivingLicenseUrl { get; set; }
        /// <summary>
        /// 交接人签名图片
        /// </summary>
        /// <returns></returns>
        [Column("HANDOVERSIGN")]
        public string HandoverSign { get; set; }
        /// <summary>
        /// 交接人姓名
        /// </summary>
        /// <returns></returns>
        [Column("HANDOVERNAME")]
        public string HandoverName { get; set; }
        /// <summary>
        /// 所属单位
        /// </summary>
        /// <returns></returns>
        [Column("THECOMPANY")]
        public string TheCompany { get; set; }

        /// <summary>
        /// 厂内行驶路线（正常、异常）
        /// </summary>
        /// <returns></returns>
        [Column("DRIVINGROUTE")]
        public string DrivingRoute { get; set; }

        /// <summary>
        /// 厂内停留时间（正常、异常）
        /// </summary>
        /// <returns></returns>
        [Column("RESIDENCETIME")]
        public string ResidenceTime { get; set; }

        /// <summary>
        /// 厂内行驶速度（正常、异常）
        /// </summary>
        /// <returns></returns>
        [Column("DRIVINGSPEED")]
        public string DrivingSpeed { get; set; }

        /// <summary>
        /// 是否已提交0未提交 1已提交 2车辆已出厂
        /// </summary>
        /// <returns></returns>
        [Column("ISSUBMIT")]
        public int? Issubmit { get; set; }

        /// <summary>
        /// 手机手机app审批状态默认0 通过1 未通过2
        /// </summary>
        /// <returns></returns>
        [Column("APPSTATUE")]
        public int AppStatue { get; set; }

        /// <summary>
        /// 申请入场时间
        /// </summary>
        [Column("APPLYDATE")]
        public DateTime? ApplyDate { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID)?Guid.NewGuid().ToString():ID;
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