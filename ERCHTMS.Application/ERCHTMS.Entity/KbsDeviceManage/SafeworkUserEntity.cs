using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.KbsDeviceManage
{
    /// <summary>
    /// 作业管理监管成员子表
    /// </summary>
    [Table("BIS_SAFEWORKUSER")]
    public class SafeworkUserEntity : BaseEntity
    {
         
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// 作业主键
        /// </summary>
        /// <returns></returns>
        [Column("WORKID")]
        public string workid { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string username { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string userid { get; set; }

        /// <summary>
        /// 监护人类型 0 施工单位 1 主管部门  2 安全监察部 3 工作负责人
        /// </summary>
        /// <returns></returns>
        [Column("TYPE")]
        public int type { get; set; }

        /// <summary>
        /// 是否在监管区域 状态 0 否 1是
        /// </summary>
        [Column("STATE")]
        public int state { get; set; }

        /// <summary>
        /// 备用
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string REMARK { get; set; }

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = DateTime.Now;

        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
        }
        #endregion


    }
}
