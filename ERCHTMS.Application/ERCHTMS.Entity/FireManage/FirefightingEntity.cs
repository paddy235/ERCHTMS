using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.FireManage
{
    /// <summary>
    /// 描 述：消防设施
    /// </summary>
    [Table("HRS_FIREFIGHTING")]
    public class FirefightingEntity : BaseEntity
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
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 水带数
        /// </summary>
        /// <returns></returns>
        [Column("WATERBELT")]
        public int? WaterBelt { get; set; }
        /// <summary>
        /// 检查时间
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEDATE")]
        public DateTime? ExamineDate { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSER")]
        public string DutyUser { get; set; }
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
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 检测结论 0合格 1不合格
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONVERDICT")]
        public int? DetectionVerdict { get; set; }
        /// <summary>
        /// 灭火器类型
        /// </summary>
        /// <returns></returns>
        [Column("EXTINGUISHERTYPE")]
        public string ExtinguisherType { get; set; }
        /// <summary>
        /// 灭火器类型编号
        /// </summary>
        /// <returns></returns>
        [Column("EXTINGUISHERTYPENO")]
        public string ExtinguisherTypeNo { get; set; }
        /// <summary>
        /// 维护单位
        /// </summary>
        /// <returns></returns>
        [Column("SAFEGUARDUNIT")]
        public string SafeguardUnit { get; set; }
        /// <summary>
        /// 下次检测时间
        /// </summary>
        /// <returns></returns>
        [Column("NEXTDETECTIONDATE")]
        public DateTime? NextDetectionDate { get; set; }
        /// <summary>
        /// 检测时间
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONDATE")]
        public DateTime? DetectionDate { get; set; }
        /// <summary>
        /// 责任部门
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPT")]
        public string DutyDept { get; set; }
        /// <summary>
        /// 检查周期
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEPERIOD")]
        public int? ExaminePeriod { get; set; }
        /// <summary>
        /// 检测单位
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONUNIT")]
        public string DetectionUnit { get; set; }
        /// <summary>
        /// 配置区域
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICT")]
        public string District { get; set; }
        /// <summary>
        /// 安装单位
        /// </summary>
        /// <returns></returns>
        [Column("INSTALLUNIT")]
        public string InstallUnit { get; set; }
        /// <summary>
        /// 责任部门编号
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPTCODE")]
        public string DutyDeptCode { get; set; }
        /// <summary>
        /// 保护对象
        /// </summary>
        /// <returns></returns>
        [Column("PROTECTOBJECT")]
        public string ProtectObject { get; set; }
        /// <summary>
        /// 枪头数
        /// </summary>
        /// <returns></returns>
        [Column("SPEARHEAD")]
        public int? Spearhead { get; set; }
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
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 配置区域ID
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTID")]
        public string DistrictId { get; set; }
        /// <summary>
        /// 检测周期
        /// </summary>
        /// <returns></returns>
        [Column("DETECTIONPERIOD")]
        public int? DetectionPeriod { get; set; }
        /// <summary>
        /// 配置位置
        /// </summary>
        /// <returns></returns>
        [Column("LOCATION")]
        public string Location { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 配置区域Code
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTCODE")]
        public string DistrictCode { get; set; }
        /// <summary>
        /// 设计单位
        /// </summary>
        /// <returns></returns>
        [Column("DESIGNUNIT")]
        public string DesignUnit { get; set; }
        /// <summary>
        /// 竣工时间
        /// </summary>
        /// <returns></returns>
        [Column("DONEDATE")]
        public DateTime? DoneDate { get; set; }
        /// <summary>
        /// 责任人电话
        /// </summary>
        /// <returns></returns>
        [Column("DUTYTEL")]
        public string DutyTel { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNAME")]
        public string EquipmentName { get; set; }
        /// <summary>
        /// 设备名称编号
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTNAMENO")]
        public string EquipmentNameNo { get; set; }
        /// <summary>
        /// 主要参数
        /// </summary>
        /// <returns></returns>
        [Column("MAINPARAMETER")]
        public string MainParameter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 下次检查时间
        /// </summary>
        /// <returns></returns>
        [Column("NEXTEXAMINEDATE")]
        public DateTime? NextExamineDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AutoId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 上次充装时间
        /// </summary>
        /// <returns></returns>
        [Column("LASTFILLDATE")]
        public DateTime? LastFillDate { get; set; }
        /// <summary>
        /// 下次充装时间
        /// </summary>
        /// <returns></returns>
        [Column("NEXTFILLDATE")]
        public DateTime? NextFillDate { get; set; }
        /// <summary>
        /// 设备编号
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTCODE")]
        public string EquipmentCode { get; set; }
        /// <summary>
        /// 责任人ID
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        /// <returns></returns>
        [Column("SPECIFICATIONS")]
        public string Specifications { get; set; }
        /// <summary>
        /// 出厂时间
        /// </summary>
        /// <returns></returns>
        [Column("LEAVEDATE")]
        public DateTime? LeaveDate { get; set; }
        /// <summary>
        /// nfc
        /// </summary>
        /// <returns></returns>
        [Column("NFC")]
        public string NFC { get; set; }
        /// <summary>
        /// 充装/更换周期（天）
        /// </summary>
        /// <returns></returns>
        [Column("FILLPERIOD")]
        public int? FillPeriod { get; set; }
        /// <summary>
        /// 检查人
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEUSER")]
        public string ExamineUser { get; set; }
        /// <summary>
        /// 检查人ID
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEUSERID")]
        public string ExamineUserId { get; set; }
        /// <summary>
        /// 检查部门
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEDEPT")]
        public string ExamineDept { get; set; }
        /// <summary>
        /// 检查部门code
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEDEPTCODE")]
        public string ExamineDeptCode { get; set; }
        /// <summary>
        /// 定期检测时间
        /// </summary>
        /// <returns></returns>
        [Column("TERMINALDETECTIONDATE")]
        public DateTime? TerminalDetectionDate { get; set; }
        /// <summary>
        /// 定期检测单位
        /// </summary>
        /// <returns></returns>
        [Column("TERMINALDETECTIONUNIT")]
        public string TerminalDetectionUnit { get; set; }
        /// <summary>
        /// 定期检测结论
        /// </summary>
        /// <returns></returns>
        [Column("TERMINALDETECTIONVERDICT")]
        public int? TerminalDetectionVerdict { get; set; }
        /// <summary>
        /// 定期检测周期(天)
        /// </summary>
        /// <returns></returns>
        [Column("TERMINALDETECTIONPERIOD")]
        public int? TerminalDetectionPeriod { get; set; }
        /// <summary>
        /// 下次定期检测时间
        /// </summary>
        /// <returns></returns>
        [Column("NEXTTERMINALDETECTIONDATE")]
        public DateTime? NextTerminalDetectionDate { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        /// <returns></returns>
        [Column("AMOUNT")]
        public int? Amount { get; set; }
        /// <summary>
        /// 数量单位
        /// </summary>
        /// <returns></returns>
        [Column("AMOUNTUNIT")]
        public string AmountUnit { get; set; }
        /// <summary>
        /// 数量单位
        /// </summary>
        /// <returns></returns>
        [Column("SCRAPDATE")]
        public DateTime? ScrapDate { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
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