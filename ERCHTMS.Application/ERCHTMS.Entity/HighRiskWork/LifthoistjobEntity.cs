using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// �� ���������ҵ
    /// </summary>
    [Table("BIS_LIFTHOISTJOB")]
    public class LifthoistjobEntity : BaseEntity
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
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// ������ǰ׺
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCODEPREFIX")]
        public string APPLYCODEPREFIX { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCODESTR")]
        public string APPLYCODESTR { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCODE")]
        public string APPLYCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODITYUSERID")]
        public string MODITYUSERID { get; set; }
        /// <summary>
        /// �໤��
        /// </summary>
        /// <returns></returns>
        [Column("GUARDIANID")]
        public string GUARDIANID { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERID")]
        public string APPLYUSERID { get; set; }

        /// <summary>
        /// ���뵥λCODE
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCOMPANYCODE")]
        public string APPLYCOMPANYCODE { get; set; }

        /// <summary>
        /// ���뵥λ����
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCOMPANYNAME")]
        public string APPLYCOMPANYNAME { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDATE")]
        public DateTime? APPLYDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLENAME")]
        public string FLOWROLENAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODITYUSERNAME")]
        public string MODITYUSERNAME { get; set; }
        /// <summary>
        /// ��ҵ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("WORKENDDATE")]
        public DateTime? WORKENDDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPTID")]
        public string FLOWDEPTID { get; set; }
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
        [Column("FLOWROLEID")]
        public string FLOWROLEID { get; set; }
        /// <summary>
        /// ��װ����
        /// </summary>
        /// <returns></returns>
        [Column("HOISTCONTENT")]
        public string HOISTCONTENT { get; set; }
        /// <summary>
        /// ���״̬:
        ///0-����
        ///1-�ύ �����
        ///2-����
        /// </summary>
        /// <returns></returns>
        [Column("AUDITSTATE")]
        public int? AUDITSTATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// ��װʩ���ص�
        /// </summary>
        /// <returns></returns>
        [Column("CONSTRUCTIONADDRESS")]
        public string CONSTRUCTIONADDRESS { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CHARGEPERSONNAME")]
        public string CHARGEPERSONNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// ���뵥λ
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCOMPANYID")]
        public string APPLYCOMPANYID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPTNAME")]
        public string FLOWDEPTNAME { get; set; }
        /// <summary>
        /// ��װʩ����λ
        /// </summary>
        /// <returns></returns>
        [Column("CONSTRUCTIONUNITID")]
        public string CONSTRUCTIONUNITID { get; set; }
        /// <summary>
        /// ��װʩ����λCODE
        /// </summary>
        /// <returns></returns>
        [Column("CONSTRUCTIONUNITCODE")]
        public string CONSTRUCTIONUNITCODE { get; set; }
        /// <summary>
        /// ��װʩ����λ����
        /// </summary>
        /// <returns></returns>
        [Column("CONSTRUCTIONUNITNAME")]
        public string CONSTRUCTIONUNITNAME { get; set; }
        /// <summary>
        /// ���רҵID
        /// </summary>
        [Column("CHECKMAJORID")]
        public string CHECKMAJORID { get; set; }
        /// <summary>
        /// ���רҵ����
        /// </summary>
        [Column("CHECKMAJORCODE")]
        public string CHECKMAJORCODE { get; set; }
        /// <summary>
        /// ���רҵ����
        /// </summary>
        [Column("CHECKMAJORNAME")]
        public string CHECKMAJORNAME { get; set; }

        /// <summary>
        /// ��ҵ��ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("WORKSTARTDATE")]
        public DateTime? WORKSTARTDATE { get; set; }
        /// <summary>
        /// ��װ��������
        /// </summary>
        /// <returns></returns>
        [Column("TOOLNAME")]
        public string TOOLNAME { get; set; }
        /// <summary>
        /// ���������������
        ///0-30T����
        ///1-30T����
        ///2-2̨�����豸��ͬ���3T������
        /// </summary>
        /// <returns></returns>
        [Column("QUALITYTYPE")]
        public string QUALITYTYPE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FLOWID { get; set; }
        /// <summary>
        /// �໤������
        /// </summary>
        /// <returns></returns>
        [Column("GUARDIANNAME")]
        public string GUARDIANNAME { get; set; }
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
        [Column("FLOWNAME")]
        public string FLOWNAME { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERNAME")]
        public string APPLYUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("CHARGEPERSONID")]
        public string CHARGEPERSONID { get; set; }


        /// <summary>
        /// ���������������
        /// </summary>
        [Column("QUALITYTYPENAME")]
        public string QUALITYTYPENAME { get; set; }

        /// <summary>
        /// �������Ϲ���ID
        /// </summary>
        [Column("FAZLFILES")]
        public string FAZLFILES { get; set; }

        /// <summary>
        /// �����������
        /// </summary>
        [Column("QUALITY")]
        public string QUALITY { get;set;}

        /// <summary>
        /// רҵ���
        /// </summary>
        [Column("SPECIALTYTYPE")]
        public string SPECIALTYTYPE { get; set; }

        /// <summary>
        /// ��ҵ��λ���
        /// </summary>
        [Column("WORKDEPTTYPE")]
        public string WORKDEPTTYPE { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Column("ENGINEERINGNAME")]
        public string ENGINEERINGNAME { get;set;}

        /// <summary>
        /// ����ID
        /// </summary>
        [Column("ENGINEERINGID")]
        public string ENGINEERINGID { get; set; }

        /// <summary>
        /// ��˱�ע
        /// </summary>
        [Column("FLOWREMARK")]
        public string FLOWREMARK { get; set; }

        /// <summary>
        /// ��װʩ����������
        /// </summary>
        [Column("WORKAREANAME")]
        public string WORKAREANAME { get; set; }


        /// <summary>
        /// ��װʩ������Code
        /// </summary>
        [Column("WORKAREACODE")]
        public string WORKAREACODE { get; set; }

        /// <summary>
        /// רҵ�������
        /// </summary>
        [Column("SPECIALTYTYPENAME")]
        public string SPECIALTYTYPENAME { get; set; }




        public List<HighRiskRecordEntity> RiskRecord { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
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
            this.MODITYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODITYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}