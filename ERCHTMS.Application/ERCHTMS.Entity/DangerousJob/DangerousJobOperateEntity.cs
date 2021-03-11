using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.DangerousJob
{
    /// <summary>
    /// 描 述：高危作业操作表
    /// </summary>
    [Table("BIS_DANGEROUSJOBOPERATE")]
    public class DangerousJobOperateEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 操作人id
        /// </summary>
        /// <returns></returns>
        [Column("OPERATEPERSONID")]
        public string OperatePersonId { get; set; }
        /// <summary>
        /// 操作人部门id
        /// </summary>
        /// <returns></returns>
        [Column("OPERATEPERSONDEPTID")]
        public string OperatePersonDeptId { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        /// <returns></returns>
        [Column("OPERATEPERSON")]
        public string OperatePerson { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 关联业务id
        /// </summary>
        /// <returns></returns>
        [Column("RECID")]
        public string RecId { get; set; }
        /// <summary>
        /// 操作人部门Code
        /// </summary>
        /// <returns></returns>
        [Column("OPERATEPERSONDEPTCODE")]
        public string OperatePersonDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 操作人部门
        /// </summary>
        /// <returns></returns>
        [Column("OPERATEPERSONDEPT")]
        public string OperatePersonDept { get; set; }
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
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 操作意见
        /// </summary>
        /// <returns></returns>
        [Column("OPERATEOPINION")]
        public string OperateOpinion { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("OPERATETIME")]
        public DateTime? OperateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 操作类型  0：验收 1：备案 2：停电 3：送电
        /// </summary>
        /// <returns></returns>
        [Column("OPERATETYPE")]
        public int? OperateType { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        [Column("SIGNIMG")]
        public string SignImg { get; set; }
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