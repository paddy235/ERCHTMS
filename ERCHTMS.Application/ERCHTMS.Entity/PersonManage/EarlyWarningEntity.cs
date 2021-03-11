using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERCHTMS.Entity.PersonManage
{
    /// <summary>
    /// 人员行为安全预警
    /// </summary>
    public class EarlyWarningEntity : BaseEntity
    {
        #region MyRegion
        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }

        /// <summary>
        /// 抓拍图片地址
        /// </summary>
        [Column("PICURL")]
        public string PicUrl { get; set; }

        /// <summary>
        /// 预警内容
        /// </summary>
        [Column("WARNINGCONTENT")]
        public string WarningContent { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        [Column("AREANAME")]
        public string AreaName { get; set; }

        /// <summary>
        /// 区域编码
        /// </summary>
        [Column("AREACODE")]
        public string AreaCode { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        [Column("DUTYPERSON")]
        public string DutyPerson { get; set; }

        /// <summary>
        /// 责任人ID
        /// </summary>
        [Column("DUTYPERSONID")]
        public string DutyPersonId { get; set; }

        /// <summary>
        /// 部门/班组编码
        /// </summary>
        [Column("DEPARTCODE")]
        public string DepartCode { get; set; }

        /// <summary>
        ///  部门/班组
        /// </summary>
        [Column("DEPARTNAME")]
        public string DepartName { get; set; }

        /// <summary>
        /// 预警时间
        /// </summary>
        [Column("WARNINGTIME")]
        public DateTime WarningTime { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [Column("DEVICENAME")]
        public string DeviceName { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        [Column("DEVICEINDEX")]
        public string DeviceIndex { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
        }
        #endregion
    }
}
