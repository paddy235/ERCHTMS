using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// �� ������ȫ��ʩ�䶯�����
    /// </summary>
    [Table("BIS_SAFETYCHANGE")]
    public class SafetychangeEntity : BaseEntity
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
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
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
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
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
        /// ���뵥λ
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUNIT")]
        public string APPLYUNIT { get; set; }
        /// <summary>
        /// ���뵥λID
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUNITID")]
        public string APPLYUNITID { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYPEOPLE")]
        public string APPLYPEOPLE { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("APPLYPEOPLEID")]
        public string APPLYPEOPLEID { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("APPLYTIME")]
        public DateTime? APPLYTIME { get; set; }
        /// <summary>
        /// ��ҵ��λ���� 0 ����λ�ڲ� 1 �����λ
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNITTYPE")]
        public string WORKUNITTYPE { get; set; }
        /// <summary>
        /// ��ҵ��λ
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNIT")]
        public string WORKUNIT { get; set; }
        /// <summary>
        /// ��ҵ��λID
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNITID")]
        public string WORKUNITID { get; set; }
        /// <summary>
        /// ��ҵ��λCOde
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNITCODE")]
        public string WORKUNITCODE { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTID")]
        public string PROJECTID { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTNAME")]
        public string PROJECTNAME { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREA")]
        public string WORKAREA { get; set; }
        /// <summary>
        /// ��ҵ�ص�
        /// </summary>
        /// <returns></returns>
        [Column("WORKPLACE")]
        public string WORKPLACE { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKCONTENT")]
        public string WORKCONTENT { get; set; }
        /// <summary>
        /// ��ҵ������
        /// </summary>
        /// <returns></returns>
        [Column("WORKFZR")]
        public string WORKFZR { get; set; }
        /// <summary>
        /// ��䶯�İ�ȫ��ʩ����
        /// </summary>
        /// <returns></returns>
        [Column("CHANGENAME")]
        public string CHANGENAME { get; set; }
        /// <summary>
        /// �䶯��ʽ
        /// </summary>
        /// <returns></returns>
        [Column("CHANGETYPE")]
        public string CHANGETYPE { get; set; }
        /// <summary>
        /// ����䶯ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCHANGETIME")]
        public DateTime? APPLYCHANGETIME { get; set; }
        /// <summary>
        /// �ָ�ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("RETURNTIME")]
        public DateTime? RETURNTIME { get; set; }
        /// <summary>
        /// �䶯����
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEREASON")]
        public string CHANGEREASON { get; set; }
        /// <summary>
        /// ������ʩ
        /// </summary>
        /// <returns></returns>
        [Column("PROCEDURES")]
        public string PROCEDURES { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTANCE")]
        public string ACCEPTANCE { get; set; }
        /// <summary>
        /// �Ƿ����������� 0 δ��� 1 �����
        /// </summary>
        /// <returns></returns>
        [Column("ISACCEPOVER")]
        public int? ISACCEPOVER { get; set; }
        /// <summary>
        /// �Ƿ��ύ 0 δ�ύ 1 ���ύ
        /// </summary>
        /// <returns></returns>
        [Column("ISCOMMIT")]
        public int? ISCOMMIT { get; set; }
        /// <summary>
        /// �Ƿ����������� 0 δ��� 1 �����
        /// </summary>
        /// <returns></returns>
        [Column("ISAPPLYOVER")]
        public int? ISAPPLYOVER { get; set; }
        /// <summary>
        /// ��ҵ������ID
        /// </summary>
        /// <returns></returns>
        [Column("WORKFZRID")]
        public string WORKFZRID { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYNO")]
        public string APPLYNO { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPPEOPLE")]
        public string ACCEPPEOPLE { get; set; }
        /// <summary>
        /// ����������ID
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPPEOPLEID")]
        public string ACCEPPEOPLEID { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTIME")]
        public DateTime? ACCEPTIME { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYTYPE")]
        public string APPLYTYPE { get; set; }
        /// <summary>
        /// ��ǰ����״̬
        /// </summary>
        /// <returns></returns>
        [Column("NODENAME")]
        public string NodeName { get; set; }
        /// <summary>
        /// ��ǰ���̽ڵ�Id
        /// </summary>
        /// <returns></returns>
        [Column("NODEID")]
        public string NodeId { get; set; }
        /// <summary>
        /// ���̽�ɫ����/ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLE")]
        public string FLOWROLE { get; set; }

        /// <summary>
        /// ���̽�ɫ����
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLENAME")]
        public string FLOWROLENAME { get; set; }

        /// <summary>
        /// ���̲��ű���/ID 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPT")]
        public string FLOWDEPT { get; set; }

        /// <summary>
        /// ���̲�������
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPTNAME")]
        public string FLOWDEPTNAME { get; set; }
        [Column("ISACCPCOMMIT")]
        public int ISACCPCOMMIT { get; set; }
        [Column("ACCEPDEPT")]
        public string ACCEPDEPT { get; set; }
        [Column("ACCEPDEPTID")]
        public string ACCEPDEPTID { get; set; }


        /// <summary>
        /// ʵ�ʱ䶯��ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("REALITYCHANGETIME")]
        public DateTime? REALITYCHANGETIME { get; set; }

        /// <summary>
        /// ��ҵ����Code
        /// </summary>
        [Column("WORKAREACODE")]
        public string WorkAreaCode { get; set; }

        #region �䶯����
        /// <summary>
        /// רҵ����
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYTYPE")]
        public string SPECIALTYTYPE { get; set; }

        /// <summary>
        /// ��˱�ע
        /// </summary>
        /// <returns></returns>
        [Column("FLOWREMARK")]
        public string FLOWREMARK { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("COPYUSERIDS")]
        public string COPYUSERIDS { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("COPYUSERNAMES")]
        public string COPYUSERNAMES { get; set; }

        /// <summary>
        /// ����֪ͨ������(0:�� 1:��)
        /// </summary>
        /// <returns></returns>
        [Column("ISMESSAGE")]
        public string ISMESSAGE { get; set; }

        /// <summary>
        /// רҵ����
        /// </summary>
        public string SPECIALTYTYPENAME { get; set; }
        
        #endregion

        #region ��������
        /// <summary>
        /// רҵ����
        /// </summary>
        /// <returns></returns>
        [Column("ACCSPECIALTYTYPE")]
        public string ACCSPECIALTYTYPE { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("ACCCOPYUSERIDS")]
        public string ACCCOPYUSERIDS { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("ACCCOPYUSERNAMES")]
        public string ACCCOPYUSERNAMES { get; set; }

        /// <summary>
        /// ����֪ͨ������(0:�� 1:��)
        /// </summary>
        /// <returns></returns>
        [Column("ACCISMESSAGE")]
        public string ACCISMESSAGE { get; set; }

        /// <summary>
        /// רҵ����
        /// </summary>
        public string ACCSPECIALTYTYPENAME { get; set; }
        #endregion

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