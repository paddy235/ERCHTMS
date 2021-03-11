using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.FireManage
{
    /// <summary>
    /// 描 述：日常巡查（内容）
    /// </summary>
    [Table("HRS_EVERYDAYPATROLDETAIL")]
    public class EverydayPatrolDetailEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AutoId { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
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
        /// <summary>
        /// 巡查ID
        /// </summary>
        /// <returns></returns>
        [Column("PATROLID")]
        public string PatrolId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("RESULTTRUE")]
        public string ResultTrue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("RESULTFALSE")]
        public string ResultFalse { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("RESULT")]
        public int? Result { get; set; }
        /// <summary>
        /// 巡查内容
        /// </summary>
        /// <returns></returns>
        [Column("PATROLCONTENT")]
        public string PatrolContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("PROBLEM")]
        public string Problem { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DISPOSE")]
        public string Dispose { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        /// <returns></returns>
        [Column("ORDERNUMBER")]
        public int OrderNumber { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
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
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }


    public class EverydayPatrolDetailTjEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 巡查ID
        /// </summary>
        /// <returns></returns>
        public string PatrolId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ResultTrue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ResultFalse { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? Result { get; set; }
        /// <summary>
        /// 巡查内容
        /// </summary>
        /// <returns></returns>
        public string PatrolContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Problem { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Dispose { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        /// <returns></returns>
        public int OrderNumber { get; set; }
        public int YhCount { get; set; }
        public int WzCount { get; set; }
        #endregion

    }
}