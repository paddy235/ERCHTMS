using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// �� ����1.���ּܴ��衢���ա��������2.���ּܴ��衢���ա��������
    /// </summary>
    [Table("BIS_SCAFFOLD")]
    public class ScaffoldEntity : BaseEntity
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
        /// ���뵥λID
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCOMPANYID")]
        public string ApplyCompanyId { get; set; }
        /// <summary>
        /// ���뵥λCode
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCOMPANYCODE")]
        public string ApplyCompanyCode { get; set; }
        /// <summary>
        /// ���뵥λ����
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCOMPANYNAME")]
        public string ApplyCompanyName { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERID")]
        public string ApplyUserId { get; set; }

        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERNAME")]
        public string ApplyUserName { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDATE")]
        public string ApplyDate { get; set; }

        /// <summary>
        /// ������
        ///��������ĸ+���+3λ������J2018001��J2018002��
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCODE")]
        public string ApplyCode { get; set; }
        /// <summary>
        /// ��λ���(0-��λ�ڲ� 1-�����λ)
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCOMPANYTYPE")]
        public int? SetupCompanyType { get; set; }
        /// <summary>
        /// ���ּܴ�������
        ///0-6�����½��ּܴ�������
        ///1-6�����Ͻ��ּܴ�������
        /// </summary>
        /// <returns></returns>
        [Column("SETUPTYPE")]
        public int? SetupType { get; set; }
        /// <summary>
        /// ���ּܴ�������
        /// </summary>
        /// <returns></returns>
        [Column("SETUPTYPENAME")]
        public string SetupTypeName { get; set; }
        /// <summary>
        /// ʹ�õ�λID
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCOMPANYID")]
        public string SetupCompanyId { get; set; }
        /// <summary>
        /// ʹ�õ�λCode
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCOMPANYCODE")]
        public string SetupCompanyCode { get; set; }
        /// <summary>
        /// ʹ�õ�λ����
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCOMPANYNAME")]
        public string SetupCompanyName { get; set; }


        /// <summary>
        /// ����/�����λID
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCOMPANYID1")]
        public string SetupCompanyId1 { get; set; }
        /// <summary>
        /// ����/�����λCode
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCOMPANYCODE1")]
        public string SetupCompanyCode1 { get; set; }
        /// <summary>
        /// ����/�����λ����
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCOMPANYNAME1")]
        public string SetupCompanyName1 { get; set; }


        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTID")]
        public string OutProjectId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTNAME")]
        public string OutProjectName { get; set; }
        /// <summary>
        /// ���迪ʼʱ��
        ///������Ϊ�������ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("SETUPSTARTDATE")]
        public DateTime? SetupStartDate { get; set; }
        /// <summary>
        /// �������ʱ��
        ///������Ϊ�������ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("SETUPENDDATE")]
        public DateTime? SetupEndDate { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREA")]
        public string WorkArea { get; set; }
        /// <summary>
        /// ����ص�
        /// </summary>
        /// <returns></returns>
        [Column("SETUPADDRESS")]
        public string SetupAddress { get; set; }
        /// <summary>
        /// ���踺����
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCHARGEPERSON")]
        public string SetupChargePerson { get; set; }
        /// <summary>
        /// ���踺����ID
        /// </summary>
        /// <returns></returns>
        [Column("SETUPCHARGEPERSONIDS")]
        public string SetupChargePersonIds { get; set; }
        /// <summary>
        /// ������ԱID
        ///��Ա��ѡ��ʹ�á������ŷָ�
        /// </summary>
        /// <returns></returns>
        [Column("SETUPPERSONIDS")]
        public string SetupPersonIds { get; set; }
        /// <summary>
        /// ������Ա
        ///��Ա��ѡ��ʹ�á������ŷָ�
        /// </summary>
        /// <returns></returns>
        [Column("SETUPPERSONS")]
        public string SetupPersons { get; set; }
        /// <summary>
        /// ���ּ���;
        /// </summary>
        /// <returns></returns>
        [Column("PURPOSE")]
        public string Purpose { get; set; }
        /// <summary>
        /// ���ּܲ���
        /// </summary>
        /// <returns></returns>
        [Column("PARAMETER")]
        public string Parameter { get; set; }
        /// <summary>
        /// Ԥ�Ʋ��ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("EXPECTDISMENTLEDATE")]
        public DateTime? ExpectDismentleDate { get; set; }
        /// <summary>
        /// Ҫ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("DEMANDDISMENTLEDATE")]
        public DateTime? DemandDismentleDate { get; set; }
        /// <summary>
        /// ʵ�ʴ��迪ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ACTSETUPSTARTDATE")]
        public DateTime? ActSetupStartDate { get; set; }
        /// <summary>
        /// ʵ�ʴ������ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ACTSETUPENDDATE")]
        public DateTime? ActSetupEndDate { get; set; }
        /// <summary>
        /// �����ʼ����
        /// </summary>
        /// <returns></returns>
        [Column("DISMENTLESTARTDATE")]
        public DateTime? DismentleStartDate { get; set; }
        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        [Column("DISMENTLEENDDATE")]
        public DateTime? DismentleEndDate { get; set; }
        /// <summary>
        /// �����λ
        /// </summary>
        /// <returns></returns>
        [Column("DISMENTLEPART")]
        public string DismentlePart { get; set; }
        /// <summary>
        /// ���ԭ��
        /// </summary>
        /// <returns></returns>
        [Column("DISMENTLEREASON")]
        public string DismentleReason { get; set; }
        /// <summary>
        /// �����Ա
        /// </summary>
        /// <returns></returns>
        [Column("DISMENTLEPERSONS")]
        public string DismentlePersons { get; set; }

        /// <summary>
        /// �����ԱID
        /// </summary>
        /// <returns></returns>
        [Column("DISMENTLEPERSONSIDS")]
        public string DismentlePersonsIds { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("FRAMEMATERIAL")]
        public string FrameMaterial { get; set; }
        /// <summary>
        /// ������ϢID
        ///���ּܴ�����ϢID�������ֶΣ����ռ����ʱ
        ///�����û�ѡ�����ϢID
        /// </summary>
        /// <returns></returns>
        [Column("SETUPINFOID")]
        public string SetupInfoId { get; set; }
        /// <summary>
        /// ������ϢCode
        /// </summary>
        /// <returns></returns>
        [Column("SETUPINFOCODE")]
        public string SetupInfoCode { get; set; }
        /// <summary>
        /// ���ּ�����
        ///0-��������
        ///1-��������
        ///2-�������
        /// </summary>
        /// <returns></returns>
        [Column("SCAFFOLDTYPE")]
        public int? ScaffoldType { get; set; }
        /// <summary>
        /// ���״̬
        ///���ּ�����Ϊ���������롱ʱ
        ///0-������
        ///1-�����
        ///2-���δͨ��
        ///3-���ͨ��
        ///���ּ�����Ϊ���������롱ʱ
        ///0-������
        ///1-�����
        ///2-���δͨ��
        ///3-���ͨ��
        ///4-������
        ///5-����δͨ��
        ///6-����ͨ��
        ///���ּ�����Ϊ��������롱ʱ
        ///0-������
        ///1-�����
        ///2-���δͨ��
        ///3-���ͨ��
        /// </summary>
        /// <returns></returns>
        [Column("AUDITSTATE")]
        public int? AuditState { get; set; }
        /// <summary>
        /// ��ʩ��ʵ��
        /// </summary>
        /// <returns></returns>
        [Column("MEASURECARRYOUT")]
        public string MeasureCarryout { get; set; }
        
        /// <summary>
        /// ��ʩ��ʵ��ID
        /// </summary>
        /// <returns></returns>
        [Column("MEASURECARRYOUTID")]
        public string MeasureCarryoutId { get; set; }
        /// <summary>
        /// ������ʩ
        /// </summary>
        /// <returns></returns>
        [Column("MEASUREPLAN")]
        public string MeasurePlan { get; set; }


        /// <summary>
        /// ����ID
        /// </summary>
        [Column("FLOWID")]
        public string FlowId { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [Column("FLOWNAME")]
        public string FlowName { get; set; }

        /// <summary>
        /// ���̽�ɫID
        /// </summary>
        [Column("FLOWROLEID")]
        public string FlowRoleId { get; set; }

        /// <summary>
        ///���̽�ɫ��
        /// </summary>
        [Column("FLOWROLENAME")]
        public string FlowRoleName { get; set; }
        /// <summary>
        /// ���̲���ID
        /// </summary>
        [Column("FLOWDEPTID")]
        public string FlowDeptId { get; set; }

        /// <summary>
        /// ���̲�����
        /// </summary>
        [Column("FLOWDEPTNAME")]
        public string FlowDeptName { get; set; }

        /// <summary>
        /// ȷ�Ͻ����0������ 1��ȷ�� 2����� 3����ɣ�
        /// </summary>
        [Column("INVESTIGATESTATE")]
        public int? InvestigateState { get; set; }

        /// <summary>
        /// ������Ƭ
        /// </summary>
        [Column("ACCEPTFILEID")]
        public string AcceptFileId { get; set; }

        /// <summary>
        /// ʵ�ʲ����ʼʱ��
        /// </summary>
        [Column("REALITYDISMENTLESTARTDATE")]
        public DateTime? RealityDismentleStartDate { get; set; }

        /// <summary>
        /// ʵ�ʲ������ʱ��
        /// </summary>
        [Column("REALITYDISMENTLEENDDATE")]
        public DateTime? RealityDismentleEndDate { get; set; }

        /// <summary>
        ///רҵ���
        /// </summary>
        [Column("SPECIALTYTYPE")]
        public string SpecialtyType { get; set; }

        /// <summary>
        ///��˱�ע
        /// </summary>
        [Column("FLOWREMARK")]
        public string FlowRemark { get; set; }


        /// <summary>
        /// רҵ�������
        /// </summary>
        public string SpecialtyTypeName { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [Column("COPYUSERNAMES")]
        public string CopyUserNames { get; set;  }

        /// <summary>
        /// ������ID
        /// </summary>
        [Column("COPYUSERIDS")]
        public string CopyUserIds { get; set; }

        /// <summary>
        /// ��ҵ����Code
        /// </summary>
        [Column("WORKAREACODE")]
        public string WorkAreaCode { get; set; }

        /// <summary>
        /// ��ҵ״̬ 0: ������ҵ  1:��ͣ��ҵ
        /// </summary>
        [Column("WORKOPERATE")]
        public string WorkOperate { get; set; }

        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create(string keyValue)
        {
            this.Id = keyValue;
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