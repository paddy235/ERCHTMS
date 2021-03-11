using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患评估信息表
    /// </summary>
    [Table("BIS_HTAPPROVAL")]
    public class HTApprovalEntity : BaseEntity
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
        /// 隐患编号
        /// </summary>
        /// <returns></returns>
        [Column("HIDCODE")]
        public string HIDCODE { get; set; }
        /// <summary>
        /// 评估人
        /// </summary>
        /// <returns></returns>
        [Column("APPROVALPERSON")]
        public string APPROVALPERSON { get; set; }
        /// <summary>
        /// 评估人姓名
        /// </summary>
        /// <returns></returns>
        [Column("APPROVALPERSONNAME")]
        public string APPROVALPERSONNAME { get; set; }
        /// <summary>
        /// 评估单位编码
        /// </summary>
        /// <returns></returns>
        [Column("APPROVALDEPARTCODE")]
        public string APPROVALDEPARTCODE { get; set; }
        /// <summary>
        /// 评估单位名称
        /// </summary>
        /// <returns></returns>
        [Column("APPROVALDEPARTNAME")]
        public string APPROVALDEPARTNAME { get; set; }
        /// <summary>
        /// 评估时间
        /// </summary>
        /// <returns></returns>
        [Column("APPROVALDATE")]
        public DateTime? APPROVALDATE { get; set; }
        /// <summary>
        /// 评估结果
        /// </summary>
        /// <returns></returns>
        [Column("APPROVALRESULT")]
        public string APPROVALRESULT { get; set; }
        /// <summary>
        /// 不予评估原因
        /// </summary>
        /// <returns></returns>
        [Column("APPROVALREASON")]
        public string APPROVALREASON { get; set; }
        /// <summary>
        /// 自动增量
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int AUTOID { get; set; }

        /// <summary>
        /// 核准附件
        /// </summary>
        /// <returns></returns>
        [Column("APPROVALFILE")]
        public string APPROVALFILE { get; set; }


        /// <summary>
        /// 应用标记(web or  android  or ios)
        /// </summary>
        [Column("APPSIGN")]
        public string APPSIGN { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = Guid.NewGuid().ToString();
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
            this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
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