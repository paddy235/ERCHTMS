using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：标准制度收藏文件
    /// </summary>
    [Table("BIS_FILEMANAGESTORE")]
    public class FileManageStoreEntity : BSEntity
    {
        #region 实体成员
        /// <summary>
        /// 用户编号
        /// </summary>                           
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 文件ID
        /// </summary>
        [Column("FILEMANAGEID")]
        public string FileManageId { get; set; }
        #endregion
    }
}
