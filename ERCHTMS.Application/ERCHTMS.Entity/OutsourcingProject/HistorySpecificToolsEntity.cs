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
    /// 描 述：特种设备历史记录
    /// </summary>
    [Table("EPG_HISTORYSPECIFICTOOLS")]
    public class HistorySpecificToolsEntity:BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 设备类型
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSTYPE")]
        public string TOOLSTYPE { get; set; }
        /// <summary>
        /// 所属关系
        /// </summary>
        /// <returns></returns>
        [Column("RELATION")]
        public string RELATION { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSNAME")]
        public string TOOLSNAME { get; set; }
        /// <summary>
        /// 外包单位
        /// </summary>
        /// <returns></returns>
        [Column("OUTCOMPANYNAME")]
        public string OUTCOMPANYNAME { get; set; }
        /// <summary>
        /// 外包单位ID
        /// </summary>
        /// <returns></returns>
        [Column("OUTCOMPANYID")]
        public string OUTCOMPANYID { get; set; }
        /// <summary>
        /// 外包工程
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTNAME")]
        public string OUTPROJECTNAME { get; set; }
        /// <summary>
        /// 外包工程ID
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTID")]
        public string OUTPROJECTID { get; set; }

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
        /// 设备ID
        /// </summary>
        /// <returns></returns>
        [Column("SPECIFICTOOLSID")]
        public string SPECIFICTOOLSID { get; set; }

        /// <summary>
        /// 检验验收附件ID
        /// </summary>
        /// <returns></returns>
        [Column("CHECKREPORTFILE")]
        public string CHECKREPORTFILE { get; set; }

        /// <summary>
        /// 出厂年月
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSMADEDATE")]
        public DateTime? TOOLSMADEDATE { get; set; }
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
        /// 操作人员ID
        /// </summary>
        /// <returns></returns>
        [Column("OPERATIONPEOPLEID")]
        public string OPERATIONPEOPLEID { get; set; }
        /// <summary>
        /// 操作人员
        /// </summary>
        /// <returns></returns>
        [Column("OPERATIONPEOPLE")]
        public string OPERATIONPEOPLE { get; set; }
        /// <summary>
        /// 证书附件ID
        /// </summary>
        /// <returns></returns>
        [Column("REGISTERCARDFILE")]
        public string REGISTERCARDFILE { get; set; }

        /// <summary>
        /// 质量合格证明ID
        /// </summary>
        /// <returns></returns>
        [Column("QUALIFIED")]
        public string QUALIFIED { get; set; }
        /// <summary>
        /// 特种设备档案ID
        /// </summary>
        /// <returns></returns>
        [Column("SPECIFICTOOLSFILE")]
        public string SPECIFICTOOLSFILE { get; set; }
        /// <summary>
        /// 使用登记证书编号
        /// </summary>
        /// <returns></returns>
        [Column("REGISTERNUMBER")]
        public string REGISTERNUMBER { get; set; }
        /// <summary>
        /// 下次检验日期
        /// </summary>
        /// <returns></returns>
        [Column("NEXTCHECKDATE")]
        public DateTime? NEXTCHECKDATE { get; set; }
        /// <summary>
        /// 检验日期
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATE")]
        public DateTime? CHECKDATE { get; set; }
        /// <summary>
        /// 检验周期
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDAYS")]
        public string CHECKDAYS { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        /// <returns></returns>
        [Column("AREANAME")]
        public string AREANAME { get; set; }
        /// <summary>
        /// 所在区域ID
        /// </summary>
        /// <returns></returns>
        [Column("AREAID")]
        public string AREAID { get; set; }
        /// <summary>
        /// 所在区域CODE
        /// </summary>
        /// <returns></returns>
        [Column("AREACODE")]
        public string AREACODE { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSMODEL")]
        public string TOOLSMODEL { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        /// <returns></returns>
        [Column("PHONENUMBER")]
        public string PHONENUMBER { get; set; }
        /// <summary>
        /// 安全管理人员ID
        /// </summary>
        /// <returns></returns>
        [Column("SAFEMANAGERID")]
        public string SAFEMANAGERID { get; set; }
        /// <summary>
        /// 安全管理人员
        /// </summary>
        /// <returns></returns>
        [Column("SAFEMANAGER")]
        public string SAFEMANAGER { get; set; }
        /// <summary>
        /// 管控部门
        /// </summary>
        /// <returns></returns>
        [Column("MANAGERDEPT")]
        public string MANAGERDEPT { get; set; }
        /// <summary>
        /// 管控部门ID
        /// </summary>
        /// <returns></returns>
        [Column("MANAGERDEPTID")]
        public string MANAGERDEPTID { get; set; }

        /// <summary>
        /// 管控部门CODE
        /// </summary>
        /// <returns></returns>
        [Column("MANAGERDEPTCODE")]
        public string MANAGERDEPTCODE { get; set; }
        /// <summary>
        /// 购置时间
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSBUYDATE")]
        public DateTime? TOOLSBUYDATE { get; set; }

        /// <summary>
        /// 工器具信息ID
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSID")]
        public string TOOLSID { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.SPECIFICTOOLSID = string.IsNullOrEmpty(SPECIFICTOOLSID) ? Guid.NewGuid().ToString() : SPECIFICTOOLSID;
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
            this.SPECIFICTOOLSID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}
