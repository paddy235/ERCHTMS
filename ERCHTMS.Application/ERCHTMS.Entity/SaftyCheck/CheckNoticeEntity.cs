using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ERCHTMS.Entity.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查公示表
    /// </summary>
    [Table("BIS_CHECKNOTICE")]
    public class CheckNoticeEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 关联检查记录ID
        /// </summary>
        /// <returns></returns>
        [Column("CHECKID")]
        public string CheckId { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        /// <returns></returns>
        [Column("YEAR")]
        public int? Year { get; set; }
        /// <summary>
        ///检查类型
        /// </summary>
        /// <returns></returns>
        [Column("CHECKTYPE")]
        public string CheckType { get; set; }
        /// <summary>
        ///时间类型
        /// </summary>
        /// <returns></returns>
        [Column("TIMETYPE")]
        public string TimeType { get; set; }
        /// <summary>
        ///开始日期
        /// </summary>
        /// <returns></returns>
        [Column("STARTDATE")]
        public DateTime? StartDate { get; set; }
        /// <summary>
        ///结束日期
        /// </summary>
        /// <returns></returns>
        [Column("ENDDATE")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        ///部门编码
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        ///备用字段
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        ///CreateUserId
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        ///CreateUserName
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
       
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = Guid.NewGuid().ToString();
            var user = OperatorProvider.Provider.Current();
            if (user!=null)
            {
                CreateUserId = user.UserId;
                CreateUserName = user.UserName;
                DeptCode = user.DeptCode;
            }
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
