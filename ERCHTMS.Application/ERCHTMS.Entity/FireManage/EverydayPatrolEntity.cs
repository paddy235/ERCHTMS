using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.FireManage
{
    /// <summary>
    /// 描 述：日常巡查
    /// </summary>
    [Table("HRS_EVERYDAYPATROL")]
    public class EverydayPatrolEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 问题数量
        /// </summary>
        /// <returns></returns>
        [Column("PROBLEMNUM")]
        public int? ProblemNum { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("DISPOSE9")]
        //public string Dispose9 { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("DISPOSE8")]
        //public string Dispose8 { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("DISPOSE7")]
        //public string Dispose7 { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("DISPOSE6")]
        //public string Dispose6 { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("DISPOSE5")]
        //public string Dispose5 { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("DISPOSE4")]
        //public string Dispose4 { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("DISPOSE3")]
        //public string Dispose3 { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("DISPOSE2")]
        //public string Dispose2 { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("DISPOSE1")]
        //public string Dispose1 { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("PROBLEM9")]
        //public string Problem9 { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("PROBLEM8")]
        //public string Problem8 { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("PROBLEM7")]
        //public string Problem7 { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("PROBLEM6")]
        //public string Problem6 { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("PROBLEM5")]
        //public string Problem5 { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("PROBLEM4")]
        //public string Problem4 { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("PROBLEM3")]
        //public string Problem3 { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("PROBLEM2")]
        //public string Problem2 { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[Column("PROBLEM1")]
        //public string Problem1 { get; set; }
        ///// <summary>
        ///// 第九项结果
        ///// </summary>
        ///// <returns></returns>
        //[Column("RESULT9")]
        //public int? Result9 { get; set; }
        ///// <summary>
        ///// 第八项结果
        ///// </summary>
        ///// <returns></returns>
        //[Column("RESULT8")]
        //public int? Result8 { get; set; }
        ///// <summary>
        ///// 第七项结果
        ///// </summary>
        ///// <returns></returns>
        //[Column("RESULT7")]
        //public int? Result7 { get; set; }
        ///// <summary>
        ///// 第六项结果
        ///// </summary>
        ///// <returns></returns>
        //[Column("RESULT6")]
        //public int? Result6 { get; set; }
        ///// <summary>
        ///// 第五项结果
        ///// </summary>
        ///// <returns></returns>
        //[Column("RESULT5")]
        //public int? Result5 { get; set; }
        ///// <summary>
        ///// 第四项结果
        ///// </summary>
        ///// <returns></returns>
        //[Column("RESULT4")]
        //public int? Result4 { get; set; }
        ///// <summary>
        ///// 第三项结果
        ///// </summary>
        ///// <returns></returns>
        //[Column("RESULT3")]
        //public int? Result3 { get; set; }
        ///// <summary>
        ///// 第二项结果
        ///// </summary>
        ///// <returns></returns>
        //[Column("RESULT2")]
        //public int? Result2 { get; set; }
        ///// <summary>
        ///// 第一项结果
        ///// </summary>
        ///// <returns></returns>
        //[Column("RESULT1")]
        //public int? Result1 { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        /// <returns></returns>
        //[Column("PATROLCONTENT")]
        //public string PatrolContent { get; set; }
        /// <summary>
        /// 巡查位置
        /// </summary>
        /// <returns></returns>
        [Column("PATROLPLACE")]
        public string PatrolPlace { get; set; }
        /// <summary>
        /// 巡查人ID
        /// </summary>
        /// <returns></returns>
        [Column("PATROLPERSONID")]
        public string PatrolPersonId { get; set; }
        /// <summary>
        /// 巡查人
        /// </summary>
        /// <returns></returns>
        [Column("PATROLPERSON")]
        public string PatrolPerson { get; set; }
        /// <summary>
        /// 巡查时间
        /// </summary>
        /// <returns></returns>
        [Column("PATROLDATE")]
        public DateTime? PatrolDate { get; set; }
        /// <summary>
        /// 巡查部门
        /// </summary>
        /// <returns></returns>
        [Column("PATROLDEPT")]
        public string PatrolDept { get; set; }
        /// <summary>
        /// 巡查部门Code
        /// </summary>
        /// <returns></returns>
        [Column("PATROLDEPTCODE")]
        public string PatrolDeptCode { get; set; }
        /// <summary>
        /// 巡查类型
        /// </summary>
        /// <returns></returns>
        [Column("PATROLTYPE")]
        public string PatrolType { get; set; }
        /// <summary>
        /// 巡查类型编码
        /// </summary>
        /// <returns></returns>
        [Column("PATROLTYPECODE")]
        public string PatrolTypeCode { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSER")]
        public string DutyUser { get; set; }
        /// <summary>
        /// 负责人ID
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
        /// 责任部门编号
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPTCODE")]
        public string DutyDeptCode { get; set; }
        /// <summary>
        /// 责任部门ID
        /// </summary>
        /// <returns></returns>
        [Column("BYDEPTID")]
        public string ByDeptId { get; set; }
        /// <summary>
        /// 被检查单位部门
        /// </summary>
        /// <returns></returns>
        [Column("BYDEPT")]
        public string ByDept { get; set; }
        /// <summary>
        /// 被检查单位部门编号
        /// </summary>
        /// <returns></returns>
        [Column("BYDEPTCODE")]
        public string ByDeptCode { get; set; }
        /// <summary>
        /// 被检查单位负责人
        /// </summary>
        /// <returns></returns>
        [Column("BYUSER")]
        public string ByUser { get; set; }
        /// <summary>
        /// 被检查单位负责人ID
        /// </summary>
        /// <returns></returns>
        [Column("BYUSERID")]
        public string ByUserId { get; set; }
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
        /// 签名
        /// </summary>
        /// <returns></returns>
        [Column("SIGNATURE")]
        public string Signature { get; set; }
        /// <summary>
        /// 确认状态 0保存 1提交 待确认 2已确认
        /// </summary>
        /// <returns></returns>
        [Column("AFFIRMSTATE")]
        public int? AffirmState { get; set; }
        /// <summary>
        /// 流程待处理人 处理完毕为空
        /// </summary>
        /// <returns></returns>
        [Column("AFFIRMUSERID")]
        public string AffirmUserId { get; set; }
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