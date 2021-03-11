using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HazardsourceManage
{
    /// <summary>
    /// 描 述：历史记录
    /// </summary>
    [Table("HSD_HISRELATIONHD")]
    public class HisrelationhdEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 安全控制措施
        /// </summary>
        /// <returns></returns>
        [Column("MEASURENUM")]
        public int? MeaSureNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("HAZARDSOURCEID")]
        public string HazardSourceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ITEMDECR")]
        public decimal? ItemDecR { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ITEMDECQ1")]
        public string ItemDecQ1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ITEMDECQ")]
        public string ItemDecQ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ITEMDECB")]
        public decimal? ItemDecB { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ITEMDECR1")]
        public decimal? ItemDecR1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ITEMDECB1")]
        public string ItemDecB1 { get; set; }





        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("WAY")]
        public string Way { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 风险类别
        /// </summary>
        /// <returns></returns>
        [Column("RISKTYPE")]
        public string RiskType { get; set; }
        /// <summary>
        /// 评价项目R
        /// </summary>
        /// <returns></returns>
        [Column("ITEMR")]
        public decimal? ItemR { get; set; }
        /// <summary>
        /// 评价项目C
        /// </summary>
        /// <returns></returns>
        [Column("ITEMC")]
        public decimal? ItemC { get; set; }
        /// <summary>
        /// 评价项目B
        /// </summary>
        /// <returns></returns>
        [Column("ITEMB")]
        public decimal? ItemB { get; set; }
        /// <summary>
        /// 评价项目A
        /// </summary>
        /// <returns></returns>
        [Column("ITEMA")]
        public decimal? ItemA { get; set; }


        /// <summary>
        /// 风险数字等级(与风险等级对应)
        /// </summary>
        /// <returns></returns>
        [Column("GRADEVAL")]
        public int? GradeVal { get; set; }


        /// <summary>
        /// 风险等级
        /// </summary>
        /// <returns></returns>
        [Column("GRADE")]
        public string Grade { get; set; }

        /// <summary>
        /// 危险源名称/场所
        /// </summary>
        /// <returns></returns>
        [Column("DANGERSOURCE")]
        public string DangerSource { get; set; }
        /// <summary>
        /// 修改记录时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 创建用户ID
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 事故类型
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTTYPE")]
        public string AccidentType { get; set; }
        /// <summary>
        /// 监督管理责任人
        /// </summary>
        /// <returns></returns>
        [Column("JDGLZRRUSERID")]
        public string JdglzrrUserId { get; set; }
        /// <summary>
        /// 安全控制措施
        /// </summary>
        /// <returns></returns>
        [Column("MEASURE")]
        public string MeaSure { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 事故类型名称
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTNAME")]
        public string AccidentName { get; set; }
        /// <summary>
        /// 所属区域ID
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTID")]
        public string DistrictId { get; set; }
        /// <summary>
        /// 修改用户名称
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 状态 1、模板导入 2、平台导入 3、新增
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public int? Status { get; set; }
        /// <summary>
        /// 安全控制措施
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// 风险清单主键ID
        /// </summary>
        /// <returns></returns>
        [Column("RISKASSESSID")]
        public string RiskassessId { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 监督管理责任人
        /// </summary>
        /// <returns></returns>
        [Column("JDGLZRRFULLNAME")]
        public string JdglzrrFullName { get; set; }
        /// <summary>
        /// 所属区域名称
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTNAME")]
        public string DistrictName { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建用户ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// 是否为重大危险源0、否 1、是
        /// </summary>
        /// <returns></returns>
        [Column("ISDANGER")]
        public int? IsDanger { get; set; }
        /// <summary>
        /// 责任部门  
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
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
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}