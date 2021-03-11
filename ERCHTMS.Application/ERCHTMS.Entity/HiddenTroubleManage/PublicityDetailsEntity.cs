using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// 安全公示详情
    /// </summary>
    [Table("HRS_PUBLICITYDETAILS")]
    public class PublicityDetailsEntity : BaseEntity
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
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
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
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ONE")]
        public string One { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("TWO")]
        public string Two { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("THREE")]
        public string Three { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("FOUR")]
        public string Four { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("FIVE")]
        public string Five { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("SIX")]
        public string Six { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("SEVEN")]
        public string Seven { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("EIGHT")]
        public string Eight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("NINE")]
        public string Nine { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("TEN")]
        public string Ten { get; set; }
        /// <summary>
        /// 主表ID
        /// </summary>
        /// <returns></returns>
        [Column("MAINID")]
        public string MainId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        [Column("NAMEID")]
        public string NameId { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        /// <returns></returns>
        [Column("DEPT")]
        public string Dept { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        /// <returns></returns>
        [Column("POST")]
        public string Post { get; set; }
        /// <summary>
        /// 管理隐患
        /// </summary>
        /// <returns></returns>
        [Column("MANAGERHIDDEN")]
        public int? ManagerHidden { get; set; }
        /// <summary>
        /// 实体隐患
        /// </summary>
        /// <returns></returns>
        [Column("ENTITYHIDDEN")]
        public int? EntityHidden { get; set; }
        /// <summary>
        /// 隐患总数
        /// </summary>
        /// <returns></returns>
        [Column("HIDDENNUM")]
        public int? HiddenNum { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        [Column("SORTNUM")]
        public int? SortNum { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        /// <returns></returns>
        [Column("ORDERNUM")]
        public int? OrderNum { get; set; }
        /// <summary>
        /// 详情序号
        /// </summary>
        /// <returns></returns>
        [Column("DETAILSORDERNUM")]
        public int? DetailsOrderNum { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
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