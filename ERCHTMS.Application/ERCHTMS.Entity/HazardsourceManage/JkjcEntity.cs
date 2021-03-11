using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HazardsourceManage
{
    /// <summary>
    /// 描 述：监控
    /// </summary>
    [Table("HSD_JKJC")]
    public class JkjcEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 监控人员
        /// </summary>
        /// <returns></returns>
        [Column("JKUSERNAME")]
        public string JkUserName { get; set; }
        /// <summary>
        /// 监控时间
        /// </summary>
        /// <returns></returns>
        [Column("JKTIMESTART")]
        public DateTime? JkTimeStart { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 组份
        /// </summary>
        /// <returns></returns>
        [Column("JKZF")]
        public string Jkzf { get; set; }
        /// <summary>
        /// 监控时间
        /// </summary>
        /// <returns></returns>
        [Column("JKTIMEEND")]
        public DateTime? JkTimeEnd { get; set; }
        /// <summary>
        /// 创建人部门code
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 液位
        /// </summary>
        /// <returns></returns>
        [Column("JKYW")]
        public string Jkyw { get; set; }


        /// <summary>
        /// 修改人
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        /// <returns></returns>
        [Column("JKFILES")]
        public string JkFiles { get; set; }
        /// <summary>
        /// 隐患登记整改id集合
        /// </summary>
        /// <returns></returns>
        [Column("JKYHZGIDS")]
        public string JkyhzgIds { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 创建人orgcode
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        /// <returns></returns>
        [Column("JKOTHER")]
        public string JkOther { get; set; }
        /// <summary>
        /// 受控状态 0不受控,1受控
        /// </summary>
        /// <returns></returns>
        [Column("JKSKSTATUS")]
        public int? JkskStatus { get; set; }
        /// <summary>
        /// 监控人员
        /// </summary>
        /// <returns></returns>
        [Column("JKUSERID")]
        public string JkUserId { get; set; }
        /// <summary>
        /// 关联的重大危险源id
        /// </summary>
        /// <returns></returns>
        [Column("HDID")]
        public string HdId { get; set; }
        /// <summary>
        /// 空气中化学物质浓度
        /// </summary>
        /// <returns></returns>
        [Column("JKHXWZND")]
        public string Jkhxwznd { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 流量
        /// </summary>
        /// <returns></returns>
        [Column("JKLL")]
        public string Jkll { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        /// <returns></returns>
        [Column("JKWD")]
        public string Jkwd { get; set; }
        /// <summary>
        /// 压力
        /// </summary>
        /// <returns></returns>
        [Column("JKYL")]
        public string Jkyl { get; set; }
        /// <summary>
        /// 监控地点
        /// </summary>
        /// <returns></returns>
        [Column("JKAREAR")]
        public string JkArear { get; set; }
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