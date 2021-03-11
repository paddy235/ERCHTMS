using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SafetyLawManage
{
    /// <summary>
    /// 描 述：事故案例库
    /// </summary>
    [Table("BIS_ACCIDENTCASELAW")]
    public class AccidentCaseLawEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 文件id
        /// </summary>
        /// <returns></returns>
        [Column("FILESID")]
        public string FilesId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 事故范围
        /// </summary>
        /// <returns></returns>
        [Column("ACCRANGE")]
        public string AccRange { get; set; }
        /// <summary>
        /// 事故时间
        /// </summary>
        /// <returns></returns>
        [Column("ACCTIME")]
        public DateTime? AccTime { get; set; }
        /// <summary>
        /// 文件和资料名称
        /// </summary>
        /// <returns></returns>
        [Column("FILENAME")]
        public string FileName { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }

        /// <summary>
        /// 事故单位
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTCOMPANY")]
        public string AccidentCompany { get; set; }
        /// <summary>
        /// 涉事设备
        /// </summary>
        /// <returns></returns>
        [Column("RELATEDEQUIPMENT")]
        public string RelatedEquipment { get; set; }
        /// <summary>
        /// 涉事单位
        /// </summary>
        /// <returns></returns>
        [Column("RELATEDCOMPANY")]
        public string RelatedCompany { get; set; }
        /// <summary>
        /// 涉事工种
        /// </summary>
        /// <returns></returns>
        [Column("RELATEDJOB")]
        public string RelatedJob { get; set; }
        /// <summary>
        /// 事故等级
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTGRADE")]
        public string AccidentGrade { get; set; }
        /// <summary>
        /// 所属辖区
        /// </summary>
        /// <returns></returns>
        [Column("PROVINCE")]
        public string Province { get; set; }
        /// <summary>
        /// 死亡人数(人)
        /// </summary>
        /// <returns></returns>
        [Column("INTDEATHS")]
        public string intDeaths { get; set; }
        /// <summary>
        /// 事故类别
        /// </summary>
        /// <returns></returns>
        [Column("ACCTYPE")]
        public string AccType { get; set; }
        /// <summary>
        /// 事故类别CODE
        /// </summary>
        /// <returns></returns>
        [Column("ACCTYPECODE")]
        public string AccTypeCode { get; set; }
        /// <summary>
        /// 案例来源,0本地,1同步案例库
        /// </summary>
        /// <returns></returns>
        [Column("CASESOURCE")]
        public string CaseSource { get; set; }

        /// <summary>
        /// 案例来源,0本地,1同步案例库
        /// </summary>
        /// <returns></returns>
        [Column("CASEFID")]
        public string CaseFid { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
            this.CreateDate = DateTime.Now;
            if (OperatorProvider.Provider.Current() != null)
            {
                this.CreateUserId = OperatorProvider.Provider.Current().UserId;
                this.CreateUserName = OperatorProvider.Provider.Current().UserName;
                this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            }
            else
            {
                this.CreateUserId = "System";
                this.CreateUserName = "超级管理员";
                this.CreateUserDeptCode = "00";
            }
            this.CreateUserOrgCode = string.IsNullOrEmpty(CreateUserOrgCode) ? OperatorProvider.Provider.Current().OrganizeCode : CreateUserOrgCode;
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