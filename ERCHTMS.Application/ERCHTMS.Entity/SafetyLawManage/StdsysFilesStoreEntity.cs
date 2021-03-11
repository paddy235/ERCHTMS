using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SafetyLawManage
{
    /// <summary>
    /// 描 述：标准制度收藏文件
    /// </summary>
    [Table("HRS_STDSYSSTOREFILES")]
    public class StdsysFilesStoreEntity : BSEntity
    {
        #region 实体成员     
        /// <summary>
        /// 用户编号
        /// </summary>                           
        [Column("USERID")]        
        public string UserId { get; set; }
        /// <summary>
        /// 标准制度编号
        /// </summary>
        [Column("STDSYSID")]
        public string StdSysId { get; set; }
        #endregion
    }
}


