using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// �� �������֤
    /// </summary>
    [Table("BIS_LIFTHOISTCERT")]
    public class LifthoistcertEntity : BaseEntity
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
        /// ��ҵ��ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("WORKSTARTDATE")]
        public DateTime? WORKSTARTDATE { get; set; }
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
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERNAME")]
        public string APPLYUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODITYUSERNAME")]
        public string MODITYUSERNAME { get; set; }
        /// <summary>
        /// ��װʩ���ص�
        /// </summary>
        /// <returns></returns>
        [Column("CONSTRUCTIONADDRESS")]
        public string CONSTRUCTIONADDRESS { get; set; }
        /// <summary>
        /// ��װ��������
        /// </summary>
        /// <returns></returns>
        [Column("TOOLNAME")]
        public string TOOLNAME { get; set; }

        /// <summary>
        /// ��װʩ����λ
        /// </summary>
        [Column("CONSTRUCTIONUNITID")]
        public string CONSTRUCTIONUNITID { get; set; }
        /// <summary>
        /// ��װʩ����λCODE
        /// </summary>
        [Column("CONSTRUCTIONUNITCODE")]
        public string CONSTRUCTIONUNITCODE { get; set; }
        /// <summary>
        /// ��װʩ����λ����
        /// </summary>
        [Column("CONSTRUCTIONUNITNAME")]
        public string CONSTRUCTIONUNITNAME { get; set; }


        /// <summary>
        /// ˾������
        /// </summary>
        /// <returns></returns>
        [Column("DRIVERNAME")]
        public string DRIVERNAME { get; set; }

        /// <summary>
        /// ˾��֤��
        /// </summary>
        /// <returns></returns>
        [Column("DRIVERNUMBER")]
        public string DRIVERNUMBER { get; set; }

        /// <summary>
        /// רְ��Ա����
        /// </summary>
        /// <returns></returns>
        [Column("FULLTIMENAME")]
        public string FULLTIMENAME { get; set; }

        /// <summary>
        /// רְ��Ա֤��
        /// </summary>
        /// <returns></returns>
        [Column("FULLTIMENUMBER")]
        public string FULLTIMENUMBER { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FLOWID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
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
        [Column("FLOWNAME")]
        public string FLOWNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERID")]
        public string APPLYUSERID { get; set; }
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
        [Column("FLOWROLENAME")]
        public string FLOWROLENAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
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
        [Column("FLOWROLEID")]
        public string FLOWROLEID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODITYUSERID")]
        public string MODITYUSERID { get; set; }

        /// <summary>
        /// ���ص�װ��ҵID
        /// </summary>
        /// <returns></returns>
        [Column("LIFTHOISTJOBID")]
        public string LIFTHOISTJOBID { get; set; }

        /// <summary>
        /// ����������
        /// </summary>
        [Column("CHARGEPERSONNAME")]
        public string CHARGEPERSONNAME { get; set; }
        /// <summary>
        /// ����������ID
        /// </summary>
        [Column("CHARGEPERSONID")]
        public string CHARGEPERSONID { get; set; }
        /// <summary>
        /// ����������ǩ��
        /// </summary>
        [Column("CHARGEPERSONSIGN")]
        public string CHARGEPERSONSIGN { get; set; }
        /// <summary>
        /// ��װ������Ա
        /// </summary>
        [Column("HOISTAREAPERSONNAMES")]
        public string HOISTAREAPERSONNAMES { get; set; }
        /// <summary>
        /// ��װ������ԱID
        /// </summary>
        [Column("HOISTAREAPERSONIDS")]
        public string HOISTAREAPERSONIDS { get; set; }
        /// <summary>
        /// ��װ������Աǩ��
        /// </summary>
        [Column("HOISTAREAPERSONSIGNS")]
        public string HOISTAREAPERSONSIGNS { get; set; }

        /// <summary>
        /// �������������
        /// </summary>
        [Column("QUALITYTYPENAME")]
        public string QUALITYTYPENAME { get; set; }


        /// <summary>
        /// ��ҵ��λ����
        /// </summary>
        [Column("WORKDEPTTYPE")]
        public string WORKDEPTTYPE { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Column("ENGINEERINGNAME")]
        public string ENGINEERINGNAME { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Column("ENGINEERINGID")]
        public string ENGINEERINGID { get; set; }

        /// <summary>
        /// רҵ���
        /// </summary>
        [Column("SPECIALTYTYPE")]
        public string SPECIALTYTYPE { get; set; }



        [NotMapped]
        public List<LifthoistsafetyEntity> safetys { get; set; }
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
            this.MODITYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODITYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}