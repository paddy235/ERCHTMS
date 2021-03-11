using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病人员表
    /// </summary>
    [Table("BIS_OCCUPATIOALSTAFF")]
    public class OccupatioalstaffEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 记录编号
        /// </summary>
        /// <returns></returns>
        [Column("OCCID")]
        public string OccId { get; set; }
        /// <summary>
        /// 体检机构名称
        /// </summary>
        /// <returns></returns>
        [Column("MECHANISMNAME")]
        public string MechanismName { get; set; }
        /// <summary>
        /// 体检时间
        /// </summary>
        /// <returns></returns>
        [Column("INSPECTIONTIME")]
        public DateTime? InspectionTime { get; set; }
        /// <summary>
        /// 体检人数
        /// </summary>
        /// <returns></returns>
        [Column("INSPECTIONNUM")]
        public int? InspectionNum { get; set; }
        /// <summary>
        /// 职业病人数
        /// </summary>
        /// <returns></returns>
        [Column("PATIENTNUM")]
        public int? PatientNum { get; set; }
        /// <summary>
        /// 是否有附件
        /// </summary>
        /// <returns></returns>
        [Column("ISANNEX")]
        public int? IsAnnex { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserid { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }

        /// <summary>
        /// 异常人数
        /// </summary>
        [Column("UNUSUALNUM")]
        public int? UnusualNum { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.OccId = string.IsNullOrEmpty(OccId) ? Guid.NewGuid().ToString() : OccId;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.OccId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserid = OperatorProvider.Provider.Current().UserId;
        }
        #endregion
    }
}