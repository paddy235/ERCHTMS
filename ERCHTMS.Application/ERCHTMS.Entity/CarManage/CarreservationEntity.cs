using System;
using System.Collections.Generic;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// 描 述：班车预约记录
    /// </summary>
    [Table("BIS_CARRESERVATION")]
    public class CarreservationEntity : BaseEntity
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
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 0中午12点 1下午17点
        /// </summary>
        /// <returns></returns>
        [Column("TIME")]
        public int? Time { get; set; }
        /// <summary>
        /// 预约日期
        /// </summary>
        /// <returns></returns>
        [Column("RESDATE")]
        public DateTime? RESDate { get; set; }
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
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 进入车辆车牌号
        /// </summary>
        /// <returns></returns>
        [Column("CARNO")]
        public string CarNo { get; set; }

        /// <summary>
        /// 起始地点
        /// </summary>
        /// <returns></returns>
        [Column("SADDRESS")]
        public string Saddress { get; set; }

        /// <summary>
        /// 结束地点
        /// </summary>
        /// <returns></returns>
        [Column("EADDRESS")]
        public string Eaddress { get; set; }

        /// <summary>
        /// 数据类型 0司机预约 1普通员工预约
        /// </summary>
        [Column("DATATYPE")]
        public int DataType { get; set; }

        /// <summary>
        /// 预约主键Id
        /// </summary>
        [Column("BASEID")]
        public string BaseId { get; set; }


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

    public class ReserVation
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string CarNo { get; set; }
        /// <summary>
        /// 车辆ID
        /// </summary>
        public string CID { get; set; }
        /// <summary>
        /// 车辆型号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 荷载人数
        /// </summary>
        public int NumberLimit { get; set; }

        /// <summary>
        /// 预约详情
        /// </summary>
        public List<ReserList> RList { get; set; }

    }

    public class ReserList
    {
        /// <summary>
        /// 时间 0为中午 1为下午
        /// </summary>
        public int Time { get; set; }
        /// <summary>
        /// 预约数量
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 日期格式字符串
        /// </summary>
        public string DateStr { get; set; }

        /// <summary>
        /// 当前用户是否预约
        /// </summary>
        public int IsReser { get; set; }
    }
}