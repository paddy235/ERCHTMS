using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EngineeringManage
{
    /// <summary>
    /// 描 述：危大工程管理
    /// </summary>
    [Table("BIS_PERILENGINEERING")]
    public class PerilEngineeringEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 创建用户id
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
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 工程名称
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGNAME")]
        public string EngineeringName { get; set; }
        /// <summary>
        /// 工程类别
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGTYPE")]
        public string EngineeringType { get; set; }
        /// <summary>
        /// 工程风险点
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGSPOT")]
        public string EngineeringSpot { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        /// <returns></returns>
        [Column("ESTARTTIME")]
        public DateTime? EStartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        /// <returns></returns>
        [Column("EFINISHTIME")]
        public DateTime? EFinishTime { get; set; }
        /// <summary>
        /// 单位类别
        /// </summary>
        /// <returns></returns>
        [Column("UNITTYPE")]
        public string UnitType { get; set; }
        /// <summary>
        /// 所属单位id
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTID")]
        public string BelongDeptId { get; set; }
        /// <summary>
        /// 所属单位名称
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTNAME")]
        public string BelongDeptName { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEPERSON")]
        public string ExaminePerson { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINETIME")]
        public DateTime? ExamineTime { get; set; }
        /// <summary>
        /// 施工方案文件id
        /// </summary>
        /// <returns></returns>
        [Column("CONSTRUCTFILES")]
        public string ConstructFiles { get; set; }
        /// <summary>
        /// 交底时间
        /// </summary>
        /// <returns></returns>
        [Column("TASKTIME")]
        public DateTime? TaskTime { get; set; }
        /// <summary>
        /// 交底人
        /// </summary>
        /// <returns></returns>
        [Column("TASKPERSON")]
        public string TaskPerson { get; set; }
        /// <summary>
        /// 交底内容
        /// </summary>
        /// <returns></returns>
        [Column("TASKCONTENT")]
        public string TaskContent { get; set; }
        /// <summary>
        /// 交底附件
        /// </summary>
        /// <returns></returns>
        [Column("TASKFILES")]
        public string TaskFiles { get; set; }
        /// <summary>
        /// 进展情况
        /// </summary>
        /// <returns></returns>
        [Column("EVOLVECASE")]
        public string EvolveCase { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }

        /// <summary>
        /// 编制时间
        /// </summary>
        /// <returns></returns>
        [Column("WRITEDATE")]
        public DateTime? WriteDate { get; set; }

        /// <summary>
        /// 编制人
        /// </summary>
        /// <returns></returns>
        [Column("WRITEUSERNAME")]
        public string WriteUserName { get; set; }

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