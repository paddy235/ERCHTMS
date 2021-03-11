using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.FireManage
{
    /// <summary>
    /// 描 述：重点防火部位
    /// </summary>
    [Table("HRS_KEYPART")]
    public class KeyPartEntity : BaseEntity
    {
        #region 实体成员
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
        /// 重点防火部位名称
        /// </summary>
        /// <returns></returns>
        [Column("PARTNAME")]
        public string PartName { get; set; }
        /// <summary>
        /// 重点防火部位名称编码
        /// </summary>
        /// <returns></returns>
        [Column("PARTNO")]
        public string PartNo { get; set; }
        /// <summary>
        /// 检查人
        /// </summary>
        /// <returns></returns>
        [Column("PLACE")]
        public string Place { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICT")]
        public string District { get; set; }
        /// <summary>
        /// 区域ID
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTID")]
        public string DistrictId { get; set; }
        /// <summary>
        /// 区域Code
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTCODE")]
        public string DistrictCode { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSER")]
        public string DutyUser { get; set; }
        /// <summary>
        /// 责任人ID
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        /// 责任部门
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPT")]
        public string DutyDept { get; set; }
        /// <summary>
        /// 责任部门ID
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPTCODE")]
        public string DutyDeptCode { get; set; }
        /// <summary>
        /// 责任人电话
        /// </summary>
        /// <returns></returns>
        [Column("DUTYTEL")]
        public string DutyTel { get; set; }
        /// <summary>
        /// 建筑结构
        /// </summary>
        /// <returns></returns>
        [Column("STRUCTURE")]
        public string Structure { get; set; }
        /// <summary>
        /// 建筑面积
        /// </summary>
        /// <returns></returns>
        [Column("ACREAGE")]
        public int? Acreage { get; set; }
        /// <summary>
        /// 主要存储物品
        /// </summary>
        /// <returns></returns>
        [Column("STOREGOODS")]
        public string StoreGoods { get; set; }
        /// <summary>
        /// 主要灭火装备
        /// </summary>
        /// <returns></returns>
        [Column("OUTFIREEQUIP")]
        public string OutfireEquip { get; set; }
        /// <summary>
        /// 重点防火部位人数
        /// </summary>
        /// <returns></returns>
        [Column("PEOPLENUM")]
        public int? PeopleNum { get; set; }
        /// <summary>
        /// 动火级别
        /// </summary>
        /// <returns></returns>
        [Column("RANK")]
        public int? Rank { get; set; }
        /// <summary>
        /// 最近巡查日期
        /// </summary>
        /// <returns></returns>
        [Column("LATELYPATROLDATE")]
        public DateTime? LatelyPatrolDate { get; set; }
        /// <summary>
        /// 巡查周期
        /// </summary>
        /// <returns></returns>
        [Column("PATROLPERIOD")]
        public int? PatrolPeriod { get; set; }
        /// <summary>
        /// 下次巡查日期
        /// </summary>
        /// <returns></returns>
        [Column("NEXTPATROLDATE")]
        public DateTime? NextPatrolDate { get; set; }
        /// <summary>
        /// 使用状态
        /// </summary>
        /// <returns></returns>
        [Column("EMPLOYSTATE")]
        public int? EmployState { get; set; }
        /// <summary>
        /// 火灾危险性分析
        /// </summary>
        /// <returns></returns>
        [Column("ANALYZE")]
        public string Analyze { get; set; }
        /// <summary>
        /// 防火措施
        /// </summary>
        /// <returns></returns>
        [Column("MEASURE")]
        public string Measure { get; set; }
        /// <summary>
        /// 方案
        /// </summary>
        /// <returns></returns>
        [Column("SCHEME")]
        public string Scheme { get; set; }
        /// <summary>
        /// 方案
        /// </summary>
        /// <returns></returns>
        [Column("SCHEMEID")]
        public string SchemeId { get; set; }
        /// <summary>
        /// 方案附件ID
        /// </summary>
        /// <returns></returns>
        [Column("SCHEMEFJID")]
        public string SchemeFjId { get; set; }
        /// <summary>
        /// 管理要求
        /// </summary>
        /// <returns></returns>
        [Column("REQUIRE")]
        public string Require { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// nfc
        /// </summary>
        /// <returns></returns>
        [Column("NFC")]
        public string NFC { get; set; }
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