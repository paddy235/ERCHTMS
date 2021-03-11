using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// �� ������ͬ
    /// </summary>
    [Table("EPG_COMPACT")]
    public class CompactEntity : BaseEntity
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
        /// ��ͬ��Чʱ��
        /// </summary>
        /// <returns></returns>
        [Column("COMPACTTAKEEFFECTDATE")]
        public DateTime? COMPACTTAKEEFFECTDATE { get; set; }
        /// <summary>
        /// ��ͬ��Чʱ��
        /// </summary>
        /// <returns></returns>
        [Column("COMPACTEFFECTIVEDATE")]
        public DateTime? COMPACTEFFECTIVEDATE { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTID")]
        public string PROJECTID { get; set; }

        /// <summary>
        ///�Ƿ���0��1��
        /// </summary>
        /// <returns></returns>
        [Column("ISSEND")]
        public string ISSEND { get; set; }

        /// <summary>
        ///��ͬ���
        /// </summary>
        /// <returns></returns>
        [Column("COMPACTNO")]
        public string COMPACTNO { get; set; }

        /// <summary>
        ///�׷�ǩ����/��λ
        /// </summary>
        /// <returns></returns>
        [Column("FIRSTPARTY")]
        public string FIRSTPARTY { get; set; }

        /// <summary>
        ///�ҷ�ǩ����/��λ
        /// </summary>
        /// <returns></returns>
        [Column("SECONDPARTY")]
        public string SECONDPARTY { get; set; }

        /// <summary>
        /// ��ͬ���
        /// </summary>
        [Column("COMPACTMONEY")]
        public decimal? COMPACTMONEY { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        [Column("REMARK")]
        public string REMARK { get; set;}
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