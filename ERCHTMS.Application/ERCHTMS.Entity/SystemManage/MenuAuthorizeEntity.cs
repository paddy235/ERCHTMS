using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.SystemManage
{
    /// <summary>
    /// 菜单单位授权
    /// </summary>
    [Table("BASE_MENUAUTHORIZE")]
    public class MenuAuthorizeEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键ID
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
        /// 单位编码
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTCODE")]
        public string DepartCode { get; set; }
        /// <summary>
        /// 单位ID
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTID")]
        public string DepartId { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTNAME")]
        public string DepartName { get; set; }
        /// <summary>
        /// 上级Id
        /// </summary>
        /// <returns></returns>
        [Column("PARENTID")]
        public string ParentId { get; set; }
        /// <summary>
        /// 上级名称
        /// </summary>
        /// <returns></returns>
        [Column("PARENTNAME")]
        public string ParentName { get; set; }
        /// <summary>
        /// 单位展示的名称
        /// </summary>
        /// <returns></returns>
        [Column("DISPLAYNAME")]
        public string DisplayName { get; set; }
        /// <summary>
        /// Logo图片地支
        /// </summary>
        /// <returns></returns>
        [Column("LOGOURL")]
        public string LogoUrl { get; set; }
        /// <summary>
        /// 注册码
        /// </summary>
        /// <returns></returns>
        [Column("REGISTCODE")]
        public string RegistCode { get; set; }
        /// <summary>
        /// 班组后台接口地址
        /// </summary>
        /// <returns></returns>
        [Column("BZAPIURL")]
        public string BZApiUrl { get; set; }
        /// <summary>
        /// 双控后台接口地址
        /// </summary>
        /// <returns></returns>
        [Column("SKAPIURL")]
        public string SKApiUrl { get; set; }
        /// <summary>
        /// 培训平台后台接口地址
        /// </summary>
        /// <returns></returns>
        [Column("PXAPIURL")]
        public string PXApiUrl { get; set; }
        /// <summary>
        /// 班组文化墙地址
        /// </summary>
        /// <returns></returns>
        [Column("CULTURALURL")]
        public string CulturalUrl { get; set; }
        /// <summary>
        /// 班组终端硬件型号
        /// </summary>
        /// <returns></returns>
        [Column("TERMINALCODE")]
        public string TerminalCode { get; set; }
        /// <summary>
        /// 班组终端使用主题
        /// </summary>
        /// <returns></returns>
        [Column("THEMETYPE")]
        public int ThemeType { get; set; }
        /// <summary>
        /// 版本型号
        /// </summary>
        /// <returns></returns>
        [Column("VERSIONCODE")]
        public int VersionCode { get; set; }
        /// <summary>
        /// 首页地址
        /// </summary>
        [Column("INDEXURL")]
        public string IndexUrl { get; set; }


        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
            this.VersionCode = 0;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
            this.Id = keyValue;
            this.VersionCode++;
        }
        #endregion
    }
}
