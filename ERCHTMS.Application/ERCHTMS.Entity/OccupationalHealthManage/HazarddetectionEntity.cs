using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病危害因素监测
    /// </summary>
    [Table("BIS_HAZARDDETECTION")]
    public class HazarddetectionEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("HID")]
        public string HId { get; set; }
        /// <summary>
        /// 区域ID
        /// </summary>
        /// <returns></returns>
        [Column("AREAID")]
        public string AreaId { get; set; }
        /// <summary>
        /// 区域值
        /// </summary>
        /// <returns></returns>
        [Column("AREAVALUE")]
        public string AreaValue { get; set; }
        /// <summary>
        /// 职业病危害因素ID
        /// </summary>
        /// <returns></returns>
        [Column("RISKID")]
        public string RiskId { get; set; }
        /// <summary>
        /// 职业病危害因素名称
        /// </summary>
        /// <returns></returns>
        [Column("RISKVALUE")]
        public string RiskValue { get; set; }
        /// <summary>
        /// 采样/测量地点
        /// </summary>
        /// <returns></returns>
        [Column("LOCATION")]
        public string Location { get; set; }
        /// <summary>
        /// 开始监视时间
        /// </summary>
        /// <returns></returns>
        [Column("STARTTIME")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束监视时间
        /// </summary>
        /// <returns></returns>
        [Column("ENDTIME")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 测量指标及标准
        /// </summary>
        /// <returns></returns>
        [Column("STANDARD")]
        public string Standard { get; set; }
        /// <summary>
        /// 检测负责人ID
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONUSERID")]
        public string DetectionUserId { get; set; }
        /// <summary>
        /// 检测负责人
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONUSERNAME")]
        public string DetectionUserName { get; set; }
        /// <summary>
        /// 是否超标
        /// </summary>
        /// <returns></returns>
        [Column("ISEXCESSIVE")]
        public int? IsExcessive { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.HId = string.IsNullOrEmpty(HId) ? Guid.NewGuid().ToString() : HId;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
                       this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
           this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.HId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
                    }
        #endregion
    }
}