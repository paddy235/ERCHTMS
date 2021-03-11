using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 盲板信息
    /// </summary>
    [Table("BIS_HIGHRISKCOMMONAPPLY_MBXX")]
    public class HighRiskApplyMBXXEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        [Column("ID")]
        public string Id { get; set; }

        /// <summary>
        /// 材质
        /// </summary>
        [Column("MATERIAL")]
        public string Material { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        [Column("SPECIFICATION")]
        public string Specification { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [Column("SERIALCODE")]
        public string SerialCode { get; set; }

        /// <summary>
        /// 高风险作业id
        /// </summary>
        [Column("HIGHRISKCOMMONAPPLYID")]
        public string HighRiskCommonApplyId { get; set; }

        /// <summary>
        /// 创建用户id
        /// </summary>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 机构代码
        /// </summary>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }

        /// <summary>
        /// 部门代码
        /// </summary>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }

        /// <summary>
        /// 修改用户id
        /// </summary>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }

        /// <summary>
        /// 修改用户
        /// </summary>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = string.IsNullOrEmpty(CreateDate.ToString()) ? DateTime.Now : CreateDate;
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
