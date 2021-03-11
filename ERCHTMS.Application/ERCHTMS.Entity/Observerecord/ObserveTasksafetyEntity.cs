using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.Observerecord
{
    /// <summary>
    /// 描 述：观察记录安全行为
    /// </summary>
    [Table("BIS_OBSERVETASKSAFETY")]
   public class ObserveTasksafetyEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 行为描述
        /// </summary>
        /// <returns></returns>
        [Column("BEHAVIORDESC")]
        public string BehaviorDesc { get; set; }
        /// <summary>
        /// 观察记录Id
        /// </summary>
        /// <returns></returns>
        [Column("RECORDID")]
        public string Recordid { get; set; }
        /// <summary>
        /// 观察类别
        /// </summary>
        /// <returns></returns>
        [Column("OBSERVETYPE")]
        public string Observetype { get; set; }
        /// <summary>
        /// 预防措施
        /// </summary>
        /// <returns></returns>
        [Column("PREVENTIVEMEASURES")]
        public string PreventiveMeasures { get; set; }
        /// <summary>
        /// 备注字段
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
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
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 整改责任人
        /// </summary>
        /// <returns></returns>
        [Column("PERSONRESPONSIBLE")]
        public string PersonreSponsible { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 整改期限
        /// </summary>
        /// <returns></returns>
        [Column("REFORMDEADLINE")]
        public string ReformDeadline { get; set; }
        /// <summary>
        /// 纠正措施
        /// </summary>
        /// <returns></returns>
        [Column("REMEDIALACTION")]
        public string RemedialAction { get; set; }
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
        [Column("OBSERVETYPENAME")]
        public string ObservetypeName { get; set; }
        /// <summary>
        /// 直接原因
        /// </summary>
        /// <returns></returns>
        [Column("IMMEDIATECAUSE")]
        public string ImmediateCause { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 间接原因
        /// </summary>
        /// <returns></returns>
        [Column("REMOTECAUSE")]
        public string RemoteCause { get; set; }
        /// <summary>
        /// 是否安全行为 1 安全行为 0 不安全行为
        /// </summary>
        /// <returns></returns>
        [Column("ISSAFETY")]
        public int? IsSafety { get; set; }
        [Column("PERSONRESPONSIBLEID")]
        public string PersonreSponsibleId { get; set; }
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
