using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// �� ����ʹ������ˮ
    /// </summary>
    [Table("BIS_FIREWATER")]
    public class FireWaterEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ���뵥λ
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTCODE")]
        public string ApplyDeptCode { get; set; }
        /// <summary>
        /// ���뵥λ
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTNAME")]
        public string ApplyDeptName { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERID")]
        public string ApplyUserId { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERNAME")]
        public string ApplyUserName { get; set; }
        /// <summary>
        /// ʹ������ˮ��λ���(0:�糧�ڲ� 1:�����λ)
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTTYPE")]
        public string WorkDeptType { get; set; }
        /// <summary>
        /// ʹ������ˮ��λ
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTID")]
        public string WorkDeptId { get; set; }
        /// <summary>
        /// ʹ������ˮ��λ
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTCODE")]
        public string WorkDeptCode { get; set; }
        /// <summary>
        /// ʹ������ˮ��λ
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTNAME")]
        public string WorkDeptName { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGID")]
        public string EngineeringId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGNAME")]
        public string EngineeringName { get; set; }
        /// <summary>
        /// ʹ������ˮ��ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("WORKSTARTTIME")]
        public DateTime? WorkStartTime { get; set; }
        /// <summary>
        /// ʹ������ˮ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("WORKENDTIME")]
        public DateTime? WorkEndTime { get; set; }
        /// <summary>
        /// ʹ������ˮ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREACODE")]
        public string WorkAreaCode { get; set; }
        /// <summary>
        /// ʹ������ˮ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREANAME")]
        public string WorkAreaName { get; set; }
        /// <summary>
        /// ʹ������ˮ�ص�
        /// </summary>
        /// <returns></returns>
        [Column("WORKPLACE")]
        public string WorkPlace { get; set; }
        /// <summary>
        /// ʹ������ˮ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKCONTENT")]
        public string WorkContent { get; set; }
        /// <summary>
        /// ����״̬(0.������,1.��ˣ�������,2.��ˣ�����δͨ��,3.��ˣ�����ͨ����
        /// </summary>
        /// <returns></returns>
        [Column("APPLYSTATE")]
        public string ApplyState { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYNUMBER")]
        public string ApplyNumber { get; set; }
        /// <summary>
        /// ���̽ڵ㲿��
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPTNAME")]
        public string FlowDeptName { get; set; }
        /// <summary>
        /// ���̽ڵ㲿��id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPT")]
        public string FlowDept { get; set; }
        /// <summary>
        /// ���̽ڵ��ɫ
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLENAME")]
        public string FlowRoleName { get; set; }
        /// <summary>
        /// ���̽ڵ��ɫid
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLE")]
        public string FlowRole { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("FLOWNAME")]
        public string FlowName { get; set; }
        /// <summary>
        /// ����id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }
        /// <summary>
        /// �����0������ 2����� 3����ɣ�
        /// </summary>
        /// <returns></returns>
        [Column("INVESTIGATESTATE")]
        public string InvestigateState { get; set; }
        /// <summary>
        /// ��ҵ��Ա
        /// </summary>
        /// <returns></returns>
        [Column("WORKUSERIDS")]
        public string WorkUserIds { get; set; }
        /// <summary>
        /// ��ҵ��Ա
        /// </summary>
        /// <returns></returns>
        [Column("WORKUSERNAMES")]
        public string WorkUserNames { get; set; }
        /// <summary>
        /// ʹ������ˮʵ�ʿ�ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("REALITYWORKSTARTTIME")]
        public DateTime? RealityWorkStartTime { get; set; }
        /// <summary>
        /// ʹ������ˮʵ�ʽ���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("REALITYWORKENDTIME")]
        public DateTime? RealityWorkEndTime { get; set; }
        /// <summary>
        /// רҵ���
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
        /// ʹ����Ӧ��ȡ��ʩ
        /// </summary>
        /// <returns></returns>
        [Column("MEASURE")]
        public string Measure { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// �����û�id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �޸��û�id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// �����û��������ű���
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// �����û�������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// ���뵥λ
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTID")]
        public string ApplyDeptId { get; set; }
        /// <summary>
        /// ִ��״̬ 0 δִ�� 1 ��ִ��
        /// </summary>
        [Column("CONDITIONSTATE")]
        public string ConditionState { get; set; }

        /// <summary>
        /// ������;
        /// </summary>
        [Column("WORKUSE")]
        public string WorkUse { get; set; }

        /// <summary>
        /// ��ҵ״̬ 0: ������ҵ  1:��ͣ��ҵ
        /// </summary>
        [Column("WORKOPERATE")]
        public string WorkOperate { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Column("TOOL")]
        public string Tool { get; set; }

        /// <summary>
        /// �ֶ�ѡ�����������
        /// </summary>
        [Column("HDTOOL")]
        public string hdTool { get; set; }

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