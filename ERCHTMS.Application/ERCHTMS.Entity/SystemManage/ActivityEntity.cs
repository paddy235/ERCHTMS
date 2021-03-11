using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SystemManage
{
    /// <summary>
    /// 描 述：流程活动
    /// </summary>
    [Table("SYS_WFTBACTIVITY")]
    public class ActivityEntity : BaseEntity
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
        [Column("TAG")]
        public string Tag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("FORMHEIGHT")]
        public int? FormHeight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ALLOWCANCEL")]
        public int? AllowCancel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MOVEPROCESSNAME")]
        public string MoveProcessName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("GRAPHTOP")]
        public int? Graphtop { get; set; }
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
        [Column("OPERUSER")]
        public string OperUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AutoId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("FORMWIDTH")]
        public int? FormWidth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ISORDERSIGN")]
        public int? IsOrderSign { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("UNSHOWNPREVDIALOG")]
        public int? UnShowNPrevDialog { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("FORMNAME")]
        public string FormName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSER")]
        public string CreateUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ACTIVITYORDER")]
        public int? ActivityOrder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("KIND")]
        public string Kind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("OPERDATE")]
        public DateTime? OperDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ISDYNAMICSIGNER")]
        public int? IsDynamicSigner { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTONEXTACTIVITYID")]
        public string AutoNextActivityId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("GRAPHLEFT")]
        public int? GraphLeft { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("PROCESSID")]
        public string ProcessId { get; set; } 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("STAYTIMESPAN")]
        public int? StayTimeSpan { get; set; } 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ALLOWBACK")]
        public int? AllowBack { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("SIGNTYPE")]
        public int? SignType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("UNSHOWNNEXTDIALOG")]
        public int? UnShowNNextDialog { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("INNERHANDLER")]
        public int? InnerHandler { get; set; } 
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            this.OperDate = DateTime.Now;
            this.OperUser = OperatorProvider.Provider.Current().UserId;
            this.CreateUser = OperatorProvider.Provider.Current().UserId;
            //this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            //this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.OperDate = DateTime.Now;
            this.OperUser = OperatorProvider.Provider.Current().UserId;
            this.Id = keyValue;
        }
        #endregion
    }
}