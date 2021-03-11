using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;

namespace ERCHTMS.Entity.PowerPlantInside
{
    /// <summary>
    /// 描 述：事故事件处理整改
    /// </summary>
    [Table("BIS_POWERPLANTREFORM")]
    public class PowerplantreformEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 整改人ID
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONPERSONID")]
        public string RectificationPersonId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 整改措施
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONMEASURES")]
        public string RectificationMeasures { get; set; }
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
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 整改责任部门ID
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONDUTYDEPTID")]
        public string RectificationDutyDeptId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 整改人签名
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONPERSONSIGNIMG")]
        public string RectificationPersonSignImg { get; set; }
        /// <summary>
        /// 整改期限
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONTIME")]
        public DateTime? RectificationTime { get; set; }
        /// <summary>
        /// 整改情况描述
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONSITUATION")]
        public string RectificationSituation { get; set; }
        /// <summary>
        /// 整改人
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONPERSON")]
        public string RectificationPerson { get; set; }
        /// <summary>
        /// 整改责任部门
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONDUTYDEPT")]
        public string RectificationDutyDept { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 整改完成时间
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONENDTIME")]
        public DateTime? RectificationEndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }

        /// <summary>
        /// 关联事故事件处理记录ID
        /// </summary>
        [Column("POWERPLANTHANDLEID")]
        public string PowerPlantHandleId { get; set; }

        /// <summary>
        /// 关联事故事件处理记录详细ID
        /// </summary>
        [Column("POWERPLANTHANDLEDETAILID")]
        public string PowerPlantHandleDetailId { get; set; }

        /// <summary>
        /// 是否失效 0:有效 1:失效
        /// </summary>
        [Column("DISABLE")]
        public int? Disable { get; set; }

        [NotMapped]
        public IList<Photo> filelist { get; set; } //附件

        /// <summary>
        /// 原因及暴露问题
        /// </summary>
        [Column("REASONANDPROBLEM")]
        public string ReasonAndProblem { get; set; }

        /// <summary>
        /// 签收部门
        /// </summary>
        [Column("SIGNDEPTNAME")]
        public string SignDeptName { get; set; }

        /// <summary>
        /// 签收部门
        /// </summary>
        [Column("SIGNDEPTID")]
        public string SignDeptId { get; set; }

        /// <summary>
        /// 签收人
        /// </summary>
        [Column("SIGNPERSONNAME")]
        public string SignPersonName { get; set; }

        /// <summary>
        /// 签收人
        /// </summary>
        [Column("SIGNPERSONID")]
        public string SignPersonId { get; set; }

        /// <summary>
        /// 是否指定责任人
        /// </summary>
        [Column("ISASSIGNPERSON")]
        public string IsAssignPerson { get; set; }

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
    
}