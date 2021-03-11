using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.PersonManage
{
    /// <summary>
    /// 禁入记录  
    /// </summary>
    [Table("BIS_FORBIDDENRECORD")]
    public class ForbiddenRecordEntity : BaseEntity
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
        /// 禁入开始时间
        /// </summary>
        /// <returns></returns>
        [Column("STARTTIME")]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 禁入开始时间
        /// </summary>
        /// <returns></returns>
        [Column("ENDTIME")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 禁入原因
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }


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
