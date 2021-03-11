using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// �� ���������ҵ��˼�¼
    /// </summary>
    [Table("BIS_LIFTHOISTAUDITRECORD")]
    public class LifthoistauditrecordEntity : BaseEntity
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
        [JsonIgnore]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        [JsonIgnore]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        [JsonIgnore]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        [JsonIgnore]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        [JsonIgnore]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        [JsonIgnore]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODITYUSERID")]
        [JsonIgnore]
        public string MODITYUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODITYUSERNAME")]
        [JsonIgnore]
        public string MODITYUSERNAME { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDATE")]
        public DateTime? AUDITDATE { get; set; }
        /// <summary>
        /// ��˲���ID
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDEPTID")]
        public string AUDITDEPTID { get; set; }
        /// <summary>
        /// ��˲���CODE
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDEPTCODE")]
        public string AUDITDEPTCODE { get; set; }
        /// <summary>
        /// ��˲�������
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDEPTNAME")]
        public string AUDITDEPTNAME { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [Column("AUDITUSERID")]
        public string AUDITUSERID { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        /// <returns></returns>
        [Column("AUDITUSERNAME")]
        public string AUDITUSERNAME { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("AUDITREMARK")]
        public string AUDITREMARK { get; set; }
        /// <summary>
        /// ���״̬��0 or null-��ͬ�� 1-ͬ�⣩
        /// </summary>
        /// <returns></returns>
        [Column("AUDITSTATE")]
        public int? AUDITSTATE { get; set; }
        /// <summary>
        /// ҵ��ID
        /// </summary>
        /// <returns></returns>
        [Column("BUSINESSID")]
        public string BUSINESSID { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FLOWID { get; set; }

        /// <summary>
        /// ���ǩ��
        /// </summary>
        [Column("AUDITSIGNIMG")]
        public string AUDITSIGNIMG { get; set; }

        /// <summary>
        /// �Ƿ�ʧЧ  0:��Ч 1��ʧЧ
        /// </summary>
        [Column("DISABLE")]
        public int? DISABLE { get; set; }
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