using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患整改信息表
    /// </summary>
    [Table("BIS_HTCHANGEINFO")]
    public class HTChangeInfoEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 整改附件
        /// </summary>
        [Column("ATTACHMENT")]
        public string ATTACHMENT { get; set; }
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
        public int POSTPONEDAYS { get; set; }

        /// <summary>
        /// 申请延期状态 
        /// </summary>
        [Column("APPLICATIONSTATUS")]
        public string APPLICATIONSTATUS { get; set; }

        /// <summary>
        /// 整改期限(天数)
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEDEADINENUM")]
        public int? CHANGEDEADINENUM { get; set; }
        /// <summary>
        /// 重大隐患治理方案
        /// </summary>
        /// <returns></returns>
        [Column("GREATHIDPROJECT")]
        public string GREATHIDPROJECT { get; set; }
        /// <summary>
        /// 纳入治理计划日期
        /// </summary>
        /// <returns></returns>
        [Column("MANAGEPLANDATE")]
        public DateTime? MANAGEPLANDATE { get; set; }
        /// <summary>
        /// 纳入治理计划情况
        /// </summary>
        /// <returns></returns>
        [Column("MANAGEPLANSTATUS")]
        public string MANAGEPLANSTATUS { get; set; }
        /// <summary>
        /// 应急预案到位日期
        /// </summary>
        /// <returns></returns>
        [Column("PREVENTFINISHDATE")]
        public DateTime? PREVENTFINISHDATE { get; set; }
        /// <summary>
        /// 应急预案到位情况
        /// </summary>
        /// <returns></returns>
        [Column("PREVENTFINISHSTATUS")]
        public string PREVENTFINISHSTATUS { get; set; }
        /// <summary>
        /// 整改时限到位日期
        /// </summary>
        /// <returns></returns>
        [Column("DEADINEFINISHDATE")]
        public DateTime? DEADINEFINISHDATE { get; set; }
        /// <summary>
        /// 整改时限到位情况
        /// </summary>
        /// <returns></returns>
        [Column("DEADINEFINISHSTATUS")]
        public string DEADINEFINISHSTATUS { get; set; }
        /// <summary>
        /// 整改资金到位日期
        /// </summary>
        /// <returns></returns>
        [Column("CAPITALFINISHDATE")]
        public DateTime? CAPITALFINISHDATE { get; set; }
        /// <summary>
        /// 整改资金到位情况
        /// </summary>
        /// <returns></returns>
        [Column("CAPITALFINISHSTATUS")]
        public string CAPITALFINISHSTATUS { get; set; }
        /// <summary>
        /// 治理措施到位日期
        /// </summary>
        /// <returns></returns>
        [Column("MEASUREFINISHDATE")]
        public DateTime? MEASUREFINISHDATE { get; set; }
        /// <summary>
        /// 治理措施到位情况
        /// </summary>
        /// <returns></returns>
        [Column("MEASUREFINISHSTATUS")]
        public string MEASUREFINISHSTATUS { get; set; }
        /// <summary>
        /// 整改责任到位日期
        /// </summary>
        /// <returns></returns>
        [Column("DUTYFINISHDATE")]
        public DateTime? DUTYFINISHDATE { get; set; }
        /// <summary>
        /// 整改责任到位情况
        /// </summary>
        /// <returns></returns>
        [Column("DUTYFINISHSTATUS")]
        public string DUTYFINISHSTATUS { get; set; }
        /// <summary>
        /// 整改目标到位日期
        /// </summary>
        /// <returns></returns>
        [Column("TARGETFINISHDATE")]
        public DateTime? TARGETFINISHDATE { get; set; }
        /// <summary>
        /// 整改目标到位情况
        /// </summary>
        /// <returns></returns>
        [Column("TARGETFINISHSTATUS")]
        public string TARGETFINISHSTATUS { get; set; }
        /// <summary>
        /// 回退原因
        /// </summary>
        /// <returns></returns>
        [Column("BACKREASON")]
        public string BACKREASON { get; set; }
        /// <summary>
        /// 隐患整改相片
        /// </summary>
        /// <returns></returns>
        [Column("HIDCHANGEPHOTO")]
        public string HIDCHANGEPHOTO { get; set; }
        /// <summary>
        /// 整改情况描述
        /// </summary>
        /// <returns></returns>
        [Column("CHANGERESUME")]
        public string CHANGERESUME { get; set; }
        /// <summary>
        /// 整改结果
        /// </summary>
        /// <returns></returns>
        [Column("CHANGERESULT")]
        public string CHANGERESULT { get; set; }
        /// <summary>
        /// 整改完成时间
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEFINISHDATE")]
        public DateTime? CHANGEFINISHDATE { get; set; }
        /// <summary>
        /// 整改措施
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEMEASURE")]
        public string CHANGEMEASURE { get; set; }
        /// <summary>
        /// 实际治理资金
        /// </summary>
        /// <returns></returns>
        [Column("REALITYMANAGECAPITAL")]
        public decimal? REALITYMANAGECAPITAL { get; set; }
        /// <summary>
        /// 计划治理资金
        /// </summary>
        /// <returns></returns>
        [Column("PLANMANAGECAPITAL")]
        public decimal? PLANMANAGECAPITAL { get; set; }
        /// <summary>
        /// 隐患评估时间
        /// </summary>
        /// <returns></returns>
        [Column("HIDESTIMATETIME")]
        public DateTime? HIDESTIMATETIME { get; set; }
        /// <summary>
        /// 整改截至时间
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEDEADINE")]
        public DateTime? CHANGEDEADINE { get; set; }
        /// <summary>
        /// 整改责任人电话
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEDUTYTEL")]
        public string CHANGEDUTYTEL { get; set; }
        /// <summary>
        /// 整改责任人姓名
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEPERSONNAME")]
        public string CHANGEPERSONNAME { get; set; }
        /// <summary>
        /// 整改责任人
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEPERSON")]
        public string CHANGEPERSON { get; set; }
        /// <summary>
        /// 整改责任单位
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEDUTYDEPARTID")]
        public string CHANGEDUTYDEPARTID { get; set; }
        /// <summary>
        /// 整改责任单位名称
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEDUTYDEPARTNAME")]
        public string CHANGEDUTYDEPARTNAME { get; set; }
        /// <summary>
        /// 整改责任单位
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEDUTYDEPARTCODE")]
        public string CHANGEDUTYDEPARTCODE { get; set; }
        /// <summary>
        /// 隐患编号
        /// </summary>
        /// <returns></returns>
        [Column("HIDCODE")]
        public string HIDCODE { get; set; }
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
        /// 自动增量
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int AUTOID { get; set; }


        /// <summary>
        /// 应用标记(web or  android  or ios)
        /// </summary>
        [Column("APPSIGN")]
        public string APPSIGN { get; set; }

        /// <summary>
        ///整改责任负责人
        /// </summary>
        [Column("CHARGEPERSON")]
        public string CHARGEPERSON { get; set; }

        /// <summary>
        /// 整改责任负责人姓名
        /// </summary>
        [Column("CHARGEPERSONNAME")]
        public string CHARGEPERSONNAME { get; set; }

        /// <summary>
        /// 整改责任负责人单位编码
        /// </summary>
        [Column("CHARGEDEPTID")]
        public string CHARGEDEPTID { get; set; } 

        /// <summary>
        /// 整改责任负责人单位名称
        /// </summary>
        [Column("CHARGEDEPTNAME")]
        public string CHARGEDEPTNAME { get; set; }

        /// <summary>
        /// 整改责任负责人单位名称
        /// </summary>
        [Column("ISAPPOINT")]
        public string ISAPPOINT { get; set; }
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