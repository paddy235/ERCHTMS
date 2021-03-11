using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病人详情表
    /// </summary>
    [Table("BIS_OCCUPATIONALSTAFFDETAIL")]
    public class OccupationalstaffdetailEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 体检ID
        /// </summary>
        /// <returns></returns>
        [Column("OCCDETAILID")]
        public string OccDetailId { get; set; }
        /// <summary>
        /// 体检ID
        /// </summary>
        /// <returns></returns>
        [Column("OCCID")]
        public string OccId { get; set; }
        /// <summary>
        /// 体检人ID
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 人员名称
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
        /// <summary>
        /// 姓氏拼音（首字母）
        /// </summary>
        /// <returns></returns>
        [Column("USERNAMEPINYIN")]
        public string UserNamePinYin { get; set; }
        /// <summary>
        /// 体检时间
        /// </summary>
        /// <returns></returns>
        [Column("INSPECTIONTIME")]
        public DateTime? InspectionTime { get; set; }
        /// <summary>
        /// 是否有职业病
        /// </summary>
        /// <returns></returns>
        [Column("ISSICK")]
        public int? Issick { get; set; }
        /// <summary>
        /// 职业病种类
        /// </summary>
        /// <returns></returns>
        [Column("SICKTYPE")]
        public string SickType { get; set; }

        /// <summary>
        /// 职业病种类名称
        /// </summary>
        /// <returns></returns>
        [Column("SICKTYPENAME")]
        public string SickTypeName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("NOTE")]
        public string Note { get; set; }
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
        /// 异常描述
        /// </summary>
        [Column("UNUSUALNOTE")]
        public string UnusualNote { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.OccDetailId = string.IsNullOrEmpty(OccDetailId) ? Guid.NewGuid().ToString() : OccDetailId;
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
            this.OccDetailId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
        }
        #endregion
    }
}