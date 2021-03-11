using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// 访客管理-随行人员附件表
    /// </summary>
    [Table("BIS_USERCARFILE_MULTIPLE")]
    public class UserCarFileMultipleEntity : BaseEntity
    {
        #region  实体成员

        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        /// <returns></returns>
        [Column("IMGPATH")]
        public string ImgPath { get; set; }

        /// <summary>
        /// base64格式存储
        /// </summary>
        /// <returns></returns>
        [Column("IMGDATA")]
        public string ImgData { get; set; }

        /// <summary>
        /// 主表Id
        /// </summary>
        /// <returns></returns>
        [Column("BASEID")]
        public string BaseId { get; set; }

        /// <summary>
        /// 随行人员表Id
        /// </summary>
        /// <returns></returns>
        [Column("USERCARFILEID")]
        public string UserCarFileId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
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
