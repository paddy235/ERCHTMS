using ERCHTMS.Code;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERCHTMS.Entity.ComprehensiveManage
{
    /// <summary>
    /// �� ������Ϣ����
    /// </summary>
    [Table("HRS_INFOSUBMIT")]
    public class InfoSubmitEntity : BaseEntity
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
        /// ������Ϣ����
        /// </summary>
        [Column("INFONAME")]
        public string InfoName { get; set; }
        /// <summary>
        /// Ҫ��
        /// </summary>
        [Column("REQUIRE")]
        public string Require { get; set; }
        /// <summary>
        /// ���Ϳ�ʼʱ��
        /// </summary>
        [Column("STARTTIME")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// ���ͽ���ʱ��
        /// </summary>
        [Column("ENDTIME")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// ������id
        /// </summary>
        [Column("SUBMITUSERID")]
        public string SubmitUserId { get; set; }
        /// <summary>
        /// �ѱ�����id
        /// </summary>
        [Column("SUBMITEDUSERID")]
        public string SubmitedUserId { get; set; }
        /// <summary>
        /// �������ʺ�
        /// </summary>
        [Column("SUBMITUSERACCOUNT")]
        public string SubmitUserAccount { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        [Column("SUBMITUSERNAME")]
        public string SubmitUserName { get; set; }
        /// <summary>
        /// ���Ͳ���id
        /// </summary>
        [Column("SUBMITDEPARTID")]
        public string SubmitDepartId { get; set; }
        /// <summary>
        /// ���Ͳ�������
        /// </summary>
        [Column("SUBMITDEPARTNAME")]
        public string SubmitDepartName { get; set; }
        /// <summary>
        /// �������������ٷֱȣ�
        /// </summary>
        [Column("PCT")]
        public decimal? Pct { get; set; }
        /// <summary>
        /// ʣ��δ��������
        /// </summary>
        [Column("REMNUM")]
        public int? Remnum { get; set; }
        /// <summary>
        /// ʣ��δ����������
        /// </summary>
        [Column("REMUSERNAME")]
        public string RemUserName { get; set; }
        /// <summary>
        /// ʣ��δ�����˲���
        /// </summary>
        [Column("REMDEPARTNAME")]
        public string RemDepartName { get; set; }
        /// <summary>
        /// �Ƿ���(ֵ���ǡ���)
        /// </summary>
        [Column("ISSUBMIT")]
        public string IsSubmit { get; set; }
        /// <summary>
        /// ����(�±������������걨���걨)
        /// </summary>
        [Column("INFOTYPE")]
        public string InfoType { get; set; }
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