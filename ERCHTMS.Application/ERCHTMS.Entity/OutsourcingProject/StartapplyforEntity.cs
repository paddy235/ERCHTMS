using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// �� �������������
    /// </summary>
    [Table("EPG_STARTAPPLYFOR")]
    public class StartapplyforEntity : BaseEntity
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
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYPEOPLEID")]
        public string APPLYPEOPLEID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("APPLYPEOPLE")]
        public string APPLYPEOPLE { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("APPLYTIME")]
        public DateTime? APPLYTIME { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYTYPE")]
        public string APPLYTYPE { get; set; }
        /// <summary>
        /// ���뿪��ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("APPLYRETURNTIME")]
        public DateTime? APPLYRETURNTIME { get; set; }
        /// <summary>
        /// �ƻ�����ʱ��
        /// </summary>
        [Column("APPLYENDTIME")]
        public DateTime? APPLYENDTIME { get; set; }
        /// <summary>
        /// ����ԭ��
        /// </summary>
        /// <returns></returns>
        [Column("APPLYCAUSE")]
        public string APPLYCAUSE { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string REMARK { get; set; }
        /// <summary>
        /// ���뵥��
        /// </summary>
        /// <returns></returns>
        [Column("APPLYNO")]
        public string APPLYNO { get; set; }
        /// <summary>
        /// �Ƿ��ύ 0 ��δ�ύ 1���ύ
        /// </summary>
        /// <returns></returns>
        [Column("ISCOMMIT")]
        public string ISCOMMIT { get; set; }
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
        ///�Ƿ������0:δ������1:�ѽ�����
        /// </summary>
        /// <returns></returns>
        [Column("ISOVER")]
        public int IsOver { get; set; }

        /// <summary>
        ///��Ŀ�����(��Ӣ�Ķ��ŷָ���0:δ��ɣ�1:�����)
        /// </summary>
        /// <returns></returns>
        [Column("CHECKRESULT")]
        public string CheckResult { get; set; }
        /// <summary>
        ///��Ŀ�����(��Ӣ�Ķ��ŷָ�)
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERS")]
        public string CheckUsers { get; set; }
        /// <summary>
        ///��˽�ɫ
        /// </summary>
        /// <returns></returns>
        [Column("AUDITROLE")]
        public string AuditRole { get; set; }

        /// <summary>
        ///�Ƿ���������0:δ������1:�ѽ�����
        /// </summary>
        /// <returns></returns>
        [Column("ISINVESTOVER")]
        public int ISINVESTOVER { get; set; }

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
        [Column("SAFETYMAN")]
        public string SafetyMan { get; set; }
        [Column("DUTYMAN")]
        public string DutyMan { get; set; }
        [Column("HTNUM")]
        public string htnum { get; set; }

    
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