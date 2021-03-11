using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EnvironmentalManage
{
    /// <summary>
    /// 描 述：水质分析记录
    /// </summary>
    [Table("BIS_WATERRECORD")]
    public class WaterrecordEntity : BaseEntity
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
        /// 试验方法
        /// </summary>
        /// <returns></returns>
        [Column("TESTMETHOD")]
        public string TESTMETHOD { get; set; }

        /// <summary>
        /// 样品类型
        /// </summary>
        /// <returns></returns>
        [Column("SAMPLETYPE")]
        public string SampleType { get; set; }
        /// <summary>
        /// 试验项目
        /// </summary>
        /// <returns></returns>
        [Column("TESTPROJECT")]
        public string TESTPROJECT { get; set; }
        /// <summary>
        /// 考核指标
        /// </summary>
        /// <returns></returns>
        [Column("KPITARGET")]
        public string KPITARGET { get; set; }
        /// <summary>
        /// 参考依据
        /// </summary>
        /// <returns></returns>
        [Column("REFACCORDING")]
        public string REFACCORDING { get; set; }
        /// <summary>
        /// 试验结果
        /// </summary>
        /// <returns></returns>
        [Column("TESTRESULT")]
        public string TESTRESULT { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        /// <returns></returns>
        [Column("UNIT")]
        public string UNIT { get; set; }
     
        /// <summary>
        /// 项目code
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTCODE")]
        public string PROJECTCODE { get; set; }
        
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
}