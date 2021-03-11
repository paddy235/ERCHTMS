using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.DangerousJob
{
    /// <summary>
    /// 描 述：危险作业清单
    /// </summary>
    [Table("BIS_DANGERJOBLIST")]
    public class DangerjoblistEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 创建用户ID
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
        /// 创建用户姓名
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改用户ID
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改记录时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户名称
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
        /// 危险作业名称
        /// </summary>
        /// <returns></returns>
        [Column("DANGERJOBNAME")]
        public string DangerJobName { get; set; }
        /// <summary>
        /// 作业人数
        /// </summary>
        /// <returns></returns>
        [Column("NUMBEROFPEOPLE")]
        public int? NumberofPeople { get; set; }

        /// <summary>
        /// 作业人数Text
        /// </summary>
        /// <returns></returns>
        [Column("NUMBEROFPEOPLENAME")]
        public string NumberofPeopleName { get; set; }

        /// <summary>
        /// 作业单位编码
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODES")]
        public string DeptCodes { get; set; }

        /// <summary>
        /// 作业单位ID
        /// </summary>
        /// <returns></returns>
        [Column("DEPTIDS")]
        public string DeptIds { get; set; }
        
        /// <summary>
        /// 作业单位
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAMES")]
        public string DeptNames { get; set; }
        /// <summary>
        /// 作业频次
        /// </summary>
        /// <returns></returns>
        [Column("JOBFREQUENCY")]
        public string JobFrequency { get; set; }
        /// <summary>
        /// 存在的危险因素
        /// </summary>
        /// <returns></returns>
        [Column("DANGERFACTORS")]
        public string DangerFactors { get; set; }
        /// <summary>
        /// 可能发生的事故类别
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTCATEGORIES")]
        public string AccidentCategories { get; set; }
        /// <summary>
        /// 安全措施
        /// </summary>
        /// <returns></returns>
        [Column("SAFETYMEASURES")]
        public string SafetyMeasures { get; set; }
        /// <summary>
        /// 危险作业级别
        /// </summary>
        /// <returns></returns>
        [Column("JOBLEVEL")]
        public string JobLevel { get; set; }

        /// <summary>
        /// 危险作业级别Text
        /// </summary>
        /// <returns></returns>
        [Column("JOBLEVELNAME")]
        public string JobLevelName { get; set; }
        /// <summary>
        /// 现场监护负责人
        /// </summary>
        /// <returns></returns>
        [Column("PRINCIPALIDS")]
        public string PrincipalIds { get; set; }
        /// <summary>
        /// 现场监护负责人
        /// </summary>
        /// <returns></returns>
        [Column("PRINCIPALNAMES")]
        public string PrincipalNames { get; set; }

        /// <summary>
        /// 签名地址
        /// </summary>
        /// <returns></returns>
        [Column("SIGNATUREPATH")]
        public string SignaturePath { get; set; }

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