using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.SystemManage
{
    /// <summary>
    /// 描 述：短消息关注设置
    /// </summary>
    [Table("BASE_MESSAGEUSERSET")]
    public class MessageUserSetEntity:BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>		
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>		
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建人部门Code
        /// </summary>		
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建人机构Code
        /// </summary>		
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
         [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        [Column("SETCATEGORY")]
        public string SetCategory { get; set; }
        [Column("REMARK")]
        public string Remark { get; set; }

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
            this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
            this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
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
