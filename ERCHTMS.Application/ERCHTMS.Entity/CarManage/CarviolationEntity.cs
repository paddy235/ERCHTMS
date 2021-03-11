using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// 描 述：违章信息类
    /// </summary>
    [Table("BIS_CARVIOLATION")]
    public class CarviolationEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        /// <returns></returns>
        [Column("CARDNO")]
        public string CardNo { get; set; }
        /// <summary>
        /// 司机电话
        /// </summary>
        /// <returns></returns>
        [Column("PHONE")]
        public string Phone { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 车辆类型 0为电厂班车 1为私家车 2为商务公车 3为拜访车辆 4为物料车辆 5为危化品车辆
        /// </summary>
        /// <returns></returns>
        [Column("CARTYPE")]
        public int? CarType { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 默认为0  否 1为是
        /// </summary>
        /// <returns></returns>
        [Column("ISPROCESS")]
        public int? IsProcess { get; set; }
        /// <summary>
        /// 驾驶人
        /// </summary>
        /// <returns></returns>
        [Column("DIRVER")]
        public string Dirver { get; set; }
        /// <summary>
        /// 车辆记录ID
        /// </summary>
        /// <returns></returns>
        [Column("CID")]
        public string CID { get; set; }
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
        /// 处理措施
        /// </summary>
        /// <returns></returns>
        [Column("PROCESSMEASURE")]
        public string ProcessMeasure { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 0为超速 1为偏离航迹 2为超时
        /// </summary>
        /// <returns></returns>
        [Column("VIOLATIONTYPE")]
        public int? ViolationType { get; set; }
        /// <summary>
        /// 车辆违章内容
        /// </summary>
        /// <returns></returns>
        [Column("VIOLATIONMSG")]
        public string ViolationMsg { get; set; }

        /// <summary>
        /// 抓拍地点
        /// </summary>
        /// <returns></returns>
        [Column("ADDRESS")]
        public string Address { get; set; }

        /// <summary>
        /// 车速
        /// </summary>
        /// <returns></returns>
        [Column("SPEED")]
        public int Speed { get; set; }

        /// <summary>
        /// 车牌url
        /// </summary>
        [Column("PLATEPICURL")]
        public string platePicUrl { get; set; }

        /// <summary>
        ///车辆url
        /// </summary>
        [Column("VEHICLEPICURL")]
        public string vehiclePicUrl { get; set; }

        /// <summary>
        /// 车辆型号 大客车 小型汽车
        /// </summary>
        [Column("VEHICLETYPENAME")]
        public string vehicleTypeName { get; set; }

        [Column("DEPTNAME")]
        public string DeptName { get; set; }

        /// <summary>
        /// 图片服务器唯一编号
        /// </summary>
        [Column("HIKPICSVR")]
        public string HikPicSvr { get; set; }

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