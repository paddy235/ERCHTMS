using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.PowerPlantInside
{
    /// <summary>
    /// �� �����¹��¼�������Ϣ
    /// </summary>
    [Table("BIS_POWERPLANTHANDLEDETAIL")]
    public class PowerplanthandledetailEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// ����״̬(0.������,1.�����,2,��˲�ͨ��,3.������,4.������,5.�����)
        /// </summary>
        /// <returns></returns>
        [Column("APPLYSTATE")]
        public int? ApplyState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �����¹��¼������¼ID
        /// </summary>
        /// <returns></returns>
        [Column("POWERPLANTHANDLEID")]
        public string PowerPlantHandleId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// ����������ID
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONDUTYPERSONID")]
        public string RectificationDutyPersonId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// �������β���
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONDUTYDEPT")]
        public string RectificationDutyDept { get; set; }
        /// <summary>
        /// �������β���ID
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONDUTYDEPTID")]
        public string RectificationDutyDeptId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONTIME")]
        public DateTime? RectificationTime { get; set; }
        /// <summary>
        /// ���Ĵ�ʩ
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONMEASURES")]
        public string RectificationMeasures { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONDUTYPERSON")]
        public string RectificationDutyPerson { get; set; }

        /// <summary>
        /// ���̽ڵ㲿��
        /// </summary>
        [Column("FLOWDEPTNAME")]
        public string FlowDeptName { get; set; }

        /// <summary>
        /// ���̽ڵ㲿��id
        /// </summary>
        [Column("FLOWDEPT")]
        public string FlowDept { get; set; }

        /// <summary>
        /// ���̽ڵ��ɫ
        /// </summary>
        [Column("FLOWROLENAME")]
        public string FlowRoleName { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Column("FLOWROLE")]
        public string FlowRole { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Column("FLOWNAME")]
        public string FlowName { get; set; }
        /// <summary>
        /// ���̽ڵ�ID
        /// </summary>
        [Column("FLOWID")]
        public string FlowId { get; set; }

        /// <summary>
        /// ʵ�����Ĳ���
        /// </summary>
        [Column("REALREFORMDEPT")]
        public string RealReformDept { get; set; }

        /// <summary>
        /// ʵ�����Ĳ���ID
        /// </summary>
        [Column("REALREFORMDEPTID")]
        public string RealReformDeptId { get; set; }

        /// <summary>
        /// ʵ�����Ĳ���Code
        /// </summary>
        [Column("REALREFORMDEPTCODE")]
        public string RealReformDeptCode { get; set; }

        /// <summary>
        /// ԭ�򼰱�¶����
        /// </summary>
        [Column("REASONANDPROBLEM")]
        public string ReasonAndProblem { get; set; }

        /// <summary>
        /// ǩ�ղ���
        /// </summary>
        [Column("SIGNDEPTNAME")]
        public string SignDeptName { get; set; }

        /// <summary>
        /// ǩ�ղ���
        /// </summary>
        [Column("SIGNDEPTID")]
        public string SignDeptId { get; set; }

        /// <summary>
        /// ǩ����
        /// </summary>
        [Column("SIGNPERSONNAME")]
        public string SignPersonName { get; set; }

        /// <summary>
        /// ǩ����
        /// </summary>
        [Column("SIGNPERSONID")]
        public string SignPersonId { get; set; }

        /// <summary>
        /// �Ƿ�ָ��������
        /// </summary>
        [Column("ISASSIGNPERSON")]
        public string IsAssignPerson { get; set; }

        /// <summary>
        /// ʵ��ǩ����
        /// </summary>
        [Column("REALSIGNPERSONNAME")]
        public string RealSignPersonName { get; set; }

        /// <summary>
        /// ʵ��ǩ����
        /// </summary>
        [Column("REALSIGNPERSONID")]
        public string RealSignPersonId { get; set; }

        /// <summary>
        /// ǩ��ʱ��
        /// </summary>
        [Column("REALSIGNDATE")]
        public DateTime? RealSignDate { get; set; }

        /// <summary>
        /// ʵ��ǩ�ղ���
        /// </summary>
        [Column("REALSIGNPERSONDEPT")]
        public string RealSignPersonDept { get; set; }

        /// <summary>
        /// ʵ��ǩ�ղ���
        /// </summary>
        [Column("REALSIGNPERSONDEPTID")]
        public string RealSignPersonDeptId { get; set; }

        /// <summary>
        /// ʵ��ǩ�ղ���
        /// </summary>
        [Column("REALSIGNPERSONDEPTCODE")]
        public string RealSignPersonDeptCode { get; set; }

        /// <summary>
        /// ���
        /// </summary>
        [Column("APPLYCODE")]
        public string ApplyCode { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
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