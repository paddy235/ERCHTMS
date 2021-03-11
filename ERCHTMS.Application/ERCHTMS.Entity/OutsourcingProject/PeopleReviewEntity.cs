using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：人员资质审查表
    /// </summary>
    [Table("EPG_PEOPLEREVIEW")]
    public class PeopleReviewEntity : BaseEntity
    {
        #region 实体成员
        [Column("ID")]
        public string ID { get; set; }

        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }

        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }

        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }

        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }

        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 工程Id
        /// </summary>
        /// <returns></returns>
        [Column("OUTENGINEERID")]
        public string OUTENGINEERID { get; set; }
        /// <summary>
        /// 工程名称
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERNAME")]
        public string ENGINEERNAME { get; set; }
        /// <summary>
        /// 下一个审核单位Code
        /// </summary>
        /// <returns></returns>
        [Column("NEXTAUDITDEPTCODE")]
        public string NEXTAUDITDEPTCODE { get; set; }
        /// <summary>
        /// 是否审核完成 0：未完成 1：完成
        /// </summary>
        /// <returns></returns>
        [Column("ISAUDITOVER")]
        public string ISAUDITOVER { get; set; }
        /// <summary>
        /// 下一个审核单位Id
        /// </summary>
        /// <returns></returns>
        [Column("NEXTAUDITDEPTID")]
        public string NEXTAUDITDEPTID { get; set; }


        /// <summary>
        /// 下一个审核角色
        /// </summary>
        /// <returns></returns>
        [Column("NEXTAUDITROLE")]
        public string NEXTAUDITROLE { get; set; }
        /// <summary>
        /// 是否提交 0：未提交 1：已提交
        /// </summary>
        /// <returns></returns>
        [Column("ISSAVEORCOMMIT")]
        public string ISSAVEORCOMMIT { get; set; }
        /// <summary>
        /// 审核流程Id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
            this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}
