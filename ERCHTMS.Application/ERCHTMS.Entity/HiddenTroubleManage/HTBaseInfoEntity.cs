using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患基本信息表
    /// </summary>
    [Table("BIS_HTBASEINFO")]
    public class HTBaseInfoEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 隐患编码
        /// </summary>
        /// <returns></returns>
        [Column("HIDCODE")]
        public string HIDCODE { get; set; }
        /// <summary>
        /// 临时流程用户组
        /// </summary>
        /// <returns></returns>
        [Column("PARTICIPANT")]
        public string PARTICIPANT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 隐患地点
        /// </summary>
        /// <returns></returns>
        [Column("HIDPLACE")]
        public string HIDPLACE { get; set; }
        /// <summary>
        /// 风险点名称
        /// </summary>
        /// <returns></returns>
        [Column("RISKNAME")]
        public string RISKNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 隐患相片
        /// </summary>
        /// <returns></returns>
        [Column("HIDPHOTO")]
        public string HIDPHOTO { get; set; }
        /// <summary>
        /// 选择单位
        /// </summary>
        /// <returns></returns>
        [Column("CHOOSEDEPART")]
        public string CHOOSEDEPART { get; set; }
        /// <summary>
        /// 隐患区域
        /// </summary>
        /// <returns></returns>
        [Column("HIDPOINT")]
        public string HIDPOINT { get; set; }
        /// <summary>
        /// 隐患区域名称
        /// </summary>
        /// <returns></returns>
        [Column("HIDPOINTNAME")]
        public string HIDPOINTNAME { get; set; }
        /// <summary>
        /// 所属工程
        /// </summary>
        /// <returns></returns>
        [Column("HIDPROJECT")]
        public string HIDPROJECT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMAN")]
        public string CHECKMAN { get; set; }
        /// <summary>
        /// 曝光时间
        /// </summary>
        /// <returns></returns>
        [Column("EXPOSUREDATETIME")]
        public DateTime? EXPOSUREDATETIME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 排查类型
        /// </summary>
        /// <returns></returns>
        [Column("CHECKTYPE")]
        public string CHECKTYPE { get; set; }
        /// <summary>
        /// 隐患描述
        /// </summary>
        /// <returns></returns>
        [Column("HIDDESCRIBE")]
        public string HIDDESCRIBE { get; set; }
        /// <summary>
        /// 排查单位
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPART")]
        public string CHECKDEPART { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDEPTCODE")]
        public string CREATEDEPTCODE { get; set; }
        /// <summary>
        /// 隐患类别
        /// </summary>
        /// <returns></returns>
        [Column("HIDTYPE")]
        public string HIDTYPE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 培训模板名称
        /// </summary>
        /// <returns></returns>
        [Column("TRAINTEMPLATENAME")]
        public string TRAINTEMPLATENAME { get; set; }
        /// <summary>
        /// 隐患级别
        /// </summary>
        /// <returns></returns>
        [Column("HIDRANK")]
        public string HIDRANK { get; set; }
        /// <summary>
        /// 整改状态
        /// </summary>
        /// <returns></returns>
        [Column("STATES")]
        public string STATES { get; set; }

        /// <summary>
        /// 隐患标准
        /// </summary>
        /// <returns></returns>
        [Column("HIDDANGERNORM")]
        public string HIDDANGERNORM { get; set; }
        /// <summary>
        /// 违章人员姓名
        /// </summary>
        /// <returns></returns>
        [Column("BREAKRULEUSERNAMES")]
        public string BREAKRULEUSERNAMES { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPARTCODE")]
        public string CHECKDEPARTCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 隐患报告摘要
        /// </summary>
        /// <returns></returns>
        [Column("REPORTDIGEST")]
        public string REPORTDIGEST { get; set; }
        /// <summary>
        /// 是否违章
        /// </summary>
        /// <returns></returns>
        [Column("ISBREAKRULE")]
        public string ISBREAKRULE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CHOOSEDEPARTNAME")]
        public string CHOOSEDEPARTNAME { get; set; }
        /// <summary>
        /// 隐患名称
        /// </summary>
        /// <returns></returns>
        [Column("HIDDANGERNAME")]
        public string HIDDANGERNAME { get; set; }
        /// <summary>
        /// 培训模板ID
        /// </summary>
        /// <returns></returns>
        [Column("TRAINTEMPLATEID")]
        public string TRAINTEMPLATEID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 隐患所属部门主键
        /// </summary>
        /// <returns></returns>
        [Column("HIDDEPART")]
        public string HIDDEPART { get; set; }
        /// <summary>
        /// 工作流节点
        /// </summary>
        /// <returns></returns>
        [Column("WORKSTREAM")]
        public string WORKSTREAM { get; set; }
        /// <summary>
        /// 排查日期
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATE")]
        public DateTime? CHECKDATE { get; set; }
        /// <summary>
        /// 隐患排查批号
        /// </summary>
        /// <returns></returns>
        [Column("CHECKNUMBER")]
        public string CHECKNUMBER { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 15天未进行隐患排查倍数
        /// </summary>
        /// <returns></returns>
        [Column("OVERMULTIPLE")]
        public int? OVERMULTIPLE { get; set; }
        /// <summary>
        /// 隐患所属部门名称
        /// </summary>
        /// <returns></returns>
        [Column("HIDDEPARTNAME")]
        public string HIDDEPARTNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPARTID")]
        public string CHECKDEPARTID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// 违章人员ID
        /// </summary>
        /// <returns></returns>
        [Column("BREAKRULEUSERIDS")]
        public string BREAKRULEUSERIDS { get; set; }
        /// <summary>
        /// 隐患发现日期
        /// </summary>
        /// <returns></returns>
        [Column("FINDDATE")]
        public DateTime? FINDDATE { get; set; }
        /// <summary>
        /// 登记类型 已整改隐患/未整改隐患
        /// </summary>
        /// <returns></returns>
        [Column("ADDTYPE")]
        public string ADDTYPE { get; set; }
        /// <summary>
        /// 曝光状态
        /// </summary>
        /// <returns></returns>
        [Column("EXPOSURESTATE")]
        public string EXPOSURESTATE { get; set; }
        /// <summary>
        /// 安全检查项ID
        /// </summary>
        [Column("SAFETYCHECKOBJECTID")]
        public string SAFETYCHECKOBJECTID { get; set; }
        /// <summary>
        /// 安全检查类型
        /// </summary>
        [Column("SAFETYCHECKTYPE")]
        public string SAFETYCHECKTYPE { get; set; }

        /// <summary>
        /// 隐患产生的原因
        /// </summary>
        [Column("HIDREASON")]
        public string HIDREASON { get; set; }

        /// <summary>
        /// 隐患危害的程度
        /// </summary>
        [Column("HIDDANGERLEVEL")]
        public string HIDDANGERLEVEL { get; set; }

        /// <summary>
        /// 防控措施
        /// </summary>
        [Column("PREVENTMEASURE")]
        public string PREVENTMEASURE { get; set; }

        /// <summary>
        /// 隐患整改计划
        /// </summary>
        [Column("HIDCHAGEPLAN")]
        public string HIDCHAGEPLAN { get; set; }

        /// <summary>
        /// 应急预案简述
        /// </summary>
        [Column("EXIGENCERESUME")]
        public string EXIGENCERESUME { get; set; }

        /// <summary>
        /// 是否挂牌督办
        /// </summary>
        [Column("ISGETAFTER")]
        public string ISGETAFTER { get; set; }

        /// <summary>
        /// 所属工程名称
        /// </summary>
        [Column("HIDPROJECTNAME")]
        public string HIDPROJECTNAME { get; set; }

        /// <summary>
        /// 排查人姓名
        /// </summary>
        [Column("CHECKMANNAME")]
        public string CHECKMANNAME { get; set; }
        /// <summary>
        /// 排查单位名称
        /// </summary>
        [Column("CHECKDEPARTNAME")]
        public string CHECKDEPARTNAME { get; set; }
        /// <summary>
        /// 设备设施Id
        /// </summary>
        [Column("DEVICEID")]
        public string DEVICEID { get; set; }
        /// <summary>
        /// 设备设施名称
        /// </summary>
        [Column("DEVICENAME")]
        public string DEVICENAME { get; set; }
        /// <summary>
        /// 设备设施编码
        /// </summary>
        [Column("DEVICECODE")]
        public string DEVICECODE { get; set; }
        /// <summary>
        /// 监控人员ID
        /// </summary>
        [Column("MONITORPERSONID")]
        public string MONITORPERSONID { get; set; }
        /// <summary>
        /// 监控人员名称
        /// </summary>
        [Column("MONITORPERSONNAME")]
        public string MONITORPERSONNAME { get; set; }  
        /// <summary>
        /// 关联Id
        /// </summary>
        [Column("RELEVANCEID")]
        public string RELEVANCEID { get; set; }
        /// <summary>
        /// 关联类型
        /// </summary>
        [Column("RELEVANCETYPE")]
        public string RELEVANCETYPE { get; set; }

        /// <summary>
        /// 专业分类
        /// </summary>
        [Column("MAJORCLASSIFY")]
        public string MAJORCLASSIFY { get; set; }
        /// <summary>
        /// 隐患名称
        /// </summary>
        [Column("HIDNAME")]
        public string HIDNAME { get; set; } 
        /// <summary>
        /// 隐患现状
        /// </summary>
        [Column("HIDSTATUS")]
        public string HIDSTATUS { get; set; }
        /// <summary>
        /// 可能导致的后果
        /// </summary>
        [Column("HIDCONSEQUENCE")] 
        public string HIDCONSEQUENCE { get; set; }

        /// <summary>
        /// 应用标记(web or  android  or ios)
        /// </summary>
        [Column("APPSIGN")]
        public string APPSIGN { get; set; }

        /// <summary>
        /// 应用标记(web or  android  or ios)
        /// </summary>
        [Column("UPSUBMIT")]
        public string UPSUBMIT { get; set; }

        /// <summary>
        /// 是否本部门整改 
        /// </summary>
        [Column("ISSELFCHANGE")]
        public string ISSELFCHANGE { get; set; }

        /// <summary>
        /// 是否制定整改计划 
        /// </summary>
        [Column("ISFORMULATE")]
        public string ISFORMULATE { get; set; }

        /// <summary>
        /// 安全检查名称 
        /// </summary>
        [Column("SAFETYCHECKNAME")]
        public string SAFETYCHECKNAME { get; set; }

        /// <summary>
        /// 所属部门名称 
        /// </summary>
        [Column("HIDBMNAME")]
        public string HIDBMNAME { get; set; }

        /// <summary>
        /// 所属部门id 
        /// </summary>
        [Column("HIDBMID")] 
        public string HIDBMID { get; set; }
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