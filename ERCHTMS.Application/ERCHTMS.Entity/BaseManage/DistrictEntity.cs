using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.BaseManage
{
    /// <summary>
    /// 描 述：区域设置
    /// </summary>
    [Table("BIS_DISTRICT")]
    public class DistrictEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 区域主键
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTID")]
        public string DistrictID { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPT")]
        public string BelongDept { get; set; }
        /// <summary>
        /// 部门负责人
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCHARGEPERSON")]
        public string DeptChargePerson { get; set; }

        /// <summary>
        /// 部门负责人主键
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCHARGEPERSONID")]
        public string DeptChargePersonID { get; set; }
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
        /// 排序
        /// </summary>
        /// <returns></returns>
        [Column("SORTCODE")]
        public int? SortCode { get; set; }
        /// <summary>
        /// 区域负责人
        /// </summary>
        /// <returns></returns>
        [Column("DISREICTCHARGEPERSON")]
        public string DisreictChargePerson { get; set; }

        /// <summary>
        /// 区域负责人主键
        /// </summary>
        /// <returns></returns>
        [Column("DISREICTCHARGEPERSONID")]
        public string DisreictChargePersonID { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTNAME")]
        public string DistrictName { get; set; }
        /// <summary>
        /// 管控部门
        /// </summary>
        /// <returns></returns>
        [Column("CHARGEDEPT")]
        public string ChargeDept { get; set; }

        /// <summary>
        /// 管控部门主键
        /// </summary>
        /// <returns></returns>
        [Column("CHARGEDEPTID")]
        public string ChargeDeptID { get; set; }
        /// <summary>
        /// 管控部门CODE
        /// </summary>
        /// <returns></returns>
        [Column("CHARGEDEPTCODE")]
        public string ChargeDeptCode { get; set; }

        /// <summary>
        /// 区域编码
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTCODE")]
        public string DistrictCode { get; set; }
        
        /// <summary>
        /// 所属公司
        /// </summary>
        /// <returns></returns>
        [Column("BELONGCOMPANY")]
        public string BelongCompany { get; set; }
        /// <summary>
        /// 父级主键
        /// </summary>
        /// <returns></returns>
        [Column("PARENTID")]
        public string ParentID { get; set; }

        /// <summary>
        /// 公司主键
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZEID")]
        public string OrganizeId { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        /// <returns></returns>
        [Column("LINKMAN")]
        public string LinkMan { get; set; }
        /// <summary>
        /// 联系人邮箱
        /// </summary>
        /// <returns></returns>
        [Column("LINKEMAIL")]
        public string LinkMail { get; set; }
       
        /// <summary>
        /// 联系电话
        /// </summary>
        /// <returns></returns>
        [Column("LINKTEL")]
        public string LinkTel { get; set; }

        /// <summary>
        /// 创建人所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }

        /// <summary>
        /// 创建人所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        /// <summary>
        /// 关联公司的区域
        /// </summary>
        /// <returns></returns>
        [Column("LINKTOCOMPANY")]
        public string LinkToCompany { get; set; }

        /// <summary>
        /// 关联公司的区域ID
        /// </summary>
        /// <returns></returns>
        [Column("LINKTOCOMPANYID")]
        public string LinkToCompanyID { get; set; }
        /// <summary>
        /// 区域坐标
        /// </summary>
        /// <returns></returns>
        [Column("LATLNG")]
        public string LatLng { get; set; }

        /// <summary>
        /// 安全监察部门区域负责人
        /// </summary>
        [Column("SAFETYDEPTCHARGEPERSON")]
        public string SafetyDeptChargePerson { get; set; }

        /// <summary>
        /// 安全监察部门区域负责人
        /// </summary>
        [Column("SAFETYDEPTCHARGEPERSONID")]
        public string SafetyDeptChargePersonID { get; set; }

        /// <summary>
        /// 安全监察部门区域负责人联系电话
        /// </summary>
        [Column("SAFETYLINKTEL")]
        public string SafetyLinkTel { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.DistrictID = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
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
            this.DistrictID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}