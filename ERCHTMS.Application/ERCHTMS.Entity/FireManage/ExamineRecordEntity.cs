using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.FireManage
{
    /// <summary>
    /// 描 述：检查记录
    /// </summary>
    [Table("HRS_EXAMINERECORD")]
    public class ExamineRecordEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 筒体/箱体
        /// </summary>
        /// <returns></returns>
        [Column("BARRELORBOX")]
        public int? BarrelOrBox { get; set; }
        /// <summary>
        /// 检查人ID
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEPERSONID")]
        public string ExaminePersonId { get; set; }
        /// <summary>
        /// 压力/阀门
        /// </summary>
        /// <returns></returns>
        [Column("STRESSORVALVE")]
        public int? StressOrValve { get; set; }
        /// <summary>
        /// 喷管/枪头
        /// </summary>
        /// <returns></returns>
        [Column("EFFUSERORSPEARHEAD")]
        public int? EffuserOrSpearhead { get; set; }
        /// <summary>
        /// 封铅/水带
        /// </summary>
        /// <returns></returns>
        [Column("SEALORWATER")]
        public int? SealOrWater { get; set; }
        /// <summary>
        /// 卫生
        /// </summary>
        /// <returns></returns>
        [Column("SANITATION")]
        public int? Sanitation { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        /// <returns></returns>
        [Column("WEIGHT")]
        public string Weight { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
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
        /// 检查时间
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEDATE")]
        public DateTime? ExamineDate { get; set; }
        /// <summary>
        /// 检查人
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEPERSON")]
        public string ExaminePerson { get; set; }
        /// <summary>
        /// 情况描述
        /// </summary>
        /// <returns></returns>
        [Column("DESCRIBE")]
        public string Describe { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        /// <returns></returns>
        [Column("VERDICT")]
        public int? Verdict { get; set; }
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