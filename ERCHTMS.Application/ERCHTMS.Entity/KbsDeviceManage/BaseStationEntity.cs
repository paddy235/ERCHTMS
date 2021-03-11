using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.KbsDeviceManage
{
   /// <summary>
   ///描述：基站管理
   /// </summary>
    [Table("BIS_BASESTATION")]
    public class BaseStationEntity : BaseEntity
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
        /// 基站ID
        /// </summary>
        /// <returns></returns>
        [Column("STATIONID")]
        public string StationID { get; set; }
        /// <summary>
        /// 基站名称
        /// </summary>
        /// <returns></returns>
        [Column("STATIONNAME")]
        public string StationName { get; set; }
        /// <summary>
        /// 基站型号
        /// </summary>
        /// <returns></returns>
        [Column("STATIONTYPE")]
        public string StationType { get; set; }

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
        /// 区域Id
        /// </summary>
        /// <returns></returns>
        [Column("AREAID")]
        public string AreaId { get; set; }

        /// <summary>
        /// 楼层编号
        /// </summary>
        /// <returns></returns>
        [Column("FLOORCODE")]
        public string FloorCode { get; set; }
        /// <summary>
        /// 基站坐标
        /// </summary>
        /// <returns></returns>
        [Column("STATIONCODE")]
        public string StationCode { get; set; }

        /// <summary>
        /// 基站IP
        /// </summary>
        /// <returns></returns>
        [Column("STATIONIP")]
        public string StationIP { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        /// <returns></returns>
        [Column("OPERUSERNAME")]
        public string OperUserName { get; set; }

        /// <summary>
        ///基站状态（表中排除）
        /// </summary>
        [NotMapped]
        public string StationState { get; set; }

        /// <summary>
        /// 备用
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 在线或离线
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
            this.OperUserName = OperatorProvider.Provider.Current().UserName;
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
            this.OperUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}
