using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：开收工会
    /// </summary>
    [Table("BIS_WORKMEETING")]
    public class WorkMeetingEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
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
        /// 工程ID
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERID")]
        public string ENGINEERID { get; set; }
        /// <summary>
        /// 工程名称
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERNAME")]
        public string ENGINEERNAME { get; set; }
        /// <summary>
        /// 工程编码
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERCODE")]
        public string ENGINEERCODE { get; set; }
        /// <summary>
        /// 工程类型
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERTYPE")]
        public string ENGINEERTYPE { get; set; }
        /// <summary>
        /// 工程区域
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERAREA")]
        public string ENGINEERAREA { get; set; }
        /// <summary>
        /// 工程风险级别
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLEVEL")]
        public string ENGINEERLEVEL { get; set; }
        /// <summary>
        /// 发包部门
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETDEPT")]
        public string ENGINEERLETDEPT { get; set; }
        /// <summary>
        /// 工程内容
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERCONTENT")]
        public string ENGINEERCONTENT { get; set; }
        /// <summary>
        /// 会议名称
        /// </summary>
        /// <returns></returns>
        [Column("MEETINGNAME")]
        public string MEETINGNAME { get; set; }
        /// <summary>
        /// 会议日期
        /// </summary>
        /// <returns></returns>
        [Column("MEETINGDATE")]
        public DateTime? MEETINGDATE { get; set; }
        /// <summary>
        /// 会议类型（开工会，收工会）
        /// </summary>
        /// <returns></returns>
        [Column("MEETINGTYPE")]
        public string MEETINGTYPE { get; set; }
        /// <summary>
        /// 会议地点
        /// </summary>
        /// <returns></returns>
        [Column("ADDRESS")]
        public string ADDRESS { get; set; }
        /// <summary>
        /// 会签人员
        /// </summary>
        /// <returns></returns>
        [Column("SIGNPERSONS")]
        public string SIGNPERSONS { get; set; }
        /// <summary>
        /// 应到人数
        /// </summary>
        /// <returns></returns>
        [Column("SHOUDPERNUM")]
        public int? SHOUDPERNUM { get; set; }
        /// <summary>
        /// 实到人数
        /// </summary>
        /// <returns></returns>
        [Column("REALPERNUM")]
        public int? REALPERNUM { get; set; }
        /// <summary>
        /// 是否健康良好
        /// </summary>
        /// <returns></returns>
        [Column("HEALTHSTA")]
        public string HEALTHSTA { get; set; }
        /// <summary>
        /// 是否配置安全劳保用品
        /// </summary>
        /// <returns></returns>
        [Column("SAFEGOODSSTA")]
        public string SAFEGOODSSTA { get; set; }
        /// <summary>
        /// 焊工人数
        /// </summary>
        /// <returns></returns>
        [Column("LNUM")]
        public int? LNUM { get; set; }
        /// <summary>
        /// 电工人数
        /// </summary>
        /// <returns></returns>
        [Column("ENUM")]
        public int? ENUM { get; set; }
        /// <summary>
        /// 起重工人数
        /// </summary>
        /// <returns></returns>
        [Column("GNUM")]
        public int? GNUM { get; set; }
        /// <summary>
        /// 架子工人数
        /// </summary>
        /// <returns></returns>
        [Column("JNUM")]
        public int? JNUM { get; set; }
        /// <summary>
        /// 其他人数
        /// </summary>
        /// <returns></returns>
        [Column("ONUM")]
        public int? ONUM { get; set; }
        /// <summary>
        /// 服装是否符合规定
        /// </summary>
        /// <returns></returns>
        [Column("CLOTHESTA")]
        public string CLOTHESTA { get; set; }
        /// <summary>
        /// 佩带有关证件
        /// </summary>
        /// <returns></returns>
        [Column("CERTSTA")]
        public string CERTSTA { get; set; }
        /// <summary>
        /// 内容1
        /// </summary>
        /// <returns></returns>
        [Column("CONTENT1")]
        public string CONTENT1 { get; set; }
        /// <summary>
        /// 内容2
        /// </summary>
        /// <returns></returns>
        [Column("CONTENT2")]
        public string CONTENT2 { get; set; }
        /// <summary>
        /// 内容3
        /// </summary>
        /// <returns></returns>
        [Column("CONTENT3")]
        public string CONTENT3 { get; set; }
        /// <summary>
        /// 其他内容（收工会）
        /// </summary>
        /// <returns></returns>
        [Column("CONTENTOTHER")]
        public string CONTENTOTHER { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK")]
        public string REMARK { get; set; }
        /// <summary>
        /// 附件列表
        /// </summary>
        public object FILES { get; set; }
        /// <summary>
        /// 删除文件编号（多个用,隔开）
        /// </summary>
        public string DELETEFILEID { get; set; }
        /// <summary>
        /// 外包单位
        /// </summary>
        public String OUTPROJECTNAME { get; set; }
        /// <summary>
        /// 外包单位
        /// </summary>
        public String OUTPROJECTCODE { get; set; }
        /// <summary>
        /// 是否提交
        /// </summary>
        [Column("ISCOMMIT")]
        public String ISCOMMIT { get; set; }
        [Column("ISOVER")]
        public int IsOver { get; set; }

        [Column("STARTMEETINGID")]
        public String StartMeetingid { get; set; }

        [Column("RISKLEVEL")]
        public String RiskLevel { get; set; }
        public List<WorkmeetingmeasuresEntity> MeasuresList { get; set; }
        public string ids { get; set; }
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