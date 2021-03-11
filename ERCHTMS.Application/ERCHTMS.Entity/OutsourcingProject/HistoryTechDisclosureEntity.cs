using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// �� ������ȫ��������
    /// </summary>
    [Table("EPG_HISTORYTECHDISCLOSURE")]
    public class HistoryTechDisclosureEntity : BaseEntity
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
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("DISCLOSURENAME")]
        public string DISCLOSURENAME { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("DISCLOSURETYPE")]
        public string DISCLOSURETYPE { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("DISCLOSUREDATE")]
        public DateTime? DISCLOSUREDATE { get; set; }
        /// <summary>
        /// ���׵ص�
        /// </summary>
        /// <returns></returns>
        [Column("DISCLOSUREPLACE")]
        public string DISCLOSUREPLACE { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("DISCLOSUREPERSON")]
        public string DISCLOSUREPERSON { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("DISCLOSUREPERSONID")]
        public string DISCLOSUREPERSONID { get; set; }
        /// <summary>
        /// ���ս�����
        /// </summary>
        /// <returns></returns>
        [Column("RECEIVEPERSON")]
        public string RECEIVEPERSON { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("RECEIVEPERSONID")]
        public string RECEIVEPERSONID { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("DISCLOSUREPERSONNUM")]
        public string DISCLOSUREPERSONNUM { get; set; }
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
        [Column("DISCLOSURECONTENT")]
        public string DISCLOSURECONTENT { get; set; }

        /// <summary>
        /// �����������
        /// </summary>
        [Column("ENGINEERNAME")]
        public string ENGINEERNAME { get; set; }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        [Column("ENGINEERCODE")]
        public string ENGINEERCODE { get; set; }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        [Column("ENGINEERTYPE")]
        public string ENGINEERTYPE { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Column("ENGINEERAREANAME")]
        public string EngAreaName { get; set; }

        /// <summary>
        /// ����Id
        /// </summary>
        [Column("ENGINEERAREA")]
        public string ENGINEERAREA { get; set; }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        [Column("ENGINEERLEVEL")]
        public string ENGINEERLEVEL { get; set; }
        
        /// <summary>
        /// ��Ŀ���ε�λid
        /// </summary>
        [Column("ENGINEERLETDEPT")]
        public string ENGINEERLETDEPT { get; set; }

        /// <summary>
        /// ��Ŀ���ε�λ����
        /// </summary>
        [Column("ENGINEERLETDEPTNAME")]
        public string ENGINEERLETDEPTNAME { get; set; }

        /// <summary>
        /// ��Ŀ��λ����
        /// </summary>
        [Column("ENGINEERLETDEPTCODE")]
        public string ENGINEERLETDEPTCODE { get; set; }

        /// <summary>
        /// ��Ŀ��������
        /// </summary>
        [Column("ENGINEERCONTENT")]
        public string ENGINEERCONTENT { get; set; }

        /// <summary>
        /// ����רҵ
        /// </summary>
        [Column("DISCLOSUREMAJOR")]
        public string DISCLOSUREMAJOR { get; set; }

        /// <summary>
        /// ���ײ���
        /// </summary>
        [Column("DISCLOSUREMAJORDEPT")]
        public string DISCLOSUREMAJORDEPT { get; set; }

        /// <summary>
        /// ���ײ���Id
        /// </summary>
        [Column("DISCLOSUREMAJORDEPTID")]
        public string DISCLOSUREMAJORDEPTID { get; set; }

        /// <summary>
        /// ���ײ���Code
        /// </summary>
        [Column("DISCLOSUREMAJORDEPTCODE")]
        public string DISCLOSUREMAJORDEPTCODE { get; set; }

        /// <summary>
        /// �Ƿ��ύ  0������ 1���ύ
        /// </summary>
        [Column("ISSUBMIT")]
        public int? ISSUBMIT { get; set; }

        /// <summary>
        /// ״ֵ̬ 0������ 1������� 2����˲�ͨ��  3�����ͨ��
        /// </summary>
        [Column("STATUS")]
        public int? STATUS { get; set; }

        /// <summary>
        /// ���̽ڵ�id
        /// </summary>
        [Column("FLOWID")]
        public string FLOWID { get; set; }

        /// <summary>
        /// ������ȫ������������¼
        /// </summary>
        [Column("RECID")]
        public string RecId { get; set; }
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