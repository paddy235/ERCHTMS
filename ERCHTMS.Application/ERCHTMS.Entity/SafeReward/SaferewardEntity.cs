using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SafeReward
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    [Table("BIS_SAFEREWARD")]
    public class SaferewardEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// רҵ������name
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYPRINCIPALNAME")]
        public string SpecialtyPrincipalName { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// רҵ�������Ƿ�ͬ��
        /// </summary>
        /// <returns></returns>
        [Column("ISAGREEPRINCIPAL")]
        public string IsAgreePrincipal { get; set; }
        /// <summary>
        /// ��������Աid
        /// </summary>
        /// <returns></returns>
        [Column("REWARDUSERID")]
        public string RewardUserId { get; set; }
        /// <summary>
        /// �����û��������ű���
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// ���Ÿ�����name
        /// </summary>
        /// <returns></returns>
        [Column("DEPTPRINCIPALNAME")]
        public string DeptPrincipalName { get; set; }
        /// <summary>
        /// �ֹ��쵼id
        /// </summary>
        /// <returns></returns>
        [Column("LEADERSHIPID")]
        public string LeaderShipId { get; set; }
        /// <summary>
        /// ����״̬(1.רҵ���,2.�������,3.EHS�����,4.�ֹ��쵼,5.�ܾ���)
        /// </summary>
        /// <returns></returns>
        [Column("APPLYSTATE")]
        public string ApplyState { get; set; }
        /// <summary>
        /// �ܾ���id
        /// </summary>
        /// <returns></returns>
        [Column("CEOID")]
        public string CeoId { get; set; }
        /// <summary>
        /// ����״̬(0���ڴ���.,1.�Ѵ���,2.δ����)
        /// </summary>
        /// <returns></returns>
        [Column("FLOWSTATE")]
        public string FlowState { get; set; }
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
        /// �ܾ���name
        /// </summary>
        /// <returns></returns>
        [Column("CEONAME")]
        public string CeoName { get; set; }
        /// <summary>
        /// ��������λ
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
        /// ���Ÿ������Ƿ�ͬ��
        /// </summary>
        /// <returns></returns>
        [Column("ISAGREEDEPT")]
        public string IsAgreeDept { get; set; }
        /// <summary>
        /// �޸��û�id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        /// <returns></returns>
        [Column("SAFEREWARDCODE")]
        public string SafeRewardCode { get; set; }
        /// <summary>
        /// ��������λ
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTID")]
        public string ApplyDeptId { get; set; }
        /// <summary>
        /// ��������Աname
        /// </summary>
        /// <returns></returns>
        [Column("REWARDUSERNAME")]
        public string RewardUserName { get; set; }
        /// <summary>
        /// ��������λ
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTNAME")]
        public string ApplyDeptName { get; set; }
        /// <summary>
        /// �ܾ����Ƿ�ͬ��
        /// </summary>
        /// <returns></returns>
        [Column("ISAGREECEO")]
        public string IsAgreeCeo { get; set; }
        /// <summary>
        /// �ֹ��쵼���
        /// </summary>
        /// <returns></returns>
        [Column("LEADERSHIPOPINION")]
        public string LeaderShipOpinion { get; set; }
        /// <summary>
        /// �ֹ��쵼�Ƿ�ͬ��
        /// </summary>
        /// <returns></returns>
        [Column("ISAGREELEADERSHIPID")]
        public string IsAgreeLeaderShipId { get; set; }
        /// <summary>
        /// ������name
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERNAME")]
        public string ApplyUserName { get; set; }

        /// <summary>
        /// �����˲���id
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERDEPTID")]
        public string ApplyUserDeptId { get; set; }


        /// <summary>
        /// �����˲���name
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERDEPTNAME")]
        public string ApplyUserDeptName { get; set; }
        /// <summary>
        /// ���뽱�����
        /// </summary>
        /// <returns></returns>
        [Column("APPLYREWARDRMB")]
        public string ApplyRewardRmb { get; set; }
        /// <summary>
        /// ���Ÿ�����id
        /// </summary>
        /// <returns></returns>
        [Column("DEPTPRINCIPALID")]
        public string DeptPrincipalId { get; set; }
        /// <summary>
        /// �ܾ������
        /// </summary>
        /// <returns></returns>
        [Column("CEOOPINION")]
        public string CeoOpinion { get; set; }
        /// <summary>
        /// �ֹ��쵼name
        /// </summary>
        /// <returns></returns>
        [Column("LEADERSHIPNAME")]
        public string LeaderShipName { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// EHS���������Ƿ�ͬ��
        /// </summary>
        /// <returns></returns>
        [Column("ISAGREEEHSDEPT")]
        public string IsAgreeEhsDept { get; set; }
        /// <summary>
        /// EHS�����������
        /// </summary>
        /// <returns></returns>
        [Column("EHSDEPTOPINION")]
        public string EhsDeptOpinion { get; set; }
        /// <summary>
        /// EHS��������name
        /// </summary>
        /// <returns></returns>
        [Column("EHSDEPTPRINCIPALNAME")]
        public string EhsDeptPrincipalName { get; set; }
        /// <summary>
        /// EHS��������id
        /// </summary>
        /// <returns></returns>
        [Column("EHSDEPTPRINCIPALID")]
        public string EhsDeptPrincipalId { get; set; }
        /// <summary>
        /// רҵ������id
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYPRINCIPALID")]
        public string SpecialtyPrincipalId { get; set; }
        /// <summary>
        /// ���Ÿ��������
        /// </summary>
        /// <returns></returns>
        [Column("DEPTPRINCIPALOPINION")]
        public string DeptPrincipalOpinion { get; set; }
        /// <summary>
        /// �����¼�����
        /// </summary>
        /// <returns></returns>
        [Column("REWARDREMARK")]
        public string RewardRemark { get; set; }
        /// <summary>
        /// רҵ���������
        /// </summary>
        /// <returns></returns>
        [Column("PRINCIPALOPINION")]
        public string PrincipalOpinion { get; set; }
        /// <summary>
        /// �����û�id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ������id
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERID")]
        public string ApplyUserId { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("APPLYTIME")]
        public DateTime? ApplyTime { get; set; }


        /// <summary>
        /// ������id
        /// </summary>
        /// <returns></returns>
        [Column("APPROVERPEOPLEIDS")]
        public string ApproverPeopleIds { get; set; }

        /// <summary>
        /// ������name
        /// </summary>
        /// <returns></returns>
        [Column("APPROVERPEOPLENAMES")]
        public string ApproverPeopleNames { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Column("BELONGDEPT")]
        public string BelongDept { get; set;}

        /// <summary>
        /// ��������ID
        /// </summary>
        [Column("BELONGDEPTID")]
        public string BelongDeptId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Column("REWARDACCORD")]
        public string RewardAccord { get; set; }

        /// <summary>
        /// רҵ���
        /// </summary>
        [Column("SPECIALTYOPINION")]
        public string SpecialtyOpinion { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        [Column("REWARDMONEY")]
        public int? RewardMoney { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
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
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}