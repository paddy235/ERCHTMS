using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������ȫ���ƻ�
    /// </summary>
    [Table("BIS_MATRIXSAFECHECK")]
    public class MatrixsafecheckEntity : BaseEntity
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
        /// �������id ����ѡ���ŷָ�
        /// </summary>
        /// <returns></returns>
        [Column("CONTENTID")]
        public string CONTENTID { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("CONTENT")]
        public string CONTENT { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CHECKTIME")]
        public DateTime? CHECKTIME { get; set; }
        /// <summary>
        /// ��鲿��id
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPT")]
        public string CHECKDEPT { get; set; }
        /// <summary>
        /// ��鲿��code
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTCODE")]
        public string CHECKDEPTCODE { get; set; }
        /// <summary>
        /// ��鲿������
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTNAME")]
        public string CHECKDEPTNAME { get; set; }
        /// <summary>
        /// �����Աid
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSER")]
        public string CHECKUSER { get; set; }
        /// <summary>
        /// �����Աcode
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERCODE")]
        public string CHECKUSERCODE { get; set; }
        /// <summary>
        /// �����Ա����
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERNAME")]
        public string CHECKUSERNAME { get; set; }
        /// <summary>
        /// 0��δ��� 1:�����
        /// </summary>
        /// <returns></returns>
        [Column("ISOVER")]
        public int? ISOVER { get; set; }
        /// <summary>
        /// ��ȫ���id
        /// </summary>
        /// <returns></returns>
        [Column("CHECKID")]
        public string CHECKID { get; set; }

        /// <summary>
        /// ������ݱ�ţ���ѡ���ŷָ�
        /// </summary>
        [Column("CONTENTNUM")]
        public string CONTENTNUM { get; set; }

        /// <summary>
        /// ��鲿�ű�ţ���ѡ���ŷָ�
        /// </summary>
        [Column("CHECKDEPTNUM")]
        public string CHECKDEPTNUM { get; set; }

        /// <summary>
        /// ��鲿��ѡ���ID
        /// </summary>
        [Column("CHECKDEPTSEL")]
        public string CHECKDEPTSEL { get; set; }

        /// <summary>
        /// �����Ա���ڲ���
        /// </summary>
        [Column("CHECKUSERDEPT")]
        public string CHECKUSERDEPT { get; set; }
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