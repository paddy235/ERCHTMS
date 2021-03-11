using ERCHTMS.Code;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERCHTMS.Entity.SystemManage
{
    /// <summary>
    /// 描 述：密码规则设置
    /// </summary>
    public class PasswordSetEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 区域主键
        /// </summary>	
          [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 单位编码
        /// </summary>
        [Column("ORGCODE")]
        public string OrgCode { get; set; }
        /// <summary>
        ///单位名称
        /// </summary>	
         [Column("ORGNAME")]
        public string OrgName { get; set; }
        /// <summary>
        /// 密码规则
        /// </summary>		
         [Column("RULE")]
        public string Rule { get; set; }
        /// <summary>
        /// 状态（0：不启用，1：启用）
        /// </summary>	
         [Column("STATUS")]
        public int? Status { get; set; }
        /// <summary>
        /// 密码长度
        /// </summary>	
         [Column("LEN")]
        public int? Len { get; set; }
         /// <summary>
         /// 设置密码连续错误多少次则禁用账号,0代表不作限制
         /// </summary>	
         [Column("NUM")]
         public int? Num { get; set; }
        /// <summary>
        /// 备用
        /// </summary>		
        [Column("REMARK")]
        public string Remark { get; set; }
        #endregion
        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = Guid.NewGuid().ToString();
            
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            Id = keyValue;
        }
        #endregion
    }
}
