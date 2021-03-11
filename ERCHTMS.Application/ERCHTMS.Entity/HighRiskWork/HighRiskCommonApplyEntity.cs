using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// �� �����߷���ͨ����ҵ����
    /// </summary>
    [Table("BIS_HIGHRISKCOMMONAPPLY")]
    public class HighRiskCommonApplyEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// ����״̬(0.������,1.ȷ����,2.ȷ��δͨ��,3.��ˣ�������,4.��ˣ�����δͨ��,5.��ˣ�����ͨ����
        /// </summary>
        /// <returns></returns>
        [Column("APPLYSTATE")]
        public string ApplyState { get; set; }
        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// ��ҵ��Ա
        /// </summary>
        /// <returns></returns>
        [Column("WORKUSERIDS")]
        public string WorkUserIds { get; set; }
        /// <summary>
        /// ���뵥λ
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTID")]
        public string ApplyDeptId { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREACODE")]
        public string WorkAreaCode { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERID")]
        public string ApplyUserId { get; set; }
        /// <summary>
        /// ��ҵ������
        /// </summary>
        /// <returns></returns>
        [Column("WORKDUTYUSERNAME")]
        public string WorkDutyUserName { get; set; }
        /// <summary>
        /// ��ҵ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("WORKENDTIME")]
        public DateTime? WorkEndTime { get; set; }
        /// <summary>
        /// ��ҵ��λ���(0:��λ�ڲ� 1:�����λ)
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTTYPE")]
        public string WorkDeptType { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGNAME")]
        public string EngineeringName { get; set; }
        /// <summary>
        /// ���ձ�ʶ
        /// </summary>
        /// <returns></returns>
        [Column("RISKIDENTIFICATION")]
        public string RiskIdentification { get; set; }
        /// <summary>
        /// ���뵥λ
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTNAME")]
        public string ApplyDeptName { get; set; }
        /// <summary>
        /// �໤��
        /// </summary>
        /// <returns></returns>
        [Column("WORKTUTELAGEUSERID")]
        public string WorkTutelageUserId { get; set; }
        /// <summary>
        /// ��ҵ�ص�
        /// </summary>
        /// <returns></returns>
        [Column("WORKPLACE")]
        public string WorkPlace { get; set; }
        /// <summary>
        /// ��ҵ��λ
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTID")]
        public string WorkDeptId { get; set; }
        /// <summary>
        /// ��ҵ��λ
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTNAME")]
        public string WorkDeptName { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKTYPE")]
        public string WorkType { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGID")]
        public string EngineeringId { get; set; }
        /// <summary>
        /// ��ҵ��Ա
        /// </summary>
        /// <returns></returns>
        [Column("WORKUSERNAMES")]
        public string WorkUserNames { get; set; }
        /// <summary>
        /// ��ҵ��λ
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTCODE")]
        public string WorkDeptCode { get; set; }
        /// <summary>
        /// �����û��������ű���
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// ��ҵ��ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("WORKSTARTTIME")]
        public DateTime? WorkStartTime { get; set; }
        /// <summary>
        /// ��ҵ������
        /// </summary>
        /// <returns></returns>
        [Column("WORKDUTYUSERID")]
        public string WorkDutyUserId { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREANAME")]
        public string WorkAreaName { get; set; }
        /// <summary>
        /// �����û�id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �໤��
        /// </summary>
        /// <returns></returns>
        [Column("WORKTUTELAGEUSERNAME")]
        public string WorkTutelageUserName { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERNAME")]
        public string ApplyUserName { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYNUMBER")]
        public string ApplyNumber { get; set; }
        /// <summary>
        /// ���뵥λ
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTCODE")]
        public string ApplyDeptCode { get; set; }
        /// <summary>
        /// �����û�������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKCONTENT")]
        public string WorkContent { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �޸��û�id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }

        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("FLOWNAME")]
        public string FlowName { get; set; }

        /// <summary>
        /// ���̽�ɫ����/ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLE")]
        public string FlowRole { get; set; }

        /// <summary>
        /// ���̽�ɫ����
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLENAME")]
        public string FlowRoleName { get; set; }

        /// <summary>
        /// ���̲��ű���/ID 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPT")]
        public string FlowDept { get; set; }

        /// <summary>
        /// ���̲�������
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPTNAME")]
        public string FlowDeptName { get; set; }

        /// <summary>
        /// ȷ�Ͻ��
        /// </summary>
        /// <returns></returns>
        [Column("INVESTIGATESTATE")]
        public string InvestigateState { get; set; }


        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <returns></returns>
        public string DeleteFileIds { get; set; }

        /// <summary>
        /// ʵ����ҵ��ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("REALITYWORKSTARTTIME")]
        public DateTime? RealityWorkStartTime { get; set; }

        /// <summary>
        /// ʵ����ҵ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("REALITYWORKENDTIME")]
        public DateTime? RealityWorkEndTime { get; set; }

        /// <summary>
        /// ���յȼ�
        /// </summary>
        /// <returns></returns>
        [Column("RISKTYPE")]
        public string RiskType { get; set; }

        /// <summary>
        /// רҵ����
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYTYPE")]
        public string SpecialtyType { get; set; }

        /// <summary>
        /// ��˱�ע
        /// </summary>
        /// <returns></returns>
        [Column("FLOWREMARK")]
        public string FlowRemark { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("COPYUSERIDS")]
        public string CopyUserIds { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("COPYUSERNAMES")]
        public string CopyUserNames { get; set; }

        /// <summary>
        /// ����֪ͨ������(0:�� 1:��)
        /// </summary>
        /// <returns></returns>
        [Column("ISMESSAGE")]
        public string IsMessage { get; set; }

        /// <summary>
        /// �ǹ���ʱ������
        /// </summary>
        [Column("NONWORKINGAPPROVE")]
        public string NonWorkingApprove { get; set; }

        /// <summary>
        /// ֵ�ಿ��
        /// </summary>
        [Column("APPROVEDEPT")]
        public string ApproveDept { get; set; }

        /// <summary>
        /// ֵ�ಿ��ID
        /// </summary>
        [Column("APPROVEDEPTID")]
        public string ApproveDeptId { get; set; }

        /// <summary>
        /// ֵ�ಿ��Code
        /// </summary>
        [Column("APPROVEDEPTCODE")]
        public string ApproveDeptCode { get; set; }

        /// <summary>
        /// ���������
        /// </summary>
        [Column("WORKLICENSORNAME")]
        public string WorkLicensorName { get; set; }

        /// <summary>
        /// ���������ID
        /// </summary>
        [Column("WORKLICENSORID")]
        public string WorkLicensorId { get; set; }

        /// <summary>
        /// ����������˺�
        /// </summary>
        [Column("WORKLICENSORACCOUNT")]
        public string WorkLicensorAccount { get; set; }

        /// <summary>
        /// ָ����һ��������˺�
        /// </summary>
        [Column("NEXTSTEPAPPROVEUSERACCOUNT")]
        public string NextStepApproveUserAccount { get; set; }

        /// <summary>
        /// �����������˺�
        /// </summary>
        [Column("WORKDUTYUSERACCOUNT")]
        public string WorkDutyUserAccount { get; set; }

        /// <summary>
        /// ������˺�
        /// </summary>
        [Column("APPROVEACCOUNT")]
        public string ApproveAccount { get; set; }

        /// <summary>
        /// ���̻�ȡ����˷�ʽ
        /// </summary>
        [Column("FLOWAPPLYTYPE")]
        public string FlowApplyType { get; set; }

        /// <summary>
        /// ��ҵ״̬ 0: ������ҵ  1:��ͣ��ҵ
        /// </summary>
        [Column("WORKOPERATE")]
        public string WorkOperate { get; set; }

        /// <summary>
        /// ��Դ�����
        /// </summary>
        [Column("POWERACCESS")]
        public string PowerAccess { get; set; }

        /// <summary>
        /// ��ѹ
        /// </summary>
        [Column("VOLTAGE")]
        public string Voltage { get; set; }

        /// <summary>
        /// �豸�ܵ�����
        /// </summary>
        [Column("PIPELINE")]
        public string PipeLine { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Column("MEDIA")]
        public string Media { get; set; }

        /// <summary>
        /// ѹ��
        /// </summary>
        [Column("PRESSURE")]
        public string Pressure { get; set; }

        /// <summary>
        /// װä�帺����Id
        /// </summary>
        [Column("ZMBDUTYUSERID")]
        public string ZMBDutyUserId { get; set; }

        /// <summary>
        /// װä�帺����
        /// </summary>
        [Column("ZMBDUTYUSERNAME")]
        public string ZMBDutyUserName { get; set; }

        /// <summary>
        /// ��ä�帺����Id
        /// </summary>
        [Column("CMBDUTYUSERID")]
        public string CMBDutyUserId { get; set; }

        /// <summary>
        /// ��ä�帺����
        /// </summary>
        [Column("CMBDUTYUSERNAME")]
        public string CMBDutyUserName { get; set; }

        /// <summary>
        /// �¶�
        /// </summary>
        [Column("TEMPERATURE")]
        public string Temperature { get; set; }

        /// <summary>
        /// ��Ӧ����Ʊ��ż�����
        /// </summary>
        [Column("WORKTICKETNOCONTENT")]
        public string WorkTicketNoContent { get; set; }

        /// <summary>
        /// ����Σ�շ���(JHA)(0:�� 1:��)
        /// </summary>
        [Column("DANGERANALYSE")]
        public int? DangerAnalyse { get; set; }

        /// <summary>
        /// ��ҵ��ȫ����(JSA)(0:�� 1:��)
        /// </summary>
        [Column("SAFETYANALYSE")]
        public int? SafetyAnalyse { get; set; }

        /// <summary>
        /// ���������id
        /// </summary>
        [Column("YXPERMITUSERID")]
        public string YXPermitUserId { get; set; }

        /// <summary>
        /// ���������
        /// </summary>
        [Column("YXPERMITUSERNAME")]
        public string YXPermitUserName { get; set; }

        /// <summary>
        /// ֵ��/ֵ�ฺ����id
        /// </summary>
        [Column("WATCHUSERID")]
        public string WatchUserId { get; set; }

        /// <summary>
        /// ֵ��/ֵ�ฺ����
        /// </summary>
        [Column("WATCHUSERNAME")]
        public string WatchUserName { get; set; }

        /// <summary>
        /// ��Ӱ����ط�ȷ����id
        /// </summary>
        [Column("EFFECTCONFIMERID")]
        public string EffectConfimerId { get; set; }

        /// <summary>
        /// ��Ӱ����ط�ȷ����
        /// </summary>
        [Column("EFFECTCONFIMERNAME")]
        public string EffectConfirmerName { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}