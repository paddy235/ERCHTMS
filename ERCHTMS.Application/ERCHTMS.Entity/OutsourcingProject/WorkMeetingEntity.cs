using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// �� �������չ���
    /// </summary>
    [Table("BIS_WORKMEETING")]
    public class WorkMeetingEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
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
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERID")]
        public string ENGINEERID { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERNAME")]
        public string ENGINEERNAME { get; set; }
        /// <summary>
        /// ���̱���
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERCODE")]
        public string ENGINEERCODE { get; set; }
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
        [Column("ENGINEERAREA")]
        public string ENGINEERAREA { get; set; }
        /// <summary>
        /// ���̷��ռ���
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLEVEL")]
        public string ENGINEERLEVEL { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERLETDEPT")]
        public string ENGINEERLETDEPT { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERCONTENT")]
        public string ENGINEERCONTENT { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("MEETINGNAME")]
        public string MEETINGNAME { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("MEETINGDATE")]
        public DateTime? MEETINGDATE { get; set; }
        /// <summary>
        /// �������ͣ������ᣬ�չ��ᣩ
        /// </summary>
        /// <returns></returns>
        [Column("MEETINGTYPE")]
        public string MEETINGTYPE { get; set; }
        /// <summary>
        /// ����ص�
        /// </summary>
        /// <returns></returns>
        [Column("ADDRESS")]
        public string ADDRESS { get; set; }
        /// <summary>
        /// ��ǩ��Ա
        /// </summary>
        /// <returns></returns>
        [Column("SIGNPERSONS")]
        public string SIGNPERSONS { get; set; }
        /// <summary>
        /// Ӧ������
        /// </summary>
        /// <returns></returns>
        [Column("SHOUDPERNUM")]
        public int? SHOUDPERNUM { get; set; }
        /// <summary>
        /// ʵ������
        /// </summary>
        /// <returns></returns>
        [Column("REALPERNUM")]
        public int? REALPERNUM { get; set; }
        /// <summary>
        /// �Ƿ񽡿�����
        /// </summary>
        /// <returns></returns>
        [Column("HEALTHSTA")]
        public string HEALTHSTA { get; set; }
        /// <summary>
        /// �Ƿ����ð�ȫ�ͱ���Ʒ
        /// </summary>
        /// <returns></returns>
        [Column("SAFEGOODSSTA")]
        public string SAFEGOODSSTA { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("LNUM")]
        public int? LNUM { get; set; }
        /// <summary>
        /// �繤����
        /// </summary>
        /// <returns></returns>
        [Column("ENUM")]
        public int? ENUM { get; set; }
        /// <summary>
        /// ���ع�����
        /// </summary>
        /// <returns></returns>
        [Column("GNUM")]
        public int? GNUM { get; set; }
        /// <summary>
        /// ���ӹ�����
        /// </summary>
        /// <returns></returns>
        [Column("JNUM")]
        public int? JNUM { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ONUM")]
        public int? ONUM { get; set; }
        /// <summary>
        /// ��װ�Ƿ���Ϲ涨
        /// </summary>
        /// <returns></returns>
        [Column("CLOTHESTA")]
        public string CLOTHESTA { get; set; }
        /// <summary>
        /// ����й�֤��
        /// </summary>
        /// <returns></returns>
        [Column("CERTSTA")]
        public string CERTSTA { get; set; }
        /// <summary>
        /// ����1
        /// </summary>
        /// <returns></returns>
        [Column("CONTENT1")]
        public string CONTENT1 { get; set; }
        /// <summary>
        /// ����2
        /// </summary>
        /// <returns></returns>
        [Column("CONTENT2")]
        public string CONTENT2 { get; set; }
        /// <summary>
        /// ����3
        /// </summary>
        /// <returns></returns>
        [Column("CONTENT3")]
        public string CONTENT3 { get; set; }
        /// <summary>
        /// �������ݣ��չ��ᣩ
        /// </summary>
        /// <returns></returns>
        [Column("CONTENTOTHER")]
        public string CONTENTOTHER { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        [Column("REMARK")]
        public string REMARK { get; set; }
        /// <summary>
        /// �����б�
        /// </summary>
        public object FILES { get; set; }
        /// <summary>
        /// ɾ���ļ���ţ������,������
        /// </summary>
        public string DELETEFILEID { get; set; }
        /// <summary>
        /// �����λ
        /// </summary>
        public String OUTPROJECTNAME { get; set; }
        /// <summary>
        /// �����λ
        /// </summary>
        public String OUTPROJECTCODE { get; set; }
        /// <summary>
        /// �Ƿ��ύ
        /// </summary>
        [Column("ISCOMMIT")]
        public String ISCOMMIT { get; set; }
        [Column("ISOVER")]
        public int IsOver { get; set; }

        [Column("STARTMEETINGID")]
        public String StartMeetingid { get; set; }

        [Column("RISKLEVEL")]
        public String RiskLevel { get; set; }
        public List<WorkmeetingmeasuresEntity> MeasuresList { get; set; }
        public string ids { get; set; }
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