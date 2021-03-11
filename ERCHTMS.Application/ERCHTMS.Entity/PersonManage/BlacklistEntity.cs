using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.PersonManage
{
    /// <summary>
    /// 描 述：人员证书
    /// </summary>
    [Table("BIS_BLACKLIST")]
    public class BlacklistEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
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
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
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
        /// 加入黑名单原因
        /// </summary>
        /// <returns></returns>
        [Column("REASON")]
        public string Reason { get; set; }
        /// <summary>
        /// 加入黑名单时间
        /// </summary>
        /// <returns></returns>
        [Column("JOINTIME")]
        public DateTime? JoinTime { get; set; }
        /// <summary>
        /// 是否删除的标识
        /// </summary>
        /// <returns></returns>
        [Column("DELETEMARK")]
        public int? DeleteMark { get; set; }
        /// <summary>
        /// 是否有效标识
        /// </summary>
        /// <returns></returns>
        [Column("ENABLEMARK")]
        public int? EnableMark { get; set; }
        
        /// <summary>
        /// 关联用户Id
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            var user=OperatorProvider.Provider.Current();
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            if (user!=null)
            {
                this.CreateUserId = user.UserId;
                this.CreateUserDeptCode = user.DeptCode;
                this.CreateUserOrgCode = user.OrganizeCode;
                this.CreateUserName = user.UserName;
            }
            else
            {
                this.CreateUserId = "System";
                this.CreateUserName ="管理员";
            }
          
            this.DeleteMark = this.EnableMark = 0;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
            var user = OperatorProvider.Provider.Current();
            if (user != null)
            {
                this.ModifyUserId = user.UserId;
            }
          
        }
        #endregion
    }
}
