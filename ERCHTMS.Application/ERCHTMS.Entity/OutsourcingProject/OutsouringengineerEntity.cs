using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// �� �������������Ϣ��
    /// </summary>
    [Table("EPG_OUTSOURINGENGINEER")]
    public class OutsouringengineerEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// ���β���
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETDEPT")]
        public string ENGINEERLETDEPT { get; set; }
        /// <summary>
        /// �ù�����
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERUSEDEPT")]
        public string ENGINEERUSEDEPT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETPEOPLEID")]
        public string ENGINEERLETPEOPLEID { get; set; }
        /// <summary>
        /// �ù�����Id
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERUSEDEPTID")]
        public string ENGINEERUSEDEPTID { get; set; }
        /// <summary>
        /// ��ȫ��������
        /// </summary>
        /// <returns></returns>
        [Column("SAFEMANAGERPEOPLE")]
        public int? SAFEMANAGERPEOPLE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// ���̱���
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERCODE")]
        public string ENGINEERCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// ����״̬
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERSTATE")]
        public string ENGINEERSTATE { get; set; }
        /// <summary>
        /// �����λ���̸�����
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERDIRECTOR")]
        public string ENGINEERDIRECTOR { get; set; }
        /// <summary>
        /// �����λ���̸�����Id
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERDIRECTORID")]
        public string ENGINEERDIRECTORID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// �ƻ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ACTUALSTARTDATE")]
        public DateTime? ACTUALSTARTDATE { get; set; }
        /// <summary>
        /// ������ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERWORKPEOPLE")]
        public int? ENGINEERWORKPEOPLE { get; set; }
        /// <summary>
        /// Ԥ�ƹ���
        /// </summary>
        /// <returns></returns>
        [Column("PREDICTTIME")]
        public string PREDICTTIME { get; set; }
        /// <summary>
        /// ���β��Ÿ����˵绰
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETPEOPLEPHONE")]
        public string ENGINEERLETPEOPLEPHONE { get; set; }
        /// <summary>
        /// ʵ�ʹ��� 
        /// </summary>
        /// <returns></returns>
        [Column("PRACTICALTIME")]
        public string PRACTICALTIME { get; set; }
        /// <summary>
        /// ���̸����˵绰
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERDIRECTORPHONE")]
        public string ENGINEERDIRECTORPHONE { get; set; }
        /// <summary>
        /// �ù����Ÿ����˵绰
        /// </summary>
        /// <returns></returns>
        [Column("USEDEPTPEOPPHONE")]
        public string USEDEPTPEOPPHONE { get; set; }
        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERNAME")]
        public string ENGINEERNAME { get; set; }
        /// <summary>
        /// �����λId
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTID")]
        public string OUTPROJECTID { get; set; }
        /// <summary>
        /// �����λ����
        /// </summary>
        [NotMapped]
        public string OUTPROJECTCODE { get; set; }
        /// <summary>
        /// �����������
        /// </summary>
        [NotMapped]
        public string OUTPROJECTNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// �ù����Ÿ�����Id
        /// </summary>
        /// <returns></returns>
        [Column("USEDEPTPEOPLEID")]
        public string USEDEPTPEOPLEID { get; set; }
        /// <summary>
        /// ʵ�ʿ���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("PLANENDDATE")]
        public DateTime? PLANENDDATE { get; set; }
        /// <summary>
        /// ���̹�ģ
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERSCALE")]
        public string ENGINEERSCALE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// ���̼�������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERTECHPERSON")]
        public int? ENGINEERTECHPERSON { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERTYPE")]
        public string ENGINEERTYPE { get; set; }
        public string ENGINEERTYPENAME { get; set; }
        /// <summary>
        /// �������Ÿ�����
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETPEOPLE")]
        public string ENGINEERLETPEOPLE { get; set; }
        /// <summary>
        /// ���̷��յȼ�
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLEVEL")]
        public string ENGINEERLEVEL { get; set; }
        public string ENGINEERLEVELNAME { get; set; }
        /// <summary>
        /// ��ȫ��֤��
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERCASHDEPOSIT")]
        public string ENGINEERCASHDEPOSIT { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// ʵ�����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ACTUALENDDATE")]
        public DateTime? ACTUALENDDATE { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERAREA")]
        public string ENGINEERAREA { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        [Column("ENGINEERAREANAME")]
        public string EngAreaName { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string ENGINEERAREANAME { get; set; }
        /// <summary>
        /// �ù����Ÿ�����
        /// </summary>
        /// <returns></returns>
        [Column("USEDEPTPEOPLE")]
        public string USEDEPTPEOPLE { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERCONTENT")]
        public string ENGINEERCONTENT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETDEPTID")]
        public string ENGINEERLETDEPTID { get; set; }
        /// <summary>
        /// �ƻ�����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("PLANSTARTDATE")]
        public DateTime? PLANSTARTDATE { get; set; }

        /// <summary>
        /// ͣ����״̬ 0 ����1��ͣ�� 
        /// </summary>
        /// <returns></returns>
        [Column("STOPRETURNSTATE")]
        public string STOPRETURNSTATE { get; set; }
        /// <summary>
        /// �Ƿ������������0 �� 1 ����
        /// </summary>
        /// <returns></returns>
        [Column("ISDEPTADD")]
        public int IsDeptAdd { get; set; }
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
        /// ����λID
        /// </summary>
        [Column("SUPERVISORID")]
        public string SupervisorId { get; set; }

        /// <summary>
        /// ����λ����
        /// </summary>
        [Column("SUPERVISORNAME")]
        public string SupervisorName { get; set; }

        /// <summary>
        /// �����λ����
        /// </summary>
        [Column("DEPTTYPE")]
        public string DeptType { get; set; }

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
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
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