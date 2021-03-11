using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.RiskDataBaseConfig
{
    /// <summary>
    /// 描 述：安全风险管控取值配置表
    /// </summary>
    [Table("BIS_RISKWAYCONFIG")]
    public class RiskwayconfigEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建电厂Code
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 风险类别
        /// </summary>
        /// <returns></returns>
        [Column("RISKTYPE")]
        public string RiskType { get; set; }
        /// <summary>
        /// 风险类别
        /// </summary>
        /// <returns></returns>
        [Column("RISKTYPECODE")]
        public string RiskTypeCode { get; set; }
        /// <summary>
        /// 取值类别 LEC PSE 风险矩阵法
        /// </summary>
        /// <returns></returns>
        [Column("WAYTYPE")]
        public string WayType { get; set; }
        /// <summary>
        /// 取值类别
        /// </summary>
        /// <returns></returns>
        [Column("WAYTYPECODE")]
        public string WayTypeCode { get; set; }
        /// <summary>
        /// 所属单位
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }
        /// <summary>
        /// 是否提交 1 提交 0 未提交
        /// </summary>
        /// <returns></returns>
        [Column("ISCOMMIT")]
        public string IsCommit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }

        public List<RiskwayconfigdetailEntity> DetaileList = new List<RiskwayconfigdetailEntity>();
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