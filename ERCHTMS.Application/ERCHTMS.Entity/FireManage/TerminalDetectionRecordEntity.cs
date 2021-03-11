using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.FireManage
{
    /// <summary>
    /// 描 述：消防设施-定期检测记录
    /// </summary>
    [Table("HRS_TERMINALDETECTIONRECORD")]
    public class TerminalDetectionRecordEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AutoId { get; set; }
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
        /// 设备ID
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTID")]
        public string EquipmentId { get; set; }
        /// <summary>
        /// 检测时间
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONDATE")]
        public DateTime? DetectionDate { get; set; }
        /// <summary>
        /// 检测人
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONPERSON")]
        public string DetectionPerson { get; set; }
        /// <summary>
        /// 检测人ID
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONPERSONID")]
        public string DetectionPersonId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 检测单位
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONUNIT")]
        public string DetectionUnit { get; set; }
        /// <summary>
        /// 检测结果 0合格 1不合格
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONRESULT")]
        public int? DetectionResult { get; set; }
        /// <summary>
        /// 检测系统
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONSYSTEM")]
        public string DetectionSystem { get; set; }
        /// <summary>
        /// 附件ID
        /// </summary>
        /// <returns></returns>
        [Column("FILESID")]
        public string FilesId { get; set; }
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
            this.DetectionPerson= OperatorProvider.Provider.Current().UserName;
            this.DetectionPersonId = OperatorProvider.Provider.Current().UserId;
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