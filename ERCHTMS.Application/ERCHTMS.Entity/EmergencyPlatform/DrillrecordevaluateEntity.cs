using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EmergencyPlatform
{
    /// <summary>
    /// 描 述：演练记录评价表
    /// </summary>
    [Table("MAE_DRILLRECORDEVALUATE")]
    public class DrillrecordevaluateEntity : BaseEntity
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
        [Column("SINGIMG")]
        public string SingImg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 评价人
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEPERSON")]
        public string EvaluatePerson { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 评价部门
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEDEPT")]
        public string EvaluateDept { get; set; }
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
        /// 评价意见
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEOPINION")]
        public string EvaluateOpinion { get; set; }
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
        [Column("EVALUATETIME")]
        public DateTime? EvaluateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATESCORE")]
        public string EvaluateScore { get; set; }
        /// <summary>
        /// 评价部门Id
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEDEPTID")]
        public string EvaluateDeptId { get; set; }
        /// <summary>
        /// 流程节点Id
        /// </summary>
        /// <returns></returns>
        [Column("NODEID")]
        public string NodeId { get; set; }
        /// <summary>
        /// 流程节点名称
        /// </summary>
        /// <returns></returns>
        [Column("NODENAME")]
        public string NodeName { get; set; }
        /// <summary>
        /// 评价人Id
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEPERSONID")]
        public string EvaluatePersonId { get; set; }
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