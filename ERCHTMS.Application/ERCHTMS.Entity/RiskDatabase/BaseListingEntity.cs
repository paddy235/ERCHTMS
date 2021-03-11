using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.RiskDatabase
{
    /// <summary>
    /// 描 述：作业活动及设备设施清单
    /// </summary>
    [Table("BIS_BASELISTING")]
    public class BaseListingEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        /// <returns></returns>
        [Column("AREANAME")]
        public string AreaName { get; set; }
        /// <summary>
        /// 是否特种设备 0：是 1：否
        /// </summary>
        /// <returns></returns>
        [Column("ISSPECIALEQU")]
        public int? IsSpecialEqu { get; set; }
        /// <summary>
        /// 作业活动名称/设备名称
        /// </summary>
        /// <returns></returns>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 修改用户名称
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        /// <returns></returns>
        [Column("OTHERS")]
        public string Others { get; set; }
        /// <summary>
        /// 修改记录时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 是否常规  0：常规 1：非常规
        /// </summary>
        /// <returns></returns>
        [Column("ISCONVENTIONAL")]
        public int? IsConventional { get; set; }
        /// <summary>
        /// 区域id
        /// </summary>
        /// <returns></returns>
        [Column("AREAID")]
        public string AreaId { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 活动步骤
        /// </summary>
        /// <returns></returns>
        [Column("ACTIVITYSTEP")]
        public string ActivityStep { get; set; }
        /// <summary>
        /// 修改用户ID
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 创建用户姓名
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 区域Code
        /// </summary>
        /// <returns></returns>
        [Column("AREACODE")]
        public string AreaCode { get; set; }
        /// <summary>
        /// 创建用户ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 类型 0：作业活动清单 1：设备设施清单
        /// </summary>
        [Column("TYPE")]
        public int Type { get; set; }
        /// <summary>
        /// 管控部门
        /// </summary>
        [Column("CONTROLSDEPT")]
        public string ControlsDept { get; set; }
        /// <summary>
        /// 管控部门
        /// </summary>
        [Column("CONTROLSDEPTID")]
        public string ControlsDeptId { get; set; }
        /// <summary>
        /// 管控部门
        /// </summary>
        [Column("CONTROLSDEPTCODE")]
        public string ControlsDeptCode { get; set; }

        /// <summary>
        /// 岗位(工种)
        /// </summary>
        [Column("POST")]
        public string Post { get; set; }

        /// <summary>
        /// 岗位(工种)
        /// </summary>
        [Column("POSTID")]
        public string PostId { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
            this.CreateDate = this.CreateDate == null ? DateTime.Now : this.CreateDate;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
           this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
           this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}