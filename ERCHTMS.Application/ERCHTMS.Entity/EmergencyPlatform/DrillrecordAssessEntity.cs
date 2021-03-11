using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.EmergencyPlatform
{
    /// <summary>
    /// 描 述：演练记录评价表
    /// </summary>
    [Table("MAE_DRILLRECORDASSESS")]
    public class DrillrecordAssessEntity:BaseEntity
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
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 签名图片
        /// </summary>
        /// <returns></returns>
        [Column("ASSESSRECORDSIGNIMG")]
        public string AssessRecordSignImg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 评估人
        /// </summary>
        /// <returns></returns>
        [Column("ASSESSRECORDPERSON")]
        public string AssessRecordPerson { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 评估部门
        /// </summary>
        /// <returns></returns>
        [Column("ASSESSRECORDDEPT")]
        public string AssessRecordDept { get; set; }
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
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 评估意见
        /// </summary>
        /// <returns></returns>
        [Column("ASSESSRECORDOPINION")]
        public string AssessRecordOpinion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 演练记录Id
        /// </summary>
        /// <returns></returns>
        [Column("DRILLRECORDID")]
        public string DrillRecordId { get; set; }
        /// <summary>
        /// 评价时间
        /// </summary>
        /// <returns></returns>
        [Column("ASSESSRECORDTIME")]
        public DateTime? AssessRecordTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
      
        /// <summary>
        /// 评估部门Id
        /// </summary>
        /// <returns></returns>
        [Column("ASSESSRECORDDEPTID")]
        public string AssessRecordDeptId { get; set; }
      
        /// <summary>
        /// 评价人Id
        /// </summary>
        /// <returns></returns>
        [Column("ASSESSRECORDPERSONID")]
        public string AssessRecordPersonId { get; set; }


        /// <summary>
        /// 评估结果
        /// </summary>
        /// <returns></returns>
        [Column("ASSESSRECORDRESULT")]
        public string AssessRecordResult { get; set; }
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
