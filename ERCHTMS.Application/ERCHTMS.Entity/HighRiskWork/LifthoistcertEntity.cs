using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：起吊证
    /// </summary>
    [Table("BIS_LIFTHOISTCERT")]
    public class LifthoistcertEntity : BaseEntity
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
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 作业开始时间
        /// </summary>
        /// <returns></returns>
        [Column("WORKSTARTDATE")]
        public DateTime? WORKSTARTDATE { get; set; }
        /// <summary>
        /// 申请单位CODE
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCOMPANYCODE")]
        public string APPLYCOMPANYCODE { get; set; }

        /// <summary>
        /// 申请单位名称
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCOMPANYNAME")]
        public string APPLYCOMPANYNAME { get; set; }
        /// <summary>
        /// 申请单位
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCOMPANYID")]
        public string APPLYCOMPANYID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPTNAME")]
        public string FLOWDEPTNAME { get; set; }
        /// <summary>
        /// 申请人名称
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERNAME")]
        public string APPLYUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODITYUSERNAME")]
        public string MODITYUSERNAME { get; set; }
        /// <summary>
        /// 吊装施工地点
        /// </summary>
        /// <returns></returns>
        [Column("CONSTRUCTIONADDRESS")]
        public string CONSTRUCTIONADDRESS { get; set; }
        /// <summary>
        /// 吊装工具名称
        /// </summary>
        /// <returns></returns>
        [Column("TOOLNAME")]
        public string TOOLNAME { get; set; }

        /// <summary>
        /// 吊装施工单位
        /// </summary>
        [Column("CONSTRUCTIONUNITID")]
        public string CONSTRUCTIONUNITID { get; set; }
        /// <summary>
        /// 吊装施工单位CODE
        /// </summary>
        [Column("CONSTRUCTIONUNITCODE")]
        public string CONSTRUCTIONUNITCODE { get; set; }
        /// <summary>
        /// 吊装施工单位名称
        /// </summary>
        [Column("CONSTRUCTIONUNITNAME")]
        public string CONSTRUCTIONUNITNAME { get; set; }


        /// <summary>
        /// 司机名称
        /// </summary>
        /// <returns></returns>
        [Column("DRIVERNAME")]
        public string DRIVERNAME { get; set; }

        /// <summary>
        /// 司机证号
        /// </summary>
        /// <returns></returns>
        [Column("DRIVERNUMBER")]
        public string DRIVERNUMBER { get; set; }

        /// <summary>
        /// 专职人员名称
        /// </summary>
        /// <returns></returns>
        [Column("FULLTIMENAME")]
        public string FULLTIMENAME { get; set; }

        /// <summary>
        /// 专职人员证号
        /// </summary>
        /// <returns></returns>
        [Column("FULLTIMENUMBER")]
        public string FULLTIMENUMBER { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FLOWID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDATE")]
        public DateTime? APPLYDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWNAME")]
        public string FLOWNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERID")]
        public string APPLYUSERID { get; set; }
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
        [Column("FLOWROLENAME")]
        public string FLOWROLENAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 作业结束时间
        /// </summary>
        /// <returns></returns>
        [Column("WORKENDDATE")]
        public DateTime? WORKENDDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPTID")]
        public string FLOWDEPTID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 审核状态:
        ///0-申请
        ///1-提交 审核中
        ///2-结束
        /// </summary>
        /// <returns></returns>
        [Column("AUDITSTATE")]
        public int? AUDITSTATE { get; set; }

        /// <summary>
        /// 申请编码组合
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCODESTR")]
        public string APPLYCODESTR { get; set; }
        /// <summary>
        /// 申请编号
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCODE")]
        public string APPLYCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLEID")]
        public string FLOWROLEID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODITYUSERID")]
        public string MODITYUSERID { get; set; }

        /// <summary>
        /// 起重吊装作业ID
        /// </summary>
        /// <returns></returns>
        [Column("LIFTHOISTJOBID")]
        public string LIFTHOISTJOBID { get; set; }

        /// <summary>
        /// 工作负责人
        /// </summary>
        [Column("CHARGEPERSONNAME")]
        public string CHARGEPERSONNAME { get; set; }
        /// <summary>
        /// 工作负责人ID
        /// </summary>
        [Column("CHARGEPERSONID")]
        public string CHARGEPERSONID { get; set; }
        /// <summary>
        /// 工作负责人签字
        /// </summary>
        [Column("CHARGEPERSONSIGN")]
        public string CHARGEPERSONSIGN { get; set; }
        /// <summary>
        /// 吊装区域人员
        /// </summary>
        [Column("HOISTAREAPERSONNAMES")]
        public string HOISTAREAPERSONNAMES { get; set; }
        /// <summary>
        /// 吊装区域人员ID
        /// </summary>
        [Column("HOISTAREAPERSONIDS")]
        public string HOISTAREAPERSONIDS { get; set; }
        /// <summary>
        /// 吊装区域人员签字
        /// </summary>
        [Column("HOISTAREAPERSONSIGNS")]
        public string HOISTAREAPERSONSIGNS { get; set; }

        /// <summary>
        /// 起吊重物质描述
        /// </summary>
        [Column("QUALITYTYPENAME")]
        public string QUALITYTYPENAME { get; set; }


        /// <summary>
        /// 作业单位类型
        /// </summary>
        [Column("WORKDEPTTYPE")]
        public string WORKDEPTTYPE { get; set; }

        /// <summary>
        /// 工程名称
        /// </summary>
        [Column("ENGINEERINGNAME")]
        public string ENGINEERINGNAME { get; set; }

        /// <summary>
        /// 工程
        /// </summary>
        [Column("ENGINEERINGID")]
        public string ENGINEERINGID { get; set; }

        /// <summary>
        /// 专业类别
        /// </summary>
        [Column("SPECIALTYTYPE")]
        public string SPECIALTYTYPE { get; set; }



        [NotMapped]
        public List<LifthoistsafetyEntity> safetys { get; set; }
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
            this.MODITYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODITYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}