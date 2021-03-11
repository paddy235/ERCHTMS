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
    /// 角儿与主题、班组文化墙关联表
    /// </summary>
    [Table("BASE_TCRULE")]
    public class TCRuleEntity : BaseEntity
    {
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 授权信息的Id，BASE_MENUAUTHORIZE表的主键
        /// </summary>
        [Column("AUTHORIZCODEID")]
        public string AuthorizCodeId { get; set; }
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        [Column("CREATEDATE")]
        public DateTime CreateDate { get; set; }
        [Column("MODIFYDATE")]
        public DateTime ModifyDate { get; set; }
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 角色Id  逗号隔开
        /// </summary>
        [Column("RULEIDS")]
        public string RuleIds { get; set; }
        /// <summary>
        /// 角色名称  逗号隔开
        /// </summary>
        [Column("RULENAMES")]
        public string RuleNames { get; set; }
        /// <summary>
        /// 数据的值
        /// </summary>
        [Column("INFOVALUE")]
        public string InfoValue { get; set; }
        /// <summary>
        /// 数据的类型
        /// 1、主题
        /// 2、文化墙地址
        /// 3、首页地址
        /// .........
        /// </summary>
        [Column("INFOTYPE")]
        public int InfoType { get; set; }
        /// <summary>
        /// 数据的类型名称
        /// </summary>
        [Column("INFOTYPENAME")]
        public string InfoTypeName { get; set; }
        /// <summary>
        /// 所属单位
        /// </summary>
        [Column("DEPTID")]
        public string DeptId { get; set; }
        /// <summary>
        /// 所属单位
        /// </summary>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK")]
        public string Remark { get; set; }

        /// <summary>
        /// 备用字段
        /// </summary>
        [Column("BK1")]
        public string BK1 { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>
        [Column("BK2")]
        public string BK2 { get; set; }

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.Id = keyValue;
        }
        #endregion
    }
}
