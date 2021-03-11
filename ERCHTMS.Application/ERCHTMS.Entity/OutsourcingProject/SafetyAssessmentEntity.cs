using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全考核主表
    /// </summary>
    [Table("EPG_SAFETYASSESSMENT")]
    public class SafetyAssessmentEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 流程名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWNAME")]
        public string FLOWNAME { get; set; }
        /// <summary>
        /// 流程角色编码/ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLE")]
        public string FLOWROLE { get; set; }
        /// <summary>
        /// 流程角色名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLENAME")]
        public string FLOWROLENAME { get; set; }
        /// <summary>
        /// 流程部门编码/ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPT")]
        public string FLOWDEPT { get; set; }
        /// <summary>
        /// 流程部门名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPTNAME")]
        public string FLOWDEPTNAME { get; set; }
        /// <summary>
        /// 考核依据
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEBASIS")]
        public string EXAMINEBASIS { get; set; }
        /// <summary>
        /// 考核事由
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEREASON")]
        public string EXAMINEREASON { get; set; }
        /// <summary>
        /// 考核时间
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINETIME")]
        public DateTime? EXAMINETIME { get; set; }
        /// <summary>
        /// 考核类别
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINETYPE")]
        public string EXAMINETYPE { get; set; }
        /// <summary>
        /// 考核人ID
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEPERSONID")]
        public string EXAMINEPERSONID { get; set; }
        /// <summary>
        /// 考核人
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEPERSON")]
        public string EXAMINEPERSON { get; set; }
        /// <summary>
        /// 提出考核部门ID
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEDEPTID")]
        public string EXAMINEDEPTID { get; set; }
        /// <summary>
        /// 提出考核部门
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEDEPT")]
        public string EXAMINEDEPT { get; set; }
        /// <summary>
        /// 考核编号
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINECODE")]
        public string EXAMINECODE { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        [Column("NUMCODE")]
        public string NUMCODE { get; set; }
        /// <summary>
        /// 流程Id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FLOWID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// 是否保存成功
        /// </summary>
        /// <returns></returns>
        [Column("ISSAVED")]
        public int? IsSaved { get; set; }

        /// <summary>
        /// 流程完成情况
        /// </summary>
        /// <returns></returns>
        [Column("ISOVER")]
        public int? IsOver { get; set; }

        /// <summary>
        /// 考核性质 0：处罚 1：奖励
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATETYPE")]
        public string EvaluateType { get; set; }

        /// <summary>
        /// 考核类别名称
        /// </summary>
        [Column("EXAMINETYPENAME")]
        public string EXAMINETYPENAME { get; set; }


        /// <summary>
        /// 创建人部门ID
        /// </summary>
        [Column("CREATEUSERDEPTID")]
        public string CreateUserDeptId { get; set; }

        /// <summary>
        /// 考核依据关联考核标准ID
        /// </summary>
        [Column("EXAMINEBASISID")]
        public string EXAMINEBASISID { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ?Guid.NewGuid().ToString(): ID;
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
           this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
           this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
            this.CreateUserDeptId = OperatorProvider.Provider.Current().DeptId;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}