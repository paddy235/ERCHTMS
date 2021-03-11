using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.LaborProtectionManage
{
    /// <summary>
    /// 描 述：劳动防护信息关联表
    /// </summary>
    [Table("BIS_LABOREQUIPMENTINFO")]
    public class LaborequipmentinfoEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 关联ID
        /// </summary>
        /// <returns></returns>
        [Column("ASSID")]
        public string AssId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 尺码
        /// </summary>
        /// <returns></returns>
        [Column("SIZE")]
        public string Size { get; set; }
        /// <summary>
        /// 配备数量
        /// </summary>
        /// <returns></returns>
        [Column("SHOULDNUM")]
        public int? ShouldNum { get; set; }
        /// <summary>
        /// 用品有效期
        /// </summary>
        /// <returns></returns>
        [Column("VALIDITYPERIOD")]
        public DateTime? ValidityPeriod { get; set; }
        /// <summary>
        /// 报废原因
        /// </summary>
        /// <returns></returns>
        [Column("RESON")]
        public string Reson { get; set; }
        /// <summary>
        /// 关联类型 0为关联物品表 1为关联发放 2为关联报废
        /// </summary>
        /// <returns></returns>
        [Column("LABORTYPE")]
        public int? LaborType { get; set; }

        /// <summary>
        /// 品牌厂家
        /// </summary>
        /// <returns></returns>
        [Column("BRAND")]
        public string Brand { get; set; }
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
                    }
        #endregion
    }

    public class Laborff
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        public string ID { get; set; }
        /// <summary>
        /// 用品型号
        /// </summary>
        /// <returns></returns>
        public string Model { get; set; }
        /// <summary>
        /// 用品名称
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }
       
        /// <summary>
        /// 用品单位
        /// </summary>
        /// <returns></returns>
        public string Unit { get; set; }
        /// <summary>
        /// 发放数量
        /// </summary>
        /// <returns></returns>
        public int? IssueNum { get; set; }
        /// <summary>
        /// 使用部门名称
        /// </summary>
        /// <returns></returns>
        public string DeptName { get; set; }
        /// <summary>
        /// 使用单位名称
        /// </summary>
        /// <returns></returns>
        [Column("ORGNAME")]
        public string OrgName { get; set; }
        /// <summary>
        /// 使用岗位名称
        /// </summary>
        /// <returns></returns>
        public string PostName { get; set; }
        /// <summary>
        /// 发放人数
        /// </summary>
        /// <returns></returns>
        public int UserCount { get; set; }

        /// <summary>
        /// 发放数量/人
        /// </summary>
        /// <returns></returns>
        public int PerCount { get; set; }
        /// <summary>
        /// 发放总数
        /// </summary>
        /// <returns></returns>
        public int Count { get; set; }

    }
}