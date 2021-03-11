using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.RiskDatabase
{
    /// <summary>
    /// 描 述：风险预知训练库
    /// </summary>
    [Table("BIS_RISKTRAINLIB")]
    public class RisktrainlibEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 资源准备
        /// </summary>
        /// <returns></returns>
        [Column("RESOURCES")]
        public string Resources { get; set; }
        /// <summary>
        /// 作业描述
        /// </summary>
        /// <returns></returns>
        [Column("WORKDES")]
        public string WorkDes { get; set; }
        /// <summary>
        /// 作业岗位
        /// </summary>
        /// <returns></returns>
        [Column("WORKPOST")]
        public string WorkPost { get; set; }
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
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 工作任务
        /// </summary>
        /// <returns></returns>
        [Column("WORKTASK")]
        public string WorkTask { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 作业类别
        /// </summary>
        /// <returns></returns>
        [Column("WORKTYPE")]
        public string WorkType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 使用次数
        /// </summary>
        /// <returns></returns>
        [Column("USERNUM")]
        public int? UserNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 作业区域
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREA")]
        public string WorkArea { get; set; }
        /// <summary>
        /// 修改次数
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYNUM")]
        public int? ModifyNum { get; set; }
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
        /// 作业区域Id
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREAID")]
        public string WorkAreaId { get; set; }
         /// <summary>
        /// 风险等级
        /// </summary>
        /// <returns></returns>
        [Column("RISKLEVEL")]
        public string RiskLevel { get; set; }
                /// <summary>
        /// 风险等级
        /// </summary>
        /// <returns></returns>
        [Column("RISKLEVELVAL")]
        public string RiskLevelVal { get; set; }
                    /// <summary>
        /// 作业类型
        /// </summary>
        /// <returns></returns>
        [Column("WORKTYPECODE")]
        public string WorkTypeCode { get; set; }
         /// <summary>
        /// 数据来源   1 ：来源风险清单 2 来源本地导入 3 本地数据（新增或修改） 
        /// </summary>
        /// <returns></returns>
        [Column("DATASOURCES")]
        public string DataSources { get; set; }
        
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
           this.UserNum = 0;
           this.ModifyNum = 0;
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