using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：资质审查资质证件表
    /// </summary>
    [Table("EPG_QUALIFICATION")]
    public class QualificationEntity : BaseEntity
    {
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
        /// 资质证件---资质证件号
        /// </summary>
        /// <returns></returns>
        [Column("CQCODE")]
        public string CqCode { get; set; }
        /// <summary>
        /// 资质证件---发证机关
        /// </summary>
        /// <returns></returns>
        [Column("CQORG")]
        public string CqOrg { get; set; }
        /// <summary>
        /// 资质证件---资质范围
        /// </summary>
        /// <returns></returns>
        [Column("CQRANGE")]
        public string CqRange { get; set; }
        /// <summary>
        /// 资质证件---发证等级
        /// </summary>
        /// <returns></returns>
        [Column("CQLEVEL")]
        public string CqLevel { get; set; }
        /// <summary>
        /// 资质证件---有效期起
        /// </summary>
        /// <returns></returns>
        [Column("CQVALIDSTARTTIME")]
        public DateTime? CqValidstarttime { get; set; }
        /// <summary>
        /// 资质证件---有效期止
        /// </summary>
        /// <returns></returns>
        [Column("CQVALIDENDTIME")]
        public DateTime? CqValidendtime { get; set; }
        /// <summary>
        /// 单位资质审查Id
        /// </summary>
        /// <returns></returns>
        [Column("INFOID")]
        public string InfoId { get; set; }

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
