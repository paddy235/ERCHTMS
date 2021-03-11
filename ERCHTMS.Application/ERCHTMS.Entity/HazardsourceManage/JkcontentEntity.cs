using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HazardsourceManage
{
    /// <summary>
    /// 描 述：监控内容
    /// </summary>
    [Table("HSD_JKCONTENT")]
    public class JkcontentEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 关联危险源Id
        /// </summary>
        /// <returns></returns>
        [Column("HDID")]
        public string HdId { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        /// <returns></returns>
        [Column("WD")]
        public string Wd { get; set; }
        /// <summary>
        /// 压力
        /// </summary>
        /// <returns></returns>
        [Column("YL")]
        public string Yl { get; set; }
        /// <summary>
        /// 液位
        /// </summary>
        /// <returns></returns>
        [Column("YW")]
        public string Yw { get; set; }
        /// <summary>
        /// 流量
        /// </summary>
        /// <returns></returns>
        [Column("LL")]
        public string Ll { get; set; }
        /// <summary>
        /// 空气中化学物质浓度
        /// </summary>
        /// <returns></returns>
        [Column("KQZHXWZND")]
        public string Kqzhxwznd { get; set; }
        /// <summary>
        /// 组分
        /// </summary>
        /// <returns></returns>
        [Column("ZF")]
        public string Zf { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        /// <returns></returns>
        [Column("QT")]
        public string Qt { get; set; }
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
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
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
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
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
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}