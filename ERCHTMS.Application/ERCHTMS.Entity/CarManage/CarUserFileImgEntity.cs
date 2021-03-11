using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// 描述：（拜访人员、拜访车辆、危化品车辆）表子表
    /// </summary>
    [Table("BIS_USERCARFILEIMG")]
    public class CarUserFileImgEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string Username { get; set; }
        /// <summary>
        /// 用户上传照片
        /// </summary>
        /// <returns></returns>
        [Column("USERIMG")]
        public string Userimg { get; set; }

        /// <summary>
        /// 主表ID
        /// </summary>
        /// <returns></returns>
        [Column("BASEID")]
        public string Baseid { get; set; }
        /// <summary>
        /// 定位设备ID
        /// </summary>
        /// <returns></returns>
        [Column("GPSID")]
        public string GPSID { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        /// <returns></returns>
        [Column("GPSNAME")]
        public string GPSNAME { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string REMARK { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        [Column("ORDERNUM")]
        public int OrderNum { get; set; }

        /// <summary>
        /// 人脸图片(base64格式存储)
        /// </summary>
        /// <returns></returns>
        [Column("IMGDATA")]
        public string Imgdata { get; set; }

        /// <summary>
        /// 随行人员附件集合（防疫行程卡（健康码/核酸检测证明））
        /// </summary>
        public List<UserCarFileMultipleEntity> FileItems { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
        }
        #endregion
    }
}
