using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.PowerPlantInside
{
    /// <summary>
    /// 描 述：事故事件验收
    /// </summary>
    [Table("BIS_POWERPLANTCHECK")]
    public class PowerplantcheckEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 验收人ID
        /// </summary>
        /// <returns></returns>
        [Column("AUDITPEOPLEID")]
        public string AuditPeopleId { get; set; }
        /// <summary>
        /// 验收人
        /// </summary>
        /// <returns></returns>
        [Column("AUDITPEOPLE")]
        public string AuditPeople { get; set; }
        /// <summary>
        /// 验收时间
        /// </summary>
        /// <returns></returns>
        [Column("AUDITTIME")]
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 验收部门ID
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDEPTID")]
        public string AuditDeptId { get; set; }
        /// <summary>
        /// 验收部门
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDEPT")]
        public string AuditDept { get; set; }
        /// <summary>
        /// 验收意见
        /// </summary>
        /// <returns></returns>
        [Column("AUDITOPINION")]
        public string AuditOpinion { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 电子签名
        /// </summary>
        /// <returns></returns>
        [Column("AUDITSIGNIMG")]
        public string AuditSignImg { get; set; }
        /// <summary>
        /// 是否失效 0:有效 1:失效
        /// </summary>
        /// <returns></returns>
        [Column("DISABLE")]
        public int? Disable { get; set; }
        /// <summary>
        /// 审核流程Id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }
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
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 关联事故事件处理记录ID
        /// </summary>
        /// <returns></returns>
        [Column("POWERPLANTHANDLEID")]
        public string PowerPlantHandleId { get; set; }
        /// <summary>
        /// 关联事故事件处理记录详细ID
        /// </summary>
        /// <returns></returns>
        [Column("POWERPLANTHANDLEDETAILID")]
        public string PowerPlantHandleDetailId { get; set; }
        /// <summary>
        /// 关联事故事件整改ID
        /// </summary>
        /// <returns></returns>
        [Column("POWERPLANTREFORMID")]
        public string PowerPlantReformId { get; set; }
        /// <summary>
        /// 验收结果
        /// </summary>
        /// <returns></returns>
        [Column("AUDITRESULT")]
        public int? AuditResult { get; set; }

        [NotMapped]
        public IList<Photo> filelist { get; set; } //附件
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
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
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }

    //文件实体
    public class Photo
    {
        public string fileid { get; set; }
        public string filename { get; set; }
        public string fileurl { get; set; }

        public string folderid { get; set; }
    }
}