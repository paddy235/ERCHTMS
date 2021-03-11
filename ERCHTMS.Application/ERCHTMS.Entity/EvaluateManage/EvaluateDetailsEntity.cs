using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EvaluateManage
{
    /// <summary>
    /// 描 述：合规性评价明细
    /// </summary>
    [Table("HRS_EVALUATEDETAILS")]
    public class EvaluateDetailsEntity : BaseEntity
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
        public int? AUTOID { get; set; }
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
        /// 类别
        /// </summary>
        /// <returns></returns>
        [Column("CATEGORY")]
        public string Category { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        /// <returns></returns>
        [Column("CATEGORYNAME")]
        public string CategoryName { get; set; }
        /// <summary>
        /// 级别(小类)
        /// </summary>
        /// <returns></returns>
        [Column("RANK")]
        public string Rank { get; set; }
        /// <summary>
        /// 级别名称(小类)
        /// </summary>
        /// <returns></returns>
        [Column("RANKNAME")]
        public string RankName { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPT")]
        public string DutyDept { get; set; }
        /// <summary>
        /// 部门编码
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPTCODE")]
        public string DutyDeptCode { get; set; }
        /// <summary>
        /// 适用条款
        /// </summary>
        /// <returns></returns>
        [Column("CLAUSE")]
        public string Clause { get; set; }
        /// <summary>
        /// 现状符合性描述
        /// </summary>
        /// <returns></returns>
        [Column("DESCRIBE")]
        public string Describe { get; set; }
        /// <summary>
        /// 整改意见
        /// </summary>
        /// <returns></returns>
        [Column("OPINION")]
        public string Opinion { get; set; }
        /// <summary>
        /// 整改完成时间
        /// </summary>
        /// <returns></returns>
        [Column("FINISHTIME")]
        public DateTime? FinishTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMAKE")]
        public string Remake { get; set; }
        /// <summary>
        /// 主表ID
        /// </summary>
        /// <returns></returns>
        [Column("MAINID")]
        public string MainId { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        /// <returns></returns>
        [Column("FILENAME")]
        public string FileName { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        /// <returns></returns>
        [Column("YEAR")]
        public int? Year { get; set; }
        /// <summary>
        /// 符合性
        /// </summary>
        /// <returns></returns>
        [Column("ISCONFORM")]
        public int? IsConform { get; set; }
        /// <summary>
        /// 整改措施
        /// </summary>
        /// <returns></returns>
        [Column("MEASURE")]
        public string Measure { get; set; }
        /// <summary>
        /// 整改实际完成时间
        /// </summary>
        /// <returns></returns>
        [Column("PRACTICALFINISHTIME")]
        public DateTime? PracticalFinishTime { get; set; }
        /// <summary>
        /// 评价计划ID
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEPLANID")]
        public string EvaluatePlanId { get; set; }
        /// <summary>
        /// 实施日期
        /// </summary>
        /// <returns></returns>
        [Column("PUTDATE")]
        public DateTime? PutDate { get; set; }
        /// <summary>
        /// 评价人
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEPERSON")]
        public string EvaluatePerson { get; set; }
        /// <summary>
        /// 评价人ID
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEPERSONID")]
        public string EvaluatePersonId { get; set; }
        /// <summary>
        /// 纳入企业标准的名称
        /// </summary>
        /// <returns></returns>
        [Column("NORMNAME")]
        public string NormName { get; set; }
        /// <summary>
        /// 适用范围
        /// </summary>
        /// <returns></returns>
        [Column("APPLYSCOPE")]
        public string ApplyScope { get; set; }

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