using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// �� ����������������Ϣ��
    /// </summary>
    [Table("EPG_APTITUDEINVESTIGATEINFO")]
    public class AptitudeinvestigateinfoEntity : BaseEntity
    {
        #region ʵ���Ա
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
        /// �����λId
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTID")]
        public string OUTPROJECTID { get; set; }
        /// <summary>
        /// �������Id
        /// </summary>
        /// <returns></returns>
        [Column("OUTENGINEERID")]
        public string OUTENGINEERID { get; set; }
        /// <summary>
        /// Ӫҵִ��--������ô���
        /// </summary>
        /// <returns></returns>
        [Column("BUSCODE")]
        public string BUSCODE { get; set; }
        /// <summary>
        /// Ӫҵִ��--��֤����
        /// </summary>
        /// <returns></returns>
        [Column("BUSCERTIFICATE")]
        public string BUSCERTIFICATE { get; set; }
        /// <summary>
        /// Ӫҵִ��--��Чʱ����
        /// </summary>
        /// <returns></returns>
        [Column("BUSVALIDSTARTTIME")]
        public DateTime? BUSVALIDSTARTTIME { get; set; }
        /// <summary>
        /// Ӫҵִ��--��Чʱ��ֹ
        /// </summary>
        /// <returns></returns>
        [Column("BUSVALIDENDTIME")]
        public DateTime? BUSVALIDENDTIME { get; set; }
        /// <summary>
        /// ��ȫ�������֤--������ô���
        /// </summary>
        /// <returns></returns>
        [Column("SPLCODE")]
        public string SPLCODE { get; set; }
        /// <summary>
        /// ��ȫ�������֤--��֤����
        /// </summary>
        /// <returns></returns>
        [Column("SPLCERTIFICATE")]
        public string SPLCERTIFICATE { get; set; }
        /// <summary>
        /// ��ȫ�������֤--��Чʱ����
        /// </summary>
        /// <returns></returns>
        [Column("SPLVALIDSTARTTIME")]
        public DateTime? SPLVALIDSTARTTIME { get; set; }
        /// <summary>
        /// ��ȫ�������֤--��Чʱ��ֹ
        /// </summary>
        /// <returns></returns>
        [Column("SPLVALIDENDTIME")]
        public DateTime? SPLVALIDENDTIME { get; set; }
        /// <summary>
        /// ����֤��---����֤����
        /// </summary>
        /// <returns></returns>
        [Column("CQCODE")]
        public string CQCODE { get; set; }
        /// <summary>
        /// ����֤��---��֤����
        /// </summary>
        /// <returns></returns>
        [Column("CQORG")]
        public string CQORG { get; set; }
        /// <summary>
        /// ����֤��---���ʷ�Χ
        /// </summary>
        /// <returns></returns>
        [Column("CQRANGE")]
        public string CQRANGE { get; set; }
        /// <summary>
        /// ����֤��---��֤�ȼ�
        /// </summary>
        /// <returns></returns>
        [Column("CQLEVEL")]
        public string CQLEVEL { get; set; }
        /// <summary>
        /// ����֤��---��Ч����
        /// </summary>
        /// <returns></returns>
        [Column("CQVALIDSTARTTIME")]
        public DateTime? CQVALIDSTARTTIME { get; set; }
        /// <summary>
        /// ����֤��---��Ч��ֹ
        /// </summary>
        /// <returns></returns>
        [Column("CQVALIDENDTIME")]
        public DateTime? CQVALIDENDTIME { get; set; }
        /// <summary>
        /// ��ע�ֶ�
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string REMARK { get; set; }
        /// <summary>
        /// 0:���� 1:���ύ
        /// </summary>
        /// <returns></returns>
        [Column("ISSAVEORCOMMIT")]
        public string ISSAVEORCOMMIT { get; set; }

        /// <summary>
        /// ��ϵ��
        /// </summary>
        /// <returns></returns>
        [Column("LINKMAN")]
        public string LINKMAN { get; set; }
        /// <summary>
        /// ��ϵ�˵绰
        /// </summary>
        /// <returns></returns>
        [Column("LINKMANPHONE")]
        public string LINKMANPHONE { get; set; }
        /// <summary>
        /// ���˴���
        /// </summary>
        /// <returns></returns>
        [Column("LEGALREP")]
        public string LEGALREP { get; set; }
        /// <summary>
        /// ��ҵ��ַ
        /// </summary>
        /// <returns></returns>
        [Column("ADDRESS")]
        public string ADDRESS { get; set; }
        /// <summary>
        /// ���˴���绰
        /// </summary>
        /// <returns></returns>
        [Column("LEGALREPPHONE")]
        public string LEGALREPPHONE { get; set; }
        /// <summary>
        /// �����λ�ſ�
        /// </summary>
        /// <returns></returns>
        [Column("GENERALSITUATION")]
        public string GENERALSITUATION { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("EMAIL")]
        public string EMAIL { get; set; }
        /// <summary>
        /// ͳһ������ô���
        /// </summary>
        /// <returns></returns>
        [Column("CREDITCODE")]
        public string CREDITCODE { get; set; }
        /// <summary>
        /// ��ϵ�˴���
        /// </summary>
        /// <returns></returns>
        [Column("LINKMANFAX")]
        public string LINKMANFAX { get; set; }
        /// <summary>
        /// ���˴�����
        /// </summary>
        /// <returns></returns>
        [Column("LEGALREPFAX")]
        public string LEGALREPFAX { get; set; }
        /// <summary>
        /// �����λ���̸�����
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERDIRECTOR")]
        public string ENGINEERDIRECTOR { get; set; }
        /// <summary>
        /// ��ȫ��������
        /// </summary>
        /// <returns></returns>
        [Column("SAFEMANAGERPEOPLE")]
        public int? SAFEMANAGERPEOPLE { get; set; }
        /// <summary>
        /// ������ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERWORKPEOPLE")]
        public int? ENGINEERWORKPEOPLE { get; set; }
        /// <summary>
        /// ���̸����˵绰
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERDIRECTORPHONE")]
        public string ENGINEERDIRECTORPHONE { get; set; }
        /// <summary>
        /// ���̼�������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERTECHPERSON")]
        public int? ENGINEERTECHPERSON { get; set; }

        [Column("NEXTCHECKDEPTCODE")]
        public string NEXTCHECKDEPTCODE { get; set; }
        [Column("ISAUDITOVER")]
        public string ISAUDITOVER { get; set; }
        [Column("NEXTCHECKROLENAME")]
        public string NEXTCHECKROLENAME { get; set; }
        [Column("NEXTCHECKDEPTID")]
        public string NEXTCHECKDEPTID { get; set; }
         [Column("ENGINEERCASHDEPOSIT")]
        public string ENGINEERCASHDEPOSIT { get; set; }
         [Column("ENGINEERCONTENT")]
        public string ENGINEERCONTENT { get; set; }
         [Column("ENGINEERSCALE")]
        public string ENGINEERSCALE { get; set; }
        /// <summary>
        /// �Ƿ��а�ȫ���֤ 0 �� 1��
        /// </summary>
        [Column("ISXK")]
         public string ISXK { get; set; }
        /// <summary>
        /// �Ƿ�������֤ 0 �� 1��
        /// </summary>
        [Column("ISZZZJ")]
        public string ISZZZJ { get; set; }
        /// <summary>
        /// �������Id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }
        /// <summary>
        /// �����λ�ֳ�������
        /// </summary>
        /// <returns></returns>
        [Column("UNITSUPER")]
        public string UnitSuper { get; set; }
        /// <summary>
        /// �����λ�ֳ�������Id
        /// </summary>
        /// <returns></returns>
        [Column("UNITSUPERID")]
        public string UnitSuperId { get; set; }
        /// <summary>
        /// �����λ�ֳ������˵绰
        /// </summary>
        /// <returns></returns>
        [Column("UNITSUPERPHONE")]
        public string UnitSuperPhone { get; set; }
        /// <summary>
        /// ί����
        /// </summary>
        [Column("MANDATOR")]
        public string Mandator { get; set; }
        /// <summary>
        /// ί����Id
        /// </summary>
        [Column("MANDATORID")]
        public string MandatorId { get; set; }
        /// <summary>
        /// ��Ȩ��Id
        /// </summary>
        [Column("CERTIGIERID")]
        public string CertigierId { get; set; }
        /// <summary>
        /// ��Ȩ��Id
        /// </summary>
        [Column("CERTIGIER")]
        public string Certigier { get; set; }
        /// <summary>
        /// ��Ч����
        /// </summary>
        [Column("IMPOWERSTARTTIME")]
        public DateTime? ImpowerStartTime { get; set; }
        /// <summary>
        /// ��Ч��ֹ
        /// </summary>
        [Column("IMPOWERENDTIME")]
        public DateTime? ImpowerEndTime { get; set; }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        [Column("PROJECTMANAGER")]
        public string ProjectManager { get; set; }

        /// <summary>
        /// ��Ŀ����绰
        /// </summary>
        [Column("PROJECTMANAGERTEL")]
        public string ProjectManagerTel { get; set; }

        /// <summary>
        /// ��ȫ������
        /// </summary>
        [Column("SAFETYMODERATOR")]
        public string SafetyModerator { get; set; }

        /// <summary>
        /// ��ȫ�����˵绰
        /// </summary>
        [Column("SAFETYMODERATORTEL")]
        public string SafetyModeratorTel { get; set; }
        /// <summary>
        /// ��ͬ�׷�ǩ����λ/��
        /// </summary>
        [Column("COMPACTFIRSTPARTY")]
        public string COMPACTFIRSTPARTY { get; set; }
        /// <summary>
        /// �ҷ�ǩ����λ/��
        /// </summary>
        [Column("COMPACTSECONDPARTY")]
        public string COMPACTSECONDPARTY { get; set; }
        /// <summary>
        /// ��ͬ��Чʱ��
        /// </summary>
        [Column("COMPACTTAKEEFFECTDATE")]
        public DateTime? COMPACTTAKEEFFECTDATE { get; set; }
        /// <summary>
        /// ��ͬ��ֹʱ��
        /// </summary>
        [Column("COMPACTEFFECTIVEDATE")]
        public DateTime? COMPACTEFFECTIVEDATE { get; set; }
        /// <summary>
        /// ��ͬ���
        /// </summary>
        [Column("COMPACTNO")]
        public string COMPACTNO { get; set; }
        /// <summary>
        /// ��ͬ���
        /// </summary>
        [Column("COMPACTMONEY")]
        public decimal? COMPACTMONEY { get; set; }
        /// <summary>
        /// ��ͬ��ע
        /// </summary>
        [Column("COMPACTREMARK")]
        public string COMPACTREMARK { get; set; }
        /// <summary>
        /// Э��׷�ǩ����λ/��Ա
        /// </summary>
        [Column("PROTOCOLFIRSTPARTY")]
        public string PROTOCOLFIRSTPARTY { get; set; }
        /// <summary>
        /// Э���ҷ�ǩ����λ/��Ա
        /// </summary>
        [Column("PROTOCOLSECONDPARTY")]
        public string PROTOCOLSECONDPARTY { get; set; }
        /// <summary>
        /// ǩ���ص�
        /// </summary>
        [Column("SIGNPLACE")]
        public string SIGNPLACE { get; set; }
        /// <summary>
        /// ǩ��ʱ��
        /// </summary>
        [Column("SIGNDATE")]
        public DateTime? SIGNDATE { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID)?Guid.NewGuid().ToString():ID;
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
           this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
           this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
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