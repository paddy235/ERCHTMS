using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.StandardSystem
{
    /// <summary>
    /// �� ������׼�����
    /// </summary>
    [Table("HRS_STANDARDAPPLY")]
    public class StandardApplyEntity : BaseEntity
    {
        #region Ĭ���ֶ�
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
        #endregion

        #region ʵ���Ա
        /// <summary>
        /// �ļ���
        /// </summary>
        /// <returns></returns>
        [Column("FILENAME")]
        public string FileName { get; set; }
        /// <summary>
        /// �ޱಿ��id
        /// </summary>
        /// <returns></returns>
        [Column("EDITDEPTID")]
        public string EditDeptID { get; set; }
        /// <summary>
        /// �ޱಿ������
        /// </summary>
        /// <returns></returns>
        [Column("EDITDEPTNAME")]
        public string EditDeptName { get; set; }
        /// <summary>
        /// �ޱ���ID
        /// </summary>
        /// <returns></returns>
        [Column("EDITPERSONID")]
        public string EditPersonID { get; set; }
        /// <summary>
        /// �ޱ�������
        /// </summary>
        /// <returns></returns>
        [Column("EDITPERSON")]
        public string EditPerson { get; set; }
        /// <summary>
        /// �ޱ�����
        /// </summary>
        /// <returns></returns>
        [Column("EDITDATE")]
        public DateTime? EditDate { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// ������id�������|�ָ���
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERID")]
        public string CheckUserID { get; set; }
        /// <summary>
        /// �����������������|�ָ���
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERNAME")]
        public string CheckUserName { get; set; }
        /// <summary>
        /// ������id�������|�ָ���
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTID")]
        public string CheckDeptID { get; set; }
        /// <summary>
        /// ���������ƣ������|�ָ���
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTNAME")]
        public string CheckDeptName { get; set; }
        /// <summary>
        /// ��������״̬
        /// </summary>
        /// <returns></returns>
        [Column("FLOWSTATE")]
        public string FlowState { get; set; }
        /// <summary>
        /// ��������Flagֵ
        /// </summary>
        [Column("CHECKBACKFLAG")]
        public string CheckBackFlag { get; set; }
        /// <summary>
        /// ����ʱ������id�������|�ָ���
        /// </summary>
        /// <returns></returns>
        [Column("CHECKBACKUSERID")]
        public string CheckBackUserID { get; set; }
        /// <summary>
        /// ����ʱ�����������������|�ָ���
        /// </summary>
        /// <returns></returns>
        [Column("CHECKBACKUSERNAME")]
        public string CheckBackUserName { get; set; }
        /// <summary>
        /// ����ʱ������id�������|�ָ���
        /// </summary>
        /// <returns></returns>
        [Column("CHECKBACKDEPTID")]
        public string CheckBackDeptID { get; set; }
        /// <summary>
        /// ����ʱ���������ƣ������|�ָ���
        /// </summary>
        /// <returns></returns>
        [Column("CHECKBACKDEPTNAME")]
        public string CheckBackDeptName { get; set; }
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