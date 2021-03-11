using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.BaseManage
{
    /// <summary>
    /// 描 述：角色管理
    /// </summary>
    [Table("BASE_ROLE")]
    public class RoleEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 角色主键
        /// </summary>		
        [Column("ROLEID")]
        public string RoleId { get; set; }
        /// <summary>
        /// 机构主键
        /// </summary>		
        [Column("ORGANIZEID")]
        public string OrganizeId { get; set; }
        /// <summary>
        /// 分类1-角色2-岗位3-职位4-工作组
        /// </summary>		
        [Column("CATEGORY")]
        public int? Category { get; set; }
        /// <summary>
        /// 角色编码
        /// </summary>		
        [Column("ENCODE")]
        public string EnCode { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>		
        [Column("FULLNAME")]
        public string FullName { get; set; }
        /// <summary>
        /// 公共角色
        /// </summary>		
        [Column("ISPUBLIC")]
        public int? IsPublic { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>		
        [Column("OVERDUETIME")]
        public DateTime? OverdueTime { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>		
        [Column("SORTCODE")]
        public int? SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>		
        [Column("DELETEMARK")]
        public int? DeleteMark { get; set; }
        /// <summary>
        /// 有效标志
        /// </summary>		
        [Column("ENABLEDMARK")]
        public int? EnabledMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        [Column("DESCRIPTION")]
        public string Description { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>		
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>		
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>		
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>		
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>		
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>		
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }

        /// <summary>
        /// 层级
        /// </summary>		
        [Column("NATURE")]
        public string Nature { get; set; }

        /// <summary>
        /// 岗位对应角色合集
        /// </summary>		
        [Column("ROLENAMES")]
        public string RoleNames { get; set; }

        /// <summary>
        /// 岗位对应角色ID合集
        /// </summary>		
        [Column("ROLEIDS")]
        public string RoleIds { get; set; }

        [Column("DEPTNAME")]
        public string DeptName { get; set; }

        [Column("DEPTID")]
        public string DeptId { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            var user = OperatorProvider.Provider.Current();
            this.RoleId = string.IsNullOrEmpty(RoleId) ? Guid.NewGuid().ToString() : RoleId;
            this.CreateDate = DateTime.Now;
            this.CreateUserId =user==null?"":user.UserId;
            this.CreateUserName = user == null ? "" : user.UserName;
            this.DeleteMark = 0;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            var user = OperatorProvider.Provider.Current();
            this.RoleId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = user==null?"":user.UserId;
            this.ModifyUserName = user==null?"":user.UserName;
        }
        #endregion
    }
}