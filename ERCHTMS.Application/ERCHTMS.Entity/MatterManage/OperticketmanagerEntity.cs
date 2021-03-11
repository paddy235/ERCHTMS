using System;
using System.ComponentModel.DataAnnotations.Schema;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.MatterManage
{
    /// <summary>
    /// 描 述：开票管理入厂开票
    /// </summary>
    [Table("WL_OPERTICKETMANAGER")]
    public class OperticketmanagerEntity : BaseEntity
    {
        #region 默认字段
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
        /// <summary>
        /// 创建用户Id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户名称
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 创建用户部门ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTID")]
        public string Createuserdeptid { get; set; }
        /// <summary>
        /// 创建用户部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建用户机构编码
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
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户ID
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户名称
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        #endregion

        #region 实体成员
        /// <summary>
        /// 提货方
        /// </summary>
        /// <returns></returns>
        [Column("TAKEGOODSNAME")]
        public string Takegoodsname { get; set; }
        /// <summary>
        /// 提货方编码
        /// </summary>
        /// <returns></returns>
        [Column("TAKEGOODSID")]
        public string Takegoodsid { get; set; }
        /// <summary>
        /// 供货方
        /// </summary>
        /// <returns></returns>
        [Column("SUPPLYNAME")]
        public string Supplyname { get; set; }
        /// <summary>
        /// 供货方编码
        /// </summary>
        /// <returns></returns>
        [Column("SUPPLYID")]
        public string Supplyid { get; set; }
        /// <summary>
        /// 开票员姓名
        /// </summary>
        /// <returns></returns>
        [Column("OPERNAME")]
        public string Opername { get; set; }
        /// <summary>
        /// 开票员账户
        /// </summary>
        /// <returns></returns>
        [Column("OPERACCOUNT")]
        public string Operaccount { get; set; }
        /// <summary>
        /// 放行人
        /// </summary>
        /// <returns></returns>
        [Column("LETMAN")]
        public string LetMan { get; set; }
        /// <summary>
        /// 是否可用1可用
        /// </summary>
        /// <returns></returns>
        [Column("ISDELETE")]
        public int? Isdelete { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// 提货单号
        /// </summary>
        /// <returns></returns>
        [Column("NUMBERS")]
        public string Numbers { get; set; }
        /// <summary>
        /// 入场时间
        /// </summary>
        /// <returns></returns>
        [Column("GETDATA")]
        public DateTime? Getdata { get; set; }

        /// <summary>
        /// 入场打印时间
        /// </summary>
        /// <returns></returns>
        [Column("GETSTAMPTIME")]
        public DateTime? GetStampTime { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        /// <returns></returns>
        [Column("PRODUCTTYPE")]
        public string Producttype { get; set; }

        /// <summary>
        /// 产品类型键值
        /// </summary>
        /// <returns></returns>
        [Column("PRODUCTTYPEID")]
        public string ProducttypeId { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        /// <returns></returns>
        [Column("PLATENUMBER")]
        public string Platenumber { get; set; }
        /// <summary>
        /// 装料点
        /// </summary>
        /// <returns></returns>
        [Column("DRESS")]
        public string Dress { get; set; }

        /// <summary>
        /// 运输类型
        /// </summary>
        [Column("TRANSPORTTYPE")]
        public string Transporttype { get; set; }

        /// <summary>
        /// 删除原因
        /// </summary>
        [Column("DELETECONTENT")]
        public string DeleteContent { get; set; }

        /// <summary>
        /// 异常放行备注/删除原因备注
        /// </summary>
        [Column("PASSREMARK")]
        public string PassRemark { get; set; }

        /// <summary>
        /// 是否第一次入场
        /// </summary>
        [Column("ISFIRST")]
        public string IsFirst { get; set; }

        /// <summary>
        /// 是否按轨迹行驶
        /// </summary>
        [Column("ISTRAJECTORY")]
        public string IsTrajectory { get; set; }

        /// <summary>
        /// 入库次数
        /// </summary>
        [Column("DATABASENUM")]
        public int? DataBaseNum { get; set; }

        /// <summary>
        /// 入库时间
        /// </summary>
        /// <returns></returns>
        [Column("GODATABASETIME")]
        public DateTime? GoDatabasetime { get; set; }

        /// <summary>
        /// 出厂打印时间
        /// </summary>
        /// <returns></returns>
        [Column("OUTDATABASETIME")]
        public DateTime? OutDatabasetime { get; set; }

        /// <summary>
        /// 称重次数
        /// </summary>
        [Column("WEIGHINGNUM")]
        public int? WeighingNum { get; set; }

        /// <summary>
        /// 离厂时间
        /// </summary>
        [Column("OUTDATE")]
        public DateTime? OutDate { get; set; }
        /// <summary>
        /// 厂内逗留时间(分钟)
        /// </summary>
        [Column("STAYTIME")]
        public double? StayTime { get; set; }
        /// <summary>
        /// 厂内状态
        /// </summary>
        [Column("STATUS")]
        public string Status { get; set; }
        /// <summary> 
        /// 审批状态（0进场1司机上传  2为已录入GPS数据 3审批通过 4离厂 99拒绝入场）
        /// </summary>
        [Column("EXAMINESTATUS")]
        public int ExamineStatus { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        [Column("WEIGHT")]
        public double? Weight { get; set; }

        /// <summary>
        /// 准入
        /// </summary>
        [Column("ADMITTANCE")]
        public string Admittance { get; set; }
        /// <summary>
        /// 称重状态1已称重
        /// </summary>
        [Column("OUTCU")]
        public string OutCu { get; set; }
        /// <summary>
        /// 司机姓名
        /// </summary>
        [Column("DRIVERNAME")]
        public string DriverName { get; set; }
        /// <summary>
        /// 司机电话
        /// </summary>
        [Column("DRIVERTEL")]
        public string DriverTel { get; set; }

        /// <summary>
        /// 核载重量
        /// </summary>
        [Column("HZWEIGHT")]
        public double HzWeight { get; set; }
        /// <summary>
        /// 驾驶证图片路径
        /// </summary>
        [Column("JSIMGPATH")]
        public string JsImgpath { get; set; }
        /// <summary>
        /// 行驶证图片路径
        /// </summary>
        [Column("XSIMGPATH")]
        public string XsImgpath { get; set; }
        /// <summary>
        /// Gps名称
        /// </summary>
        [Column("GPSNAME")]
        public string GpsName { get; set; }
        /// <summary>
        /// Gps编码
        /// </summary>
        [Column("GPSID")]
        public string GpsId { get; set; }
        /// <summary>
        /// 到达地磅室时间
        /// </summary>
        [Column("BALANCETIME")]
        public DateTime? BalanceTime { get; set; }

        /// <summary>
        /// 出厂排序字段
        /// </summary>
        [Column("ORDERNUM")]
        public int OrderNum { get; set; }

        /// <summary>
        /// 入场开票排序
        /// </summary>
        [Column("ORDERNUMR")]
        public int OrderNumR { get; set; }

        /// <summary>
        /// 是否到码头（1是默认否0）
        /// </summary>
        [Column("ISWHARF")]
        public int ISwharf { get; set; }

        /// <summary>
        /// 厂内行驶路线状态
        /// </summary>
        [Column("TRAVELSTATUS")]
        public string TravelStatus { get; set; }

        /// <summary>
        /// 驾驶人身份证照片
        /// </summary>
        [Column("IDENTITETIIMG")]
        public string IdentitetiImg { get; set; }


        /// <summary>
        ///净重状态（表中排除）
        /// </summary>
        [NotMapped]
        public string NetweightStatus { get; set; }
        /// <summary>
        ///入库状态（表中排除）
        /// </summary>
        [NotMapped]
        public string DatabaseStatus { get; set; }
        /// <summary>
        ///场内停留时间状态（表中排除）
        /// </summary>
        [NotMapped]
        public string StayTimeStatus { get; set; }

        /// <summary>
        ///入厂至地磅时间（表中排除）
        /// </summary>
        [NotMapped]
        public string RCdbTime { get; set; }

        /// <summary>
        ///地磅至出厂时间（表中排除）
        /// </summary>
        [NotMapped]
        public string DbOutTime { get; set; }

        /// <summary>
        ///模板排序字段
        /// </summary>
        [Column("TEMPLATESORT")]
        public int? TemplateSort { get; set; }

        /// <summary>
        /// 是否装船
        /// </summary>
        [Column("SHIPLOADING")]
        public int ShipLoading { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            if (string.IsNullOrEmpty(this.CreateUserId) && OperatorProvider.Provider.Current() != null)
                this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            if (string.IsNullOrEmpty(this.CreateUserName) && OperatorProvider.Provider.Current() != null)
                this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            if (string.IsNullOrEmpty(this.CreateUserDeptCode) && OperatorProvider.Provider.Current() != null)
                this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            if (string.IsNullOrEmpty(this.CreateUserOrgCode) && OperatorProvider.Provider.Current() != null)
                this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
            if (string.IsNullOrEmpty(this.Createuserdeptid) && OperatorProvider.Provider.Current() != null)
                this.Createuserdeptid = OperatorProvider.Provider.Current().DeptId;
            this.Isdelete = 1;
            this.Weight = 0;
            this.HzWeight = 0;
            this.ExamineStatus = 0;
            this.OrderNum = this.OrderNumR = 0;
            this.TravelStatus = this.Status = "正常";

        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            //this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            //this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}