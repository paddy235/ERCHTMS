using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.LllegalManage
{
    /// <summary>
    /// 描 述：违章档案扣分表
    /// </summary>
    [Table("BIS_LLLEGALDEDUCTMARKS")]
    public class LllegalDeductMarksEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
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
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// 关联违章id
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALID")]
        public string LLLEGALID { get; set; }
        /// <summary>
        /// 关联违章考核id
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHID")]
        public string PUNISHID { get; set; }
        /// <summary>
        /// 扣分人员
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string USERID { get; set; }
        /// <summary>
        /// 扣分人员
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string USERNAME { get; set; }
        /// <summary>
        /// 岗位/职务
        /// </summary>
        /// <returns></returns>
        [Column("DUTYNAME")]
        public string DUTYNAME { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DEPTID { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DEPTNAME { get; set; }
        /// <summary>
        /// 专业/班组
        /// </summary>
        /// <returns></returns>
        [Column("TEAMID")]
        public string TEAMID { get; set; }

        /// <summary>
        /// 专业/班组
        /// </summary>
        /// <returns></returns>
        [Column("TEAMNAME")]
        public string TEAMNAME { get; set; }
        /// <summary>
        /// 处罚时间
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHDATE")]
        public DateTime? PUNISHDATE { get; set; }
        /// <summary>
        /// 处罚结果
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHRESULT")]
        public decimal? PUNISHRESULT { get; set; }
        /// <summary>
        /// 扣除积分
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHPOINT")]
        public decimal? PUNISHPOINT { get; set; }
        /// <summary>
        /// 违章类型
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALTYPENAME")]
        public string LLLEGALTYPENAME { get; set; }
        /// <summary>
        /// 违章类型
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALTYPE")]
        public string LLLEGALTYPE { get; set; }
        /// <summary>
        /// 违章描述
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALDESCRIBE")]
        public string LLLEGALDESCRIBE { get; set; }
        /// <summary>
        /// 应用标记
        /// </summary>
        /// <returns></returns>
        [Column("APPSIGN")]
        public string APPSIGN { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
                /// 新增调用
                /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
            this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}