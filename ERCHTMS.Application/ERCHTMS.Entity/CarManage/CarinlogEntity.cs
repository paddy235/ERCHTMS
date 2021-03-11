using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// 描 述：车辆进出记录表
    /// </summary>
    [Table("BIS_CARINLOG")]
    public class CarinlogEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 车辆关联ID
        /// </summary>
        /// <returns></returns>
        [Column("CID")]
        public string CID { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 是否可以出门 默认0为是 1为否
        /// </summary>
        /// <returns></returns>
        [Column("ISOUT")]
        public int? IsOut { get; set; }
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
        /// 经过门岗（1号岗  3号岗）
        /// </summary>
        /// <returns></returns>
        [Column("ADDRESS")]
        public string Address { get; set; }
        /// <summary>
        /// 车辆类型  0为电厂班车 1为私家车 2为商务公车 3为拜访车辆 4为物料车辆 5危化品车辆
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
        /// 状态 0为进入 1为出门
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public int? Status { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 进入车辆车牌号
        /// </summary>
        /// <returns></returns>
        [Column("CARNO")]
        public string CarNo { get; set; }

        /// <summary>
        /// 状态 0为进入 1为出门
        /// </summary>
        /// <returns></returns>
        [Column("ISLEAVE")]
        public int? IsLeave { get; set; }

        /// <summary>
        /// 只有私家车有ID 授权给他人时为他们ID 其余车辆位空值
        /// </summary>
        /// <returns></returns>
        [Column("DRIVERID")]
        public string DriverID { get; set; }
        /// <summary>
        /// 司机名称
        /// </summary>
        /// <returns></returns>
        [Column("DRIVERNAME")]
        public string DriverName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        /// <returns></returns>
        [Column("PHONE")]
        public string Phone { get; set; }
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