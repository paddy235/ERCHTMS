using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查表详情
    /// </summary>
    [Table("BIS_SAFTYCHECKDATADETAILED")]
    public class SaftyCheckDataDetailEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 所属区域
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDISTRICT")]
        public string BelongDistrict { get; set; }
        /// <summary>
        /// 所属区域主键
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDISTRICTID")]
        public string BelongDistrictID { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPT")]
        public string BelongDept { get; set; }
        /// <summary>
        /// 所属部门主键
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTID")]
        public string BelongDeptID { get; set; }
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
        /// 修改日期
        /// </summary>		
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>		
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>		
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 检查内容
        /// </summary>
        /// <returns></returns>
        [Column("CHECKCONTENT")]
        public string CheckContent { get; set; }
        /// <summary>
        /// 风险点名称
        /// </summary>
        /// <returns></returns>
        [Column("RISKNAME")]
        public string RiskName { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 关联主表主键
        /// </summary>
        /// <returns></returns>
        [Column("RECID")]
        public string RecID { get; set; }
        /// <summary>
        /// 所属隐患数量
        /// </summary>
        /// <returns></returns>
        [Column("COUNT")]
        public int? Count { get; set; }
        [NotMapped]
        /// <summary>
        /// 违章数量
        /// </summary>
        public string WzCount { get; set; }
        [NotMapped]
        /// <summary>
        /// 问题数量
        /// </summary>
        public string WtCount { get; set; }

        /// <summary>
        /// 所属隐患数量
        /// </summary>
        /// <returns></returns>
        [Column("COUNTID")]
        public string CountID { get; set; }

        /// <summary>
        /// 检查状态
        /// </summary>
        /// <returns></returns>
        [Column("CHECKSTATE")]
        public int? CheckState { get; set; }
        /// <summary>
        /// 检查人
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMAN")]
        public string CheckMan { get; set; }
        /// <summary>
        /// 检查人主键
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMANID")]
        public string CheckManID { get; set; }
        /// <summary>
        /// 所属区域主键
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDISTRICTCODE")]
        public string BelongDistrictCode { get; set; }
        /// <summary>
        /// 检查表ID
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATAID")]
        public string CheckDataId { get; set; }
        /// <summary>
        /// 检查对象
        /// </summary>
        /// <returns></returns>
        [Column("CHECKOBJECT")]
        public string CheckObject { get; set; }
        /// <summary>
        /// 检查对象ID
        /// </summary>
        /// <returns></returns>
        [Column("CHECKOBJECTID")]
        public string CheckObjectId { get; set; }
        /// <summary>
        /// 检查对象类型 0为设备 1为危险源
        /// </summary>
        /// <returns></returns>
        [Column("CHECKOBJECTTYPE")]
        public string CheckObjectType { get; set; }
        /// <summary>
        /// 检查内容
        /// </summary>
        /// <returns></returns>
        public List<SaftyCheckContentEntity> Content { get; set; }
        /// <summary>
        /// 是否符合(0:不符合，1:符合)
        /// </summary>
        /// <returns></returns>
        /// <returns></returns>
        [Column("ISSURE")]
        public string IsSure { get; set; }
        /// <summary>
        ///备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        ///排序字段
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AutoId { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID =string.IsNullOrEmpty(ID)? Guid.NewGuid().ToString():ID;
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