using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.BaseManage
{
    /// <summary>
    /// 描 述：部门管理
    /// </summary>
    [Table("BASE_DEPARTMENT")]
    public class DepartmentEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 部门主键
        /// </summary>	
        [Column("DEPARTMENTID")]
        public string DepartmentId { get; set; }
        /// <summary>
        /// 机构主键
        /// </summary>		
        [Column("ORGANIZEID")]
        public string OrganizeId { get; set; }
        /// <summary>
        /// 父级主键
        /// </summary>		
        [Column("PARENTID")]
        public string ParentId { get; set; }
        /// <summary>
        /// 部门代码
        /// </summary>		
        [Column("ENCODE")]
        public string EnCode { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>		
        [Column("FULLNAME")]
        public string FullName { get; set; }
        /// <summary>
        /// 部门简称
        /// </summary>		
        [Column("SHORTNAME")]
        public string ShortName { get; set; }
        /// <summary>
        /// 部门类型
        /// </summary>		
        [Column("NATURE")]
        public string Nature { get; set; }
        /// <summary>
        /// 负责人主键
        /// </summary>		
        [Column("MANAGERID")]
        public string ManagerId { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>		
        [Column("MANAGER")]
        public string Manager { get; set; }
        /// <summary>
        /// 外线电话
        /// </summary>		
        [Column("OUTERPHONE")]
        public string OuterPhone { get; set; }
        /// <summary>
        /// 内线电话
        /// </summary>		
        [Column("INNERPHONE")]
        public string InnerPhone { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>		
        [Column("EMAIL")]
        public string Email { get; set; }
        /// <summary>
        /// 部门传真
        /// </summary>		
        [Column("FAX")]
        public string Fax { get; set; }
        /// <summary>
        /// 层
        /// </summary>		
        [Column("LAYER")]
        public int? Layer { get; set; }
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
        /// 部门职责
        /// </summary>		
        [Column("DEPARTDUTY")]
        public string DepartDuty { get; set; }
        /// <summary>
        /// 安全风险管控区域主键
        /// </summary>		
        [Column("DISTRICTID")]
        public string DistrictID { get; set; }
        /// <summary>
        /// 安全风险管控区域名称
        /// </summary>		
        [Column("DISTRICTNAME")]
        public string DistrictName { get; set; }
        /// <summary>
        /// 是否厂级部门(0不是1是)
        /// </summary>		
        [Column("ISORG")]
        public int IsOrg { get; set; }
        /// <summary>
        /// 是否有子集
        /// </summary>		
        [Column("HASCHILD")]
        public string HasChild { get; set; }
        /// <summary>
        /// 发包单位主键
        /// </summary>		
        [Column("SENDDEPTID")]
        public string SendDeptID { get; set; }
        /// <summary>
        /// 发包单位名称
        /// </summary>		
        [Column("SENDDEPTNAME")]
        public string SendDeptName { get; set; }
        /// <summary>
        /// 关联风险库中部门Id
        /// </summary>		
        [Column("RELATEDDEPTID")]
        public string RelatedDeptId { get; set; }
      
        /// <summary>
        /// 关联风险库中部门
        /// </summary>		
        [Column("RELATEDDEPTNAME")]
        public string RelatedDeptName { get; set; }
        /// <summary>
        /// 新的部门编码
        /// </summary>		
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// 是否对接培训云平台
        /// </summary>		
        [Column("ISTRAIN")]
        public int? IsTrain { get; set; }
        /// <summary>
        /// 关联培训平台部门信息
        /// </summary>		
        [Column("DEPTKEY")]
        public string DeptKey { get; set; }
        /// <summary>
        /// 是否对接培训工具箱
        /// </summary>		
        [Column("ISTOOLS")]
        public int? IsTools { get; set; }
        /// <summary>
        /// 关联工具箱部门信息
        /// </summary>		
        [Column("TOOLSKEY")]
        public string ToolsKey { get; set; }
        /// <summary>
        /// 行业（如:电力,建筑，水利,交通,石化）
        /// </summary>		
        [Column("INDUSTRY")]
        public string Industry { get; set; }
        /// <summary>
        /// 关联科技MIS系统部门Id
        /// </summary>		
        [Column("MISDEPTID")]
        public string MisDeptId { get; set; }
        /// <summary>
        /// 班组类别 (01: 运行，02：检修，03：其他)
        /// </summary>		
        [Column("TEAMTYPE")]
        public string TeamType { get; set; }

        /// <summary>
        ///专业类别
        /// </summary>
        [Column("SPECIALTYTYPE")]
        public string SpecialtyType { get; set; }
        /// <summary>
        ///部门类别
        /// </summary>
        [Column("DEPTTYPE")]
        public string DeptType { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            var user = OperatorProvider.Provider.Current();
            this.DepartmentId = string.IsNullOrEmpty(DepartmentId) ? Guid.NewGuid().ToString() : DepartmentId;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = user==null?"":user.UserId;
            this.CreateUserName = user == null ? "" : user.UserName;
            this.DeleteMark = 0;
            this.SortCode = SortCode == null ? 0 : SortCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            var user = OperatorProvider.Provider.Current();
            this.DepartmentId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = user == null ? "" : user.UserId;
            this.ModifyUserName = user == null ? "" : user.UserName;
        }
        #endregion
    }
}