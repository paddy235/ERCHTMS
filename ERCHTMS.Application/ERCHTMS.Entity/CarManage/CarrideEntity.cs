using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// 描 述：班车上车记录表
    /// </summary>
    [Table("BIS_CARRIDE")]
    public class CarrideEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 进入车辆车牌号
        /// </summary>
        /// <returns></returns>
        [Column("CARNO")]
        public string CarNo { get; set; }
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
        /// 经过门岗（1号岗  3号岗）
        /// </summary>
        /// <returns></returns>
        [Column("ADDRESS")]
        public string Address { get; set; }
        /// <summary>
        /// 进出记录ID
        /// </summary>
        /// <returns></returns>
        [Column("LID")]
        public string Lid { get; set; }
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
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 车辆关联ID
        /// </summary>
        /// <returns></returns>
        [Column("CID")]
        public string CID { get; set; }
        /// <summary>
        /// 车辆类型
        /// </summary>
        /// <returns></returns>
        [Column("TYPE")]
        public int? Type { get; set; }
        /// <summary>
        /// 扫码时间
        /// </summary>
        /// <returns></returns>
        [Column("SCANCODETIME")]
        public DateTime? ScancodeTime { get; set; }
        /// <summary>
        /// 状态 是否经过门岗 默认为0  未经过 1为已经过
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public int? Status { get; set; }
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