using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.MatterManage
{
    /// <summary>
    /// 工作日志
    /// </summary>
    [Table("wl_dailyrRecord")]
    public class DailyrRecordEntity : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// 创建用户Id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户名称
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }


        /// <summary>
        /// 主题
        /// </summary>
        /// <returns></returns>
        [Column("THEME")]
        public string Theme { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        /// <returns></returns>
        [Column("CONTENT")]
        public string Content { get; set; }
        /// <summary>
        /// 日志类型1入场开票 2车辆出厂 3称重计量
        /// </summary>
        /// <returns></returns>
        [Column("WORKTYPE")]
        public int WorkType { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }

        /// <summary>
        /// 备用
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }

        #region 扩展操作
        /// <summary>
        /// 新增调用(工作记录)
        /// </summary>
        public override void Create()
        {
            this.ID = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.UserName = this.CreateUserName;
        }

        #endregion

    }
    
}
