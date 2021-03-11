using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.NosaManage
{
    /// <summary>
    /// 区域代表工作总结管理表
    /// </summary>
    [Table("HRS_NOSAAREAWORKSUMMARY")]
    public class NosaAreaWorkSummaryEntity : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("AUTOID")]
        public string AutoId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 区域id
        /// </summary>
        [Column("AREAID")]
        public string AreaId { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        [Column("AREANAME")]
        public string AreaName { get; set; }
        /// <summary>
        /// 区域代表Id
        /// </summary>
        [Column("AREASUPERID")]
        public string AreaSuperId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("AREASUPER")]
        public string AreaSuper { get; set; }
        /// <summary>
        /// 责任部门
        /// </summary>
        [Column("DUTYDEPART")]
        public string DutyDepart { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("DUTYDEPARTID")]
        public string DutyDepartId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column("DUTYDEPARTCODE")]
        public string DutyDepartCode { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        [Column("MONTH")]
        public DateTime Month { get; set; }
        /// <summary>
        /// 是否提交 1 提交 0 未提交
        /// </summary>
        [Column("ISCOMMIT")]
        public int IsCommit { get; set; }

      
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
    }
}
