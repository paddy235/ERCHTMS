using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.LaborProtectionManage
{
    /// <summary>
    /// 描 述：劳动防护用品表
    /// </summary>
    [Table("BIS_LABORINFO")]
    public class LaborinfoEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 用品型号
        /// </summary>
        /// <returns></returns>
        [Column("MODEL")]
        public string Model { get; set; }
        /// <summary>
        /// 用品名称
        /// </summary>
        /// <returns></returns>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// 操作日期
        /// </summary>
        /// <returns></returns>
        [Column("LABOROPERATIONTIME")]
        public DateTime? LaboroPerationTime { get; set; }
        /// <summary>
        /// 使用岗位ID
        /// </summary>
        /// <returns></returns>
        [Column("POSTID")]
        public string PostId { get; set; }
        /// <summary>
        /// 用品单位
        /// </summary>
        /// <returns></returns>
        [Column("UNIT")]
        public string Unit { get; set; }
        /// <summary>
        /// 发放数量
        /// </summary>
        /// <returns></returns>
        [Column("ISSUENUM")]
        public int? IssueNum { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 使用部门ID
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }
        /// <summary>
        /// 使用部门Code
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        /// <returns></returns>
        [Column("LABOROPERATIONUSERNAME")]
        public string LaboroPerationUserName { get; set; }
        /// <summary>
        /// 使用部门名称
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// 最近发放时间
        /// </summary>
        /// <returns></returns>
        [Column("RECENTTIME")]
        public DateTime? RecentTime { get; set; }
        /// <summary>
        /// 使用单位ID
        /// </summary>
        /// <returns></returns>
        [Column("ORGID")]
        public string OrgId { get; set; }
        /// <summary>
        /// 使用单位Code
        /// </summary>
        /// <returns></returns>
        [Column("ORGCODE")]
        public string OrgCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 使用单位名称
        /// </summary>
        /// <returns></returns>
        [Column("ORGNAME")]
        public string OrgName { get; set; }
        /// <summary>
        /// 用品编号
        /// </summary>
        /// <returns></returns>
        [Column("NO")]
        public string No { get; set; }
        /// <summary>
        /// 用品类型
        /// </summary>
        /// <returns></returns>
        [Column("TYPE")]
        public string Type { get; set; }
        /// <summary>
        /// 使用期限单位 年、月、日
        /// </summary>
        /// <returns></returns>
        [Column("TIMETYPE")]
        public string TimeType { get; set; }
        /// <summary>
        /// 使用说明
        /// </summary>
        /// <returns></returns>
        [Column("NOTE")]
        public string Note { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 下次发放时间
        /// </summary>
        /// <returns></returns>
        [Column("NEXTTIME")]
        public DateTime? NextTime { get; set; }
        /// <summary>
        /// 使用期限时长
        /// </summary>
        /// <returns></returns>
        [Column("TIMENUM")]
        public int? TimeNum { get; set; }
        /// <summary>
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 关联物资表ID
        /// </summary>
        /// <returns></returns>
        [Column("LID")]
        public string LId { get; set; }
        /// <summary>
        /// 应配数量
        /// </summary>
        /// <returns></returns>
        [Column("SHOULDNUM")]
        public int? ShouldNum { get; set; }
        /// <summary>
        /// 使用岗位名称
        /// </summary>
        /// <returns></returns>
        [Column("POSTNAME")]
        public string PostName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.RecentTime = DateTime.Now;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
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
                    }
        #endregion
    }

}