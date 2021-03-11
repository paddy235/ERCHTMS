using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.PowerPlantInside
{
    /// <summary>
    /// �� ������λ�ڲ��챨
    /// </summary>
    [Table("BIS_POWERPLANTINSIDE")]
    public class PowerplantinsideEntity : BaseEntity
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
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
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
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
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
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }

        /// <summary>
        /// �¹��¼�����
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTEVENTNAME")]
        public string AccidentEventName { get; set; }
        /// <summary>
        /// �¹��¼����
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTEVENTNO")]
        public string AccidentEventNo { get; set; }
        /// <summary>
        /// �¹��¼����
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTEVENTTYPE")]
        public string AccidentEventType { get; set; }
        /// <summary>
        /// �¹��¼�����
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTEVENTPROPERTY")]
        public string AccidentEventProperty { get; set; }
        /// <summary>
        /// �¹��¼�����
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTEVENTCAUSE")]
        public string AccidentEventCause { get; set; }
        /// <summary>
        /// �������з�ʽ
        /// </summary>
        /// <returns></returns>
        [Column("OPERATIONMODE")]
        public string OperationMode { get; set; }
        /// <summary>
        /// ����ϵͳ
        /// </summary>
        /// <returns></returns>
        [Column("BELONGSYSTEM")]
        public string BelongSystem { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("HAPPENTIME")]
        public DateTime? HappenTime { get; set; }
        /// <summary>
        /// �ص㣨����
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICT")]
        public string District { get; set; }
        /// <summary>
        /// ����id
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTID")]
        public string DistrictId { get; set; }
        /// <summary>
        /// ����code
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTCODE")]
        public string DistrictCode { get; set; }
        /// <summary>
        /// ��������/��λ
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPT")]
        public string BelongDept { get; set; }
        /// <summary>
        /// ��������/��λid
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTID")]
        public string BelongDeptId { get; set; }
        /// <summary>
        /// ��������/��λcode
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTCODE")]
        public string BelongDeptCode { get; set; }
        /// <summary>
        /// ���רҵ
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTY")]
        public string Specialty { get; set; }
        /// <summary>
        /// �¹��¼��챨��
        /// </summary>
        /// <returns></returns>
        [Column("CONTROLUSERNAME")]
        public string ControlUserName { get; set; }

        /// <summary>
        /// �¹��¼��챨��id
        /// </summary>
        /// <returns></returns>
        [Column("CONTROLUSERID")]
        public string ControlUserId { get; set; }

        /// <summary>
        /// ���̲�������
        /// </summary>
        [Column("FLOWDEPTNAME")]
        public string FlowDeptName { get; set; }

        /// <summary>
        /// ���̲��ű���/ID
        /// </summary>
        [Column("FLOWDEPT")]
        public string FlowDept { get; set; }

        /// <summary>
        /// ����ID
        /// </summary>
        [Column("FLOWID")]
        public string FlowID { get; set; }

        /// <summary>
        /// ���̽�ɫ����
        /// </summary>
        [Column("FLOWROLENAME")]
        public string FlowRoleName { get; set; }

        /// <summary>
        /// ���̽�ɫ����/ID
        /// </summary>
        [Column("FLOWROLE")]
        public string FlowRole { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Column("FLOWNAME")]
        public string FlowName { get; set; }

        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("ISOVER")]
        public int? IsOver { get; set; }

        /// <summary>
        /// �Ƿ񱣴�ɹ�
        /// </summary>
        /// <returns></returns>
        [Column("ISSAVED")]
        public int? IsSaved { get; set; }

        /// <summary>
        /// Ӱ���¹��¼���������
        /// </summary>
        [Column("ACCIDENTEVENTCAUSENAME")]
        public string AccidentEventCauseName { get; set; }
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