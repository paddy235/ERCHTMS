using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.NosaManage
{
    /// <summary>
    /// �� ������������
    /// </summary>
    [Table("HRS_NOSAWORKS")]
    public class NosaworksEntity : BaseEntity
    {
        #region Ĭ���ֶ�
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
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
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
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
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
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        #endregion

        #region ʵ���Ա
        /// <summary>
        /// Ԫ�ظ�����id
        /// </summary>
        /// <returns></returns>
        [Column("ELEDUTYUSERID")]
        public string EleDutyUserId { get; set; }
        /// <summary>
        /// ���ύ����������
        /// </summary>
        /// <returns></returns>
        [Column("SUBMITUSERNAME")]
        public string SubmitUserName { get; set; }
        /// <summary>
        /// ������id
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        /// Ԫ�ظ���������
        /// </summary>
        /// <returns></returns>
        [Column("ELEDUTYUSERNAME")]
        public string EleDutyUserName { get; set; }
        /// <summary>
        /// ��Ŀ����
        /// </summary>
        /// <returns></returns>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// Ƶ��
        /// </summary>
        /// <returns></returns>
        [Column("RATENUM")]
        public string RateNum { get; set; }
        /// <summary>
        /// ���ύ������id
        /// </summary>
        /// <returns></returns>
        [Column("SUBMITUSERID")]
        public string SubmitUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ELENO")]
        public string EleNo { get; set; }
        /// <summary>
        /// �������ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ENDDATE")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Ԫ�����β���id
        /// </summary>
        /// <returns></returns>
        [Column("ELEDUTYDEPARTID")]
        public string EleDutyDepartId { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ACCORDING")]
        public string According { get; set; }
        /// <summary>
        /// Ԫ��id
        /// </summary>
        /// <returns></returns>
        [Column("ELEID")]
        public string EleId { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ADVISE")]
        public string Advise { get; set; }
        /// <summary>
        /// Ԫ������
        /// </summary>
        /// <returns></returns>
        [Column("ELENAME")]
        public string EleName { get; set; }
        /// <summary>
        /// Ԫ����������
        /// </summary>
        /// <returns></returns>
        [Column("ELEDUTYDEPARTNAME")]
        public string EleDutyDepartName { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERNAME")]
        public string DutyUserName { get; set; }
        /// <summary>
        /// ���β�������
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPARTNAME")]
        public string DutyDepartName { get; set; }
        /// <summary>
        /// ����������html
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERHTML")]
        public string DutyUserHtml { get; set; }
        /// <summary>
        /// ���β�������html
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPARTHTML")]
        public string DutyDepartHtml { get; set; }
        /// <summary>
        /// ���β���id
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPARTID")]
        public string DutyDepartId { get; set; }
        /// <summary>
        /// �Ƿ��ύ��ֵ���ǡ���
        /// </summary>
        /// <returns></returns>
        [Column("ISSUBMITED")]
        public string IsSubmited { get; set; }
        /// <summary>
        /// ��ɽ���
        /// </summary>
        /// <returns></returns>
        [Column("PCT")]
        public decimal? Pct { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = this.CREATEDATE.HasValue ? this.CREATEDATE : DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
            this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
            this.DutyUserHtml = this.DutyUserName;
            this.DutyDepartHtml = this.DutyDepartName;
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