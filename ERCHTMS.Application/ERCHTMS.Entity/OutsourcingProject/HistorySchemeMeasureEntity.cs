using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// �� ������ʷ������ʩ����
    /// </summary>
    [Table("EPG_HISTORYSCHEMEMEASURE")]
    public class HistorySchemeMeasureEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("FLOWNAME")]
        public string FLOWNAME { get; set; }

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

        /// <summary>
        /// �Ƿ񱣴�
        /// </summary>
        /// <returns></returns>
        [Column("ISSAVED")]
        public string ISSAVED { get; set; }

        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("ISOVER")]
        public string ISOVER { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZER")]
        public string ORGANIZER { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZTIME")]
        public DateTime? ORGANIZTIME { get; set; }

        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("CONTRACTID")]
        public string CONTRACTID { get; set; } 
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTID")]
        public string PROJECTID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// �ύʱ��
        /// </summary>
        /// <returns></returns>
        [Column("SUBMITDATE")]
        public DateTime? SUBMITDATE { get; set; }
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
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// �ύ��
        /// </summary>
        /// <returns></returns>
        [Column("SUBMITPERSON")]
        public string SUBMITPERSON { get; set; }
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
        /// �ύ����
        /// </summary>
        /// <returns></returns>
        [Column("SUMMITCONTENT")]
        public string SummitContent { get; set; }

        /// <summary>
        /// ���̱��
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERCODE")]
        public string ENGINEERCODE { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERNAME")]
        public string ENGINEERNAME { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERTYPE")]
        public string ENGINEERTYPE { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERAREANAME")]
        public string ENGINEERAREANAME { get; set; }
        /// <summary>
        /// ���̷��յȼ�
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLEVEL")]
        public string ENGINEERLEVEL { get; set; }
        /// <summary>
        /// ���β�������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETDEPT")]
        public string ENGINEERLETDEPT { get; set; }
        /// <summary>
        /// ���β���id
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETDEPTID")]
        public string ENGINEERLETDEPTID { get; set; }
        /// <summary>
        /// ���β���code
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETDEPTCODE")]
        public string ENGINEERLETDEPTCODE { get; set; }
        /// <summary>
        /// ��������id
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERAREA")]
        public string ENGINEERAREA { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERCONTENT")]
        public string ENGINEERCONTENT { get; set; }

        /// <summary>
        /// ����רҵ
        /// </summary>
        /// <returns></returns>
        [Column("BELONGMAJOR")]
        public string BELONGMAJOR { get; set; }
        /// <summary>
        /// �������β���
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTNAME")]
        public string BELONGDEPTNAME { get; set; }
        /// <summary>
        /// �������β���ID
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTID")]
        public string BELONGDEPTID { get; set; }
        /// <summary>
        /// �������β���code
        /// </summary>
        /// <returns></returns>
        [Column("BELONGCODE")]
        public string BELONGCODE { get; set; }
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
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}