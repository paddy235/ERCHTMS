using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急演练记录历史记录
    /// </summary>
    [Table("MAE_DRILLPLANRECORDHISTORY")]
    public class DrillplanrecordHistoryEntity:BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 演练总结附件
        /// </summary>
        /// <returns></returns>
        [Column("YLZJFILES")]
        public string YLZJFILES { get; set; }


        /// <summary>
        /// 演练现场图片
        /// </summary>
        /// <returns></returns>
        [Column("YLXCFILES")]
        public string YLXCFILES { get; set; }
        /// <summary>
        /// 、视频
        /// </summary>
        [Column("VIDEOFILES")]
        public string VideoFiles { get; set; }
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
        [Column("NAME")]
        public string NAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DRILLPLANID")]
        public string DRILLPLANID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DRILLPLANNAME")]
        public string DRILLPLANNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTID")]
        public string DEPARTID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTNAME")]
        public string DEPARTNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DRILLTYPE")]
        public string DRILLTYPE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DRILLTYPENAME")]
        public string DRILLTYPENAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DRILLMODENAME")]
        public string DRILLMODENAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DRILLMODE")]
        public string DRILLMODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DRILLTIME")]
        public DateTime? DRILLTIME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DRILLPLACE")]
        public string DRILLPLACE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DRILLPEOPLENUMBER")]
        public int? DRILLPEOPLENUMBER { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MAINCONTENT")]
        public string MAINCONTENT { get; set; }
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
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
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
        /// 主持人ID
        /// </summary>
        [Column("COMPERE")]
        public string Compere { get; set; }

        /// <summary>
        /// 主持人姓名
        /// </summary>
        [Column("COMPERENAME")]
        public string CompereName { get; set; }

        /// <summary>
        /// 参演人员
        /// </summary>
        [Column("DRILLPEOPLE")]
        public string DrillPeople { get; set; }

        /// <summary>
        /// 参演人员ID
        /// </summary>
        [Column("DRILLPEOPLENAME")]
        public string DrillPeopleName { get; set; }
        /// <summary>
        /// 演练状态
        /// </summary>
        [Column("STATUS")]
        public string Status { get; set; }

        /// <summary>
        /// 演练目的
        /// </summary>
        [Column("DRILLPURPOSE")]
        public string DrillPurpose { get; set; }

        /// <summary>
        /// 情景模拟
        /// </summary>
        [Column("SCENESIMULATION")]
        public string SceneSimulation { get; set; }

        /// <summary>
        /// 演练要点
        /// </summary>
        [Column("DRILLKEYPOINT")]
        public string DrillKeyPoint { get; set; }

        /// <summary>
        /// 演练步骤关联ID
        /// </summary>
        [Column("DRILLSTEPRECORDID")]
        public string DrillStepRecordId { get; set; }

        /// <summary>
        /// 是否关联演练计划
        /// </summary>
        [Column("ISCONNECTPLAN")]
        public string IsConnectPlan { get; set; }

        /// <summary>
        /// 自评分
        /// </summary>
        [Column("SELFSCORE")]
        public int? SelfScore { get; set; }

        /// <summary>
        /// 上级评分
        /// </summary>
        [Column("TOPSCORE")]
        public int? TopScore { get; set; }

        /// <summary>
        /// 演练评价意见
        /// </summary>
        [Column("DRILLIDEA")]
        public string DrillIdea { get; set; }

        /// <summary>
        /// 评价人ID
        /// </summary>
        [Column("ASSESSPERSON")]
        public string AssessPerson { get; set; }

        /// <summary>
        /// 评价人
        /// </summary>
        [Column("ASSESSPERSONNAME")]
        public string AssessPersonName { get; set; }

        /// <summary>
        /// 评价时间
        /// </summary>
        [Column("ASSESSTIME")]
        public DateTime? AssessTime { get; set; }

        /// <summary>
        /// 提醒时间
        /// </summary>
        [Column("WARNTIME")]
        public string WarnTime { get; set; }

        /// <summary>
        /// 演练方案名称
        /// </summary>
        [Column("DRILLSCHEMENAME")]
        public string DrillSchemeName { get; set; }
        /// <summary>
        /// 组织部门
        /// </summary>
        [Column("ORGDEPTID")]
        public string OrgDeptId { get; set; }
        /// <summary>
        /// 组织部门
        /// </summary>
        [Column("ORGDEPT")]
        public string OrgDept { get; set; }
        /// <summary>
        /// 组织部门CODE
        /// </summary>
        [Column("ORGDEPTCODE")]
        public string OrgDeptCode { get; set; }
        /// <summary>
        /// 演练级别
        /// </summary>
        [Column("DRILLLEVEL")]
        public string DrillLevel { get; set; }
        /// <summary>
        /// 是否配置评价流程 1 是 0 否
        /// </summary>
        [Column("ISSTARTCONFIG")]
        public int? IsStartConfig { get; set; }
        /// <summary>
        /// 流程节点名称
        /// </summary>
        [Column("NODENAME")]
        public string NodeName { get; set; }
        /// <summary>
        /// 流程Id
        /// </summary>
        [Column("NODEID")]
        public string NodeId { get; set; }
        /// <summary>
        /// 评价角色
        /// </summary>
        [Column("EVALUATEROLE")]
        public string EvaluateRole { get; set; }
        /// <summary>
        /// 评价角色
        /// </summary>
        [Column("EVALUATEROLEID")]
        public string EvaluateRoleId { get; set; }

        /// <summary>
        /// 评价部门
        /// </summary>
        [Column("EVALUATEDEPT")]
        public string EvaluateDept { get; set; }
        /// <summary>
        /// 评价部门
        /// </summary>
        [Column("EVALUATEDEPTID")]
        public string EvaluateDeptId { get; set; }
        /// <summary>
        /// 评价部门
        /// </summary>
        [Column("EVALUATEDEPTCODE")]
        public string EvaluateDeptCode { get; set; }
        /// <summary>
        /// 是否评价结束 1 结束 0评价中
        /// </summary>
        [Column("ISOVEREVALUATE")]
        public int? IsOverEvaluate { get; set; }
        /// <summary>
        /// 是否提交 1 提交 0未提交
        /// </summary>
        [Column("ISCOMMIT")]
        public int? IsCommit { get; set; }
        /// <summary>
        /// 评估表数据
        /// </summary>
        public string ASSESSDATA { get; set; }
        /// <summary>
        /// 是否进行评估 1 是 0 否
        /// </summary>
        [Column("ISASSESSRECORD")]
        public string IsAssessRecord { get; set; }
        /// <summary>
        /// 历史主键Id---关联Id
        /// </summary>
        [Column("HISTORYID")]
        public string HistoryId { get; set; }
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
