using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ERCHTMS.Entity.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查内容表
    /// </summary>
    [Table("BIS_SAFTYCONTENT")]
    public class SaftyCheckContentEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 检查详情ID
        /// </summary>
        /// <returns></returns>
        [Column("DETAILID")]
        public string DetailId { get; set; }
        /// <summary>
        /// 区域ID
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTID")]
        public string DistrictId { get; set; }
        /// <summary>
        /// 检查人姓名
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMANNAME")]
        public string CheckManName { get; set; }
        /// <summary>
        /// 检查人账号
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMANACCOUNT")]
        public string CheckManAccount { get; set; }
        /// <summary>
        /// 检查内容
        /// </summary>
        /// <returns></returns>
        [Column("SAFTYCONTENT")]
        public string SaftyContent { get; set; }
        /// <summary>
        /// 是否登记
        /// </summary>
        /// <returns></returns>
        [Column("ISREGISER")]
        public int? IsRegiser { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 关联ID
        /// </summary>
        /// <returns></returns>
        [Column("RECID")]
        public string Recid { get; set; }

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
        /// 是否符合(0:不符合，1:符合)
        /// </summary>
        /// <returns></returns>
        [Column("ISSURE")]
        public string IsSure { get; set; }
        /// <summary>
        ///备注
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
            this.ID = Guid.NewGuid().ToString();
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
            //this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}
