using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.LllegalManage
{
    /// <summary>
    /// 描 述：违章整改信息
    /// </summary>
    [Table("BIS_LLLEGALREFORM")]
    public class LllegalReformEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 应用标记(web or  android  or ios)
        /// </summary>
        [Column("APPSIGN")]
        public string APPSIGN { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 整改人
        /// </summary>
        /// <returns></returns>
        [Column("REFORMPEOPLE")]
        public string REFORMPEOPLE { get; set; }
        /// <summary>
        /// 整改人
        /// </summary>
        /// <returns></returns>
        [Column("REFORMPEOPLEID")]
        public string REFORMPEOPLEID { get; set; }
        /// <summary>
        /// 整改人电话
        /// </summary>
        /// <returns></returns>
        [Column("REFORMTEL")]
        public string REFORMTEL { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 整改措施
        /// </summary>
        /// <returns></returns>
        [Column("REFORMMEASURE")]
        public string REFORMMEASURE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 整改截止时间
        /// </summary>
        /// <returns></returns>
        [Column("REFORMDEADLINE")]
        public DateTime? REFORMDEADLINE { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// 整改结果
        /// </summary>
        /// <returns></returns>
        [Column("REFORMSTATUS")]
        public string REFORMSTATUS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
        /// <summary>
        /// 整改图片
        /// </summary>
        /// <returns></returns>
        [Column("REFORMPIC")]
        public string REFORMPIC { get; set; }

        /// <summary>
        /// 整改附件
        /// </summary>
        /// <returns></returns>
        [Column("REFORMATTACHMENT")]
        public string REFORMATTACHMENT { get; set; }
        
        /// <summary>
        /// 整改部门
        /// </summary>
        /// <returns></returns>
        [Column("REFORMDEPTNAME")]
        public string REFORMDEPTNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 整改部门
        /// </summary>
        /// <returns></returns>
        [Column("REFORMDEPTCODE")]
        public string REFORMDEPTCODE { get; set; }
        /// <summary>
        /// 违章id
        /// </summary>
        /// <returns></returns>
        [Column("LLLEGALID")]
        public string LLLEGALID { get; set; }
        /// <summary>
        /// 整改完成时间
        /// </summary>
        /// <returns></returns>
        [Column("REFORMFINISHDATE")]
        public DateTime? REFORMFINISHDATE { get; set; }


        /// <summary>
        ///整改责任负责人
        /// </summary>
        [Column("REFORMCHARGEPERSON")]
        public string REFORMCHARGEPERSON { get; set; }

        /// <summary>
        /// 整改责任负责人姓名
        /// </summary>
        [Column("REFORMCHARGEPERSONNAME")]
        public string REFORMCHARGEPERSONNAME { get; set; }

        /// <summary>
        /// 整改责任负责人单位编码
        /// </summary>
        [Column("REFORMCHARGEDEPTID")]
        public string REFORMCHARGEDEPTID { get; set; }

        /// <summary>
        /// 整改责任负责人单位名称
        /// </summary>
        [Column("REFORMCHARGEDEPTNAME")]
        public string REFORMCHARGEDEPTNAME { get; set; }

        /// <summary>
        /// 整改责任负责人单位名称
        /// </summary>
        [Column("ISAPPOINT")]
        public string ISAPPOINT { get; set; }


        /// <summary>
        /// 延期整改参与人 POSTPONEPERSON
        /// </summary>
        [Column("POSTPONEPERSON")]
        public string POSTPONEPERSON { get; set; }
        /// <summary>
        /// 整改人
        /// </summary>
        [Column("POSTPONEPERSONNAME")]
        public string POSTPONEPERSONNAME { get; set; }
        /// <summary>
        /// 延期整改批复部门名称 
        /// </summary>
        [Column("POSTPONEDEPTNAME")]
        public string POSTPONEDEPTNAME { get; set; }
        /// <summary>
        /// 延期整改批复部门Code
        /// </summary>
        [Column("POSTPONEDEPT")]
        public string POSTPONEDEPT { get; set; }
        /// <summary>
        /// 延期天数 
        /// </summary>
        [Column("POSTPONEDAYS")]
        public int? POSTPONEDAYS { get; set; }

        /// <summary>
        /// 申请延期状态 
        /// </summary>
        [Column("APPLICATIONSTATUS")]
        public string APPLICATIONSTATUS { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = DateTime.Now;
            if (string.IsNullOrEmpty(this.CREATEUSERID))
            {
                this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            }
            if (string.IsNullOrEmpty(this.CREATEUSERNAME))
            {
                this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
            }
            if (string.IsNullOrEmpty(this.CREATEUSERDEPTCODE))
            {
                this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            }
            if (string.IsNullOrEmpty(this.CREATEUSERORGCODE))
            {
                this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
            }
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
