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
    /// 描 述：检查名称字典表
    /// </summary>
    [Table("BIS_CHECKNAMESET")]
    public class CheckNameSetEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 检查名称
        /// </summary>
        /// <returns></returns>
        [Column("CHECKNAME")]
        public string CheckName { get; set; }

        /// <summary>
        /// 状态（0：禁用,1：启用）
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public int? Status { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        /// <returns></returns>
        [Column("SORTCODE")]
        public int? SortCode { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
       
        /// <summary>
        ///创建部门编码
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        ///创建机构编码
        /// </summary>
        /// <returns></returns>
        [Column("ORGCODE")]
        public string OrgCode { get; set; }
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
                OrgCode = user.OrganizeCode;
                CreateDate = DateTime.Now;
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
