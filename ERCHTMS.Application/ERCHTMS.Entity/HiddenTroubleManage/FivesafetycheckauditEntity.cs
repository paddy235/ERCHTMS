using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：五定安全检查 整改验收表
    /// </summary>
    [Table("BIS_FIVESAFETYCHECKAUDIT")]
    public class FivesafetycheckauditEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 验收意见
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTCONTENT")]
        public string ACCEPTCONTENT { get; set; }
        /// <summary>
        /// 验收结果  
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTREUSLT")]
        public string ACCEPTREUSLT { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("BEIZHU")]
        public string BEIZHU { get; set; }
        /// <summary>
        /// 实际完成时间
        /// </summary>
        /// <returns></returns>
        [Column("ACTUALDATE")]
        public DateTime? ACTUALDATE { get; set; }
        /// <summary>
        /// 整改结果
        /// </summary>
        /// <returns></returns>
        [Column("ACTIONRESULT")]
        public string ACTIONRESULT { get; set; }
        /// <summary>
        /// 验收人ID
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTUSERID")]
        public string ACCEPTUSERID { get; set; }
        /// <summary>
        /// 验收人
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTUSER")]
        public string ACCEPTUSER { get; set; }
        /// <summary>
        /// 要求完成时间
        /// </summary>
        /// <returns></returns>
        [Column("FINISHDATE")]
        public DateTime? FINISHDATE { get; set; }
        /// <summary>
        /// 责任部门ID
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPTID")]
        public string DUTYDEPTID { get; set; }
        /// <summary>
        /// 责任部门
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPT")]
        public string DUTYDEPT { get; set; }
        /// <summary>
        /// 责任部门code
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPTCODE")]
        public string DUTYDEPTCODE { get; set; }
        /// <summary>
        /// 责任人D
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERID")]
        public string DUTYUSERID { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERNAME")]
        public string DUTYUSERNAME { get; set; }
        /// <summary>
        /// 整改措施
        /// </summary>
        /// <returns></returns>
        [Column("ACTIONCONTENT")]
        public string ACTIONCONTENT { get; set; }
        /// <summary>
        /// 发现问题
        /// </summary>
        /// <returns></returns>
        [Column("FINDQUESTION")]
        public string FINDQUESTION { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
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
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
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
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// 检查主表关联id
        /// </summary>
        /// <returns></returns>
        [Column("CHECKID")]
        public string CHECKID { get; set; }

        /// <summary>
        /// 审批类型  0:保存检查表直接整改通过  1：走正常整改流程
        /// </summary>
        /// <returns></returns>
        [Column("CHECKPASS")]
        public string CHECKPASS { get; set; }

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