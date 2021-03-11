using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.Observerecord
{
    /// <summary>
    /// 描 述：观察计划任务分解
    /// </summary>
    [Table("BIS_OBSPLANWORK")]
    public class ObsplanworkEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 计划Id
        /// </summary>
        /// <returns></returns>
        [Column("PLANID")]
        public string PlanId { get; set; }
        /// <summary>
        /// 任务分解名称
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
        /// 观察人员
        /// </summary>
        /// <returns></returns>
        [Column("OBSPERSON")]
        public string ObsPerson { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("OBSPERSONID")]
        public string ObsPersonId { get; set; }
        /// <summary>
        /// 风险等级
        /// </summary>
        /// <returns></returns>
        [Column("RISKLEVEL")]
        public string RiskLevel { get; set; }
        /// <summary>
        /// 观察频率实际值
        /// </summary>
        /// <returns></returns>
        [Column("OBSNUM")]
        public string ObsNum { get; set; }
          /// <summary>
        /// 观察频率显示值
        /// </summary>
        /// <returns></returns>
        [Column("OBSNUMTEXT")]
        public string ObsNumText { get; set; }
        
        /// <summary>
        /// 计划观察月份
        /// </summary>
        /// <returns></returns>
        [Column("OBSMONTH")]
        public string ObsMonth { get; set; }

        [Column("OLDWORKID")]
        public string OldWorkId { get; set; }
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