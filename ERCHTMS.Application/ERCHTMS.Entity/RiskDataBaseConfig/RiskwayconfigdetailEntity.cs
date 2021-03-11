using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.RiskDataBaseConfig
{
    /// <summary>
    /// 描 述：安全风险管控取值配置详情表
    /// </summary>
    [Table("BIS_RISKWAYCONFIGDETAIL")]
    public class RiskwayconfigdetailEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
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
        /// 创建电厂Code
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
        /// 配置表Id
        /// </summary>
        /// <returns></returns>
        [Column("WAYCONFIGID")]
        public string WayConfigId { get; set; }
        /// <summary>
        /// 最大取值
        /// </summary>
        /// <returns></returns>
        [Column("MAXVALUE")]
        public decimal? MaxValue { get; set; }
        /// <summary>
        /// 最小取值
        /// </summary>
        /// <returns></returns>
        [Column("MINVALUE")]
        public decimal? MinValue { get; set; }
        /// <summary>
        /// 最大取值符号标示 0:< 1:> 2: = 3:<= 4:>=
        /// </summary>
        /// <returns></returns>
        [Column("MAXSYMBOL")]
        public string MaxSymbol { get; set; }
        /// <summary>
        /// 最小取值符号标示 0:< 1:> 2: = 3:<= 4:>=
        /// </summary>
        /// <returns></returns>
        [Column("MINSYMBOL")]
        public string MinSymbol { get; set; }
        /// <summary>
        /// 风险等级
        /// </summary>
        /// <returns></returns>
        [Column("RISKLEVEL")]
        public string RiskLevel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
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