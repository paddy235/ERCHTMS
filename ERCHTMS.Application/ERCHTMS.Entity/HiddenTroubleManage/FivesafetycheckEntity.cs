using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// �� �����嶨��ȫ���
    /// </summary>
    [Table("BIS_FIVESAFETYCHECK")]
    public class FivesafetycheckEntity : BaseEntity
    {
        #region ʵ���Ա
       
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("CHECKAREANAME")]
        public string CHECKAREANAME { get; set; }
        /// <summary>
        /// ������鳤ID
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMANAGEMANID")]
        public string CHECKMANAGEMANID { get; set; }
        /// <summary>
        /// ������鳤
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMANAGEMAN")]
        public string CHECKMANAGEMAN { get; set; }
        /// <summary>
        /// �����Աid ������ŷָ�
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERSID")]
        public string CHECKUSERSID { get; set; }
        /// <summary>
        /// �����Ա
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERS")]
        public string CHECKUSERS { get; set; }
        /// <summary>
        /// ��鲿��ID ������ŷָ�
        /// </summary>
        /// <returns></returns>
        [Column("CHECKEDDEPARTID")]
        public string CHECKEDDEPARTID { get; set; }
        /// <summary>
        /// ��鲿������
        /// </summary>
        /// <returns></returns>
        [Column("CHECKEDDEPART")]
        public string CHECKEDDEPART { get; set; }
        /// <summary>
        /// ��鼶��ID
        /// </summary>
        /// <returns></returns>
        [Column("CHECKLEVELID")]
        public string CHECKLEVELID { get; set; }
        /// <summary>
        /// ��鼶������
        /// </summary>
        /// <returns></returns>
        [Column("CHECKLEVEL")]
        public string CHECKLEVEL { get; set; }
        /// <summary>
        /// �������id
        /// </summary>
        /// <returns></returns>
        [Column("CHECKTYPEID")]
        public string CHECKTYPEID { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("CHECKTYPE")]
        public string CHECKTYPE { get; set; }
        /// <summary>
        /// ������ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CHECKENDDATE")]
        public DateTime? CHECKENDDATE { get; set; }
        /// <summary>
        /// ��鿪ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CHECKBEGINDATE")]
        public DateTime? CHECKBEGINDATE { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("CHECKNAME")]
        public string CHECKNAME { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("ISOVER")]
        public int? ISOVER { get; set; }
        /// <summary>
        /// �Ƿ񱣴�ɹ�
        /// </summary>
        /// <returns></returns>
        [Column("ISSAVED")]
        public int? ISSAVED { get; set; }
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
        /// ����Id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FLOWID { get; set; }
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
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
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
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// ��ǩ������ԱID ������ŷָ�
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTALL")]
        public string CHECKDEPTALL { get; set; }

        /// <summary>
        /// ��ǩ����ID ������ŷָ�
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERALL")]
        public string CHECKUSERALL { get; set; }

        /// <summary>
        /// ��鲿��ID ������ŷָ�(�ű�ʹ�ã���ֹ������Ҫ���ϼ���ֱ���޸����Ｔ��)
        /// </summary>
        /// <returns></returns>
        [Column("CHECKEDDEPARTIDALL")]
        public string CHECKEDDEPARTIDALL { get; set; }
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