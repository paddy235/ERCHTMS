using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SafetyLawManage
{
    /// <summary>
    /// 描 述：安全生产法律法规
    /// </summary>
    [Table("BIS_SAFETYLAW")]
    public class SafetyLawEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 法律法规类型编号
        /// </summary>
        /// <returns></returns>
        [Column("LAWTYPECODE")]
        public string LawTypeCode { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 文件编号
        /// </summary>
        /// <returns></returns>
        [Column("FILECODE")]
        public string FileCode { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        /// <returns></returns>
        [Column("LAWAREA")]
        public string LawArea { get; set; }
        /// <summary>
        /// 施行日期
        /// </summary>
        /// <returns></returns>
        [Column("CARRYDATE")]
        public DateTime? CarryDate { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 文件和资料名称
        /// </summary>
        /// <returns></returns>
        [Column("FILENAME")]
        public string FileName { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 发布机构
        /// </summary>
        /// <returns></returns>
        [Column("ISSUEDEPT")]
        public string IssueDept { get; set; }
        /// <summary>
        /// 发布机构CODE
        /// </summary>
        /// <returns></returns>
        [Column("ISSUEDEPTCODE")]
        public string IssueDeptCode { get; set; }
        /// <summary>
        /// 有效版本号
        /// </summary>
        /// <returns></returns>
        [Column("VALIDVERSIONS")]
        public string ValidVersions { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        /// <returns></returns>
        [Column("RESERVE")]
        public string Reserve { get; set; }

        /// <summary>
        /// 文件id
        /// </summary>
        /// <returns></returns>
        [Column("FILESID")]
        public string FilesId { get; set; }

        /// <summary>
        /// 发布城市
        /// </summary>
        /// <returns></returns>
        [Column("PROVINCE")]
        public string Province { get; set; }
        /// <summary>
        /// 效力级别（法规类型）
        /// </summary>
        /// <returns></returns>
        [Column("LAWTYPE")]
        public string LawType { get; set; }
        /// <summary>
        /// 时效性
        /// </summary>
        /// <returns></returns>
        [Column("EFFETSTATE")]
        public string EffetState { get; set; }
        /// <summary>
        /// 内容大纲
        /// </summary>
        /// <returns></returns>
        [Column("MAINCONTENT")]
        public string MainContent { get; set; }
        /// <summary>
        /// 正文快照
        /// </summary>
        /// <returns></returns>
        [Column("COPYCONTENT")]
        public string CopyContent { get; set; }
        /// <summary>
        /// 数据来源(0本地,1法规库)
        /// </summary>
        /// <returns></returns>
        [Column("LAWSOURCE")]
        public string LawSource { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        /// <returns></returns>
        [Column("RELEASEDATE")]
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        /// <returns></returns>
        [Column("UPDATEDATE")]
        public DateTime? UpdateDate { get; set; }
        /// <summary>
        /// 渠道方式
        /// </summary>
        /// <returns></returns>
        [Column("CHANNELTYPE")]
        public string ChannelType { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            if (OperatorProvider.Provider.Current() != null)
            {
                this.CreateUserId = OperatorProvider.Provider.Current().UserId;
                this.CreateUserName = OperatorProvider.Provider.Current().UserName;
                this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            }
            else {
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