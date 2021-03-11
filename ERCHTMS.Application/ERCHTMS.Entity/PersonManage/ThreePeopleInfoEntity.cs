using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.PersonManage
{
    /// <summary>
    /// 描 述：三种人审批业务表
    /// </summary>
    [Table("BIS_THREEPEOPLEINFO")]
    public class ThreePeopleInfoEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 关联业务记录Id
        /// </summary>
        /// <returns></returns>
        [Column("APPLYID")]
        public string ApplyId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 考试成绩
        /// </summary>
        /// <returns></returns>
        [Column("SCORE")]
        public int? Score { get; set; }
        /// <summary>
        /// 工作票类型
        /// </summary>
        /// <returns></returns>
        [Column("TICKETTYPE")]
        public string TicketType { get; set; }
        /// <summary>
        /// 所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("ORGCODE")]
        public string OrgCode { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
        
        /// <summary>
        /// 身份证号
        /// </summary>
        /// <returns></returns>
        [Column("IDCARD")]
        public string IdCard { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
            var user = OperatorProvider.Provider.Current();
            if (user != null)
            {
                OrgCode = user.OrganizeCode;
            }
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
                                            }
        #endregion
    }
}