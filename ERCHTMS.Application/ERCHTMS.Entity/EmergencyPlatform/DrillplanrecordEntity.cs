using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ��������¼
    /// </summary>
    [Table("MAE_DRILLPLANRECORD")]
    public class DrillplanrecordEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// �����ܽḽ��
        /// </summary>
        /// <returns></returns>
        [Column("YLZJFILES")]
        public string YLZJFILES { get; set; }


        /// <summary>
        /// �����ֳ�ͼƬ
        /// </summary>
        /// <returns></returns>
        [Column("YLXCFILES")]
        public string YLXCFILES { get; set; }
        /// <summary>
        /// ����Ƶ
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
        /// ������ID
        /// </summary>
        [Column("COMPERE")]
        public string Compere { get; set; }

        /// <summary>
        /// ����������
        /// </summary>
        [Column("COMPERENAME")]
        public string CompereName { get; set; }

        /// <summary>
        /// ������Ա
        /// </summary>
        [Column("DRILLPEOPLE")]
        public string DrillPeople { get; set; }

        /// <summary>
        /// ������ԱID
        /// </summary>
        [Column("DRILLPEOPLENAME")]
        public string DrillPeopleName { get; set; }
        /// <summary>
        /// ����״̬
        /// </summary>
        [Column("STATUS")]
        public string Status { get; set; }

        /// <summary>
        /// ����Ŀ��
        /// </summary>
        [Column("DRILLPURPOSE")]
        public string DrillPurpose { get; set; }

        /// <summary>
        /// �龰ģ��
        /// </summary>
        [Column("SCENESIMULATION")]
        public string SceneSimulation { get; set; }

        /// <summary>
        /// ����Ҫ��
        /// </summary>
        [Column("DRILLKEYPOINT")]
        public string DrillKeyPoint { get; set; }

        /// <summary>
        /// �����������ID
        /// </summary>
        [Column("DRILLSTEPRECORDID")]
        public string DrillStepRecordId { get; set; }

        /// <summary>
        /// �Ƿ���������ƻ�
        /// </summary>
        [Column("ISCONNECTPLAN")]
        public string IsConnectPlan { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [Column("SELFSCORE")]
        public int? SelfScore { get; set; }

        /// <summary>
        /// �ϼ�����
        /// </summary>
        [Column("TOPSCORE")]
        public int? TopScore { get; set; }

        /// <summary>
        /// �����������
        /// </summary>
        [Column("DRILLIDEA")]
        public string DrillIdea { get; set; }

        /// <summary>
        /// ������ID
        /// </summary>
        [Column("ASSESSPERSON")]
        public string AssessPerson { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [Column("ASSESSPERSONNAME")]
        public string AssessPersonName { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        [Column("ASSESSTIME")]
        public DateTime? AssessTime { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        [Column("WARNTIME")]
        public string WarnTime { get; set; }

        /// <summary>
        /// ������������
        /// </summary>
        [Column("DRILLSCHEMENAME")]
        public string DrillSchemeName { get; set; }
        /// <summary>
        /// ��֯����
        /// </summary>
        [Column("ORGDEPTID")]
        public string OrgDeptId { get; set; }
        /// <summary>
        /// ��֯����
        /// </summary>
        [Column("ORGDEPT")]
        public string OrgDept { get; set; }
        /// <summary>
        /// ��֯����CODE
        /// </summary>
        [Column("ORGDEPTCODE")]
        public string OrgDeptCode { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        [Column("DRILLLEVEL")]
        public string DrillLevel { get; set; }
        /// <summary>
        /// �Ƿ������������� 1 �� 0 ��
        /// </summary>
        [Column("ISSTARTCONFIG")]
        public int? IsStartConfig { get; set; }
        /// <summary>
        /// ���̽ڵ�����
        /// </summary>
        [Column("NODENAME")]
        public string NodeName { get; set; }
        /// <summary>
        /// ����Id
        /// </summary>
        [Column("NODEID")]
        public string NodeId { get; set; }
        /// <summary>
        /// ���۽�ɫ
        /// </summary>
        [Column("EVALUATEROLE")]
        public string EvaluateRole { get; set; }
        /// <summary>
        /// ���۽�ɫ
        /// </summary>
        [Column("EVALUATEROLEID")]
        public string EvaluateRoleId { get; set; }

        /// <summary>
        /// ���۲���
        /// </summary>
        [Column("EVALUATEDEPT")]
        public string EvaluateDept { get; set; }
        /// <summary>
        /// ���۲���
        /// </summary>
        [Column("EVALUATEDEPTID")]
        public string EvaluateDeptId { get; set; }
        /// <summary>
        /// ���۲���
        /// </summary>
        [Column("EVALUATEDEPTCODE")]
        public string EvaluateDeptCode { get; set; }
        /// <summary>
        /// �Ƿ����۽��� 1 ���� 0������
        /// </summary>
        [Column("ISOVEREVALUATE")]
        public int? IsOverEvaluate { get; set; }
        /// <summary>
        /// �Ƿ��ύ 1 �ύ 0δ�ύ
        /// </summary>
        [Column("ISCOMMIT")]
        public int? IsCommit { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        public string ASSESSDATA { get; set; }
        /// <summary>
        /// �Ƿ�������� 1 �� 0 ��
        /// </summary>
        [Column("ISASSESSRECORD")]
        public string IsAssessRecord { get; set; }

        /// <summary>
        /// �����ƻ�����
        /// </summary>
        [Column("PLANDRILLCOST")]
        public string PlanDrillCost { get; set; }

        /// <summary>
        /// ����ʵ�ʷ���
        /// </summary>
        [Column("DRILLCOST")]
        public string DrillCost { get; set; }
        #endregion
        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = DateTime.Now;
            if (string.IsNullOrEmpty(this.CREATEUSERID)) { this.CREATEUSERID = OperatorProvider.Provider.Current().UserId; }
            if (string.IsNullOrEmpty(this.CREATEUSERNAME)) { this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName; }
            if (string.IsNullOrEmpty(this.CREATEUSERDEPTCODE)) { this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode; }
            if (string.IsNullOrEmpty(this.CREATEUSERORGCODE)) { this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode; }
        }
        /// <summary>
        /// �༭����
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