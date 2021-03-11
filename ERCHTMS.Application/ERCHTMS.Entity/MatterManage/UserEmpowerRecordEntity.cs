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
    /// 用户授权记录表
    /// </summary>
    [Table("WL_USEREMPOWERRECORD")]
    public class UserEmpowerRecordEntity : BaseEntity
    {
        #region 基本业务
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
        /// 授权人
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
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户ID
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户名称
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        /// <returns></returns>
        [Column("ACCOUNT")]
        public string Account { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <returns></returns>
        [Column("REALNAME")]
        public string RealName { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }

        /// <summary>
        /// 是否授权1已授权0未授权
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public string Status { get; set; }
        /// <summary>
        /// 授权时间起
        /// </summary>
        /// <returns></returns>
        [Column("STARTTIME")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 授权时间止
        /// </summary>
        /// <returns></returns>
        [Column("ENDTIME")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 备用
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用(授权记录)
        /// </summary>
        public override void Create()
        {
            this.ID = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
        }

        #endregion
        
    }
}
