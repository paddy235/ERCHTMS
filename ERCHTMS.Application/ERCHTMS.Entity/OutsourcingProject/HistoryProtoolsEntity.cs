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
    /// 描 述：施工机具历史记录
    /// </summary>
    [Table("EPG_HISTORYPROJECTTOOLS")]
    public class HistoryProtoolsEntity:BaseEntity
    {
        #region 实体成员
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
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
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
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 所属关系部门名称
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSDEPTNAME")]
        public string TOOLSDEPTNAME { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSCOUNT")]
        public int? TOOLSCOUNT { get; set; }
        /// <summary>
        /// 购置时间
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSBUYTIME")]
        public DateTime? TOOLSBUYTIME { get; set; }
        /// <summary>
        /// 出厂年月
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSINITTIME")]
        public string TOOLSINITTIME { get; set; }
        /// <summary>
        /// 出厂编号
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSINITNUMBER")]
        public string TOOLSINITNUMBER { get; set; }
        /// <summary>
        /// 制造单位名称
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSMADECOMPANY")]
        public string TOOLSMADECOMPANY { get; set; }
        /// <summary>
        /// 最近检验日期
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSCHECKDATE")]
        public DateTime? TOOLSCHECKDATE { get; set; }
        /// <summary>
        /// 检验周期
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDAYS")]
        public int? CHECKDAYS { get; set; }
        /// <summary>
        /// 下次检验日期
        /// </summary>
        /// <returns></returns>
        [Column("NEXTCHECKDATE")]
        public string NEXTCHECKDATE { get; set; }
        /// <summary>
        /// 设备机具档案
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSFILE")]
        public string TOOLSFILE { get; set; }
        /// <summary>
        /// 工器具信息ID
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSID")]
        public string TOOLSID { get; set; }
        /// <summary>
        /// 主键ＩＤ
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSNAME")]
        public string TOOLSNAME { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSTYPE")]
        public string TOOLSTYPE { get; set; }
        /// <summary>
        /// 所属关系部门ID
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSDEPTID")]
        public string TOOLSDEPTID { get; set; }
        /// <summary>
        /// 设备分类
        /// </summary>
        [Column("TOOLTYPE")]
        public string TOOLTYPE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK")]
        public string REMARK { get; set; }

        /// <summary>
        /// 是否验收通过状态 0:是 1:否
        /// </summary>
        [Column("STATUS")]
        public string Status { get; set; }

        /// <summary>
        /// 验收意见
        /// </summary>
        [Column("CHECKOPTION")]
        public string CheckOption { get; set; }
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
