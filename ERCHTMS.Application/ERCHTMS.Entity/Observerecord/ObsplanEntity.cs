using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.Observerecord
{
    /// <summary>
    /// 描 述：观察计划
    /// </summary>
    [Table("BIS_OBSPLAN")]
    public class ObsplanEntity : BaseEntity
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
        /// 
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
        /// 计划年度
        /// </summary>
        /// <returns></returns>
        [Column("PLANYEAR")]
        public string PlanYear { get; set; }
        /// <summary>
        /// 计划部门
        /// </summary>
        /// <returns></returns>
        [Column("PLANDEPT")]
        public string PlanDept { get; set; }
        /// <summary>
        /// 计划部门
        /// </summary>
        /// <returns></returns>
        [Column("PLANDEPTID")]
        public string PlanDeptId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("PLANDEPTCODE")]
        public string PlanDeptCode { get; set; }
        /// <summary>
        /// 计划专业
        /// </summary>
        /// <returns></returns>
        [Column("PLANSPECIATY")]
        public string PlanSpeciaty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("PLANSPECIATYCODE")]
        public string PlanSpeciatyCode { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        /// <returns></returns>
        [Column("PLANAREA")]
        public string PlanArea { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("PLANAREACODE")]
        public string PlanAreaCode { get; set; }
        /// <summary>
        /// 作业内容
        /// </summary>
        /// <returns></returns>
        [Column("WORKNAME")]
        public string WorkName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 计划等级
        /// </summary>
        /// <returns></returns>
        [Column("PLANLEVEL")]
        public string PlanLevel { get; set; }
        [Column("ISCOMMIT")]
        public string Iscommit { get; set; }
        /// <summary>
        /// 是否发布 0 未发布 1 已发布
        /// </summary>
        [Column("ISPUBLIC")]
        public string IsPublic { get; set; }
        /// <summary>
        /// EHS是否已经提交 0 未提交 1 已经提交
        /// </summary>
        [Column("ISEMSCOMMIT")]
        public string IsEmsCommit { get; set; }

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