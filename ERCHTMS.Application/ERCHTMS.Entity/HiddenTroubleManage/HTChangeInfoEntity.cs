using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    [Table("BIS_HTCHANGEINFO")]
    public class HTChangeInfoEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ���ĸ���
        /// </summary>
        [Column("ATTACHMENT")]
        public string ATTACHMENT { get; set; }
        /// <summary>
        /// �������Ĳ����� POSTPONEPERSON
        /// </summary>
        [Column("POSTPONEPERSON")]
        public string POSTPONEPERSON { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        [Column("POSTPONEPERSONNAME")]
        public string POSTPONEPERSONNAME { get; set; }
        /// <summary>
        /// �������������������� 
        /// </summary>
        [Column("POSTPONEDEPTNAME")]
        public string POSTPONEDEPTNAME { get; set; }
        /// <summary>
        /// ����������������Code
        /// </summary>
        [Column("POSTPONEDEPT")]
        public string POSTPONEDEPT { get; set; }
        /// <summary>
        /// �������� 
        /// </summary>
        [Column("POSTPONEDAYS")]
        public int POSTPONEDAYS { get; set; }

        /// <summary>
        /// ��������״̬ 
        /// </summary>
        [Column("APPLICATIONSTATUS")]
        public string APPLICATIONSTATUS { get; set; }

        /// <summary>
        /// ��������(����)
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEDEADINENUM")]
        public int? CHANGEDEADINENUM { get; set; }
        /// <summary>
        /// �ش�����������
        /// </summary>
        /// <returns></returns>
        [Column("GREATHIDPROJECT")]
        public string GREATHIDPROJECT { get; set; }
        /// <summary>
        /// ��������ƻ�����
        /// </summary>
        /// <returns></returns>
        [Column("MANAGEPLANDATE")]
        public DateTime? MANAGEPLANDATE { get; set; }
        /// <summary>
        /// ��������ƻ����
        /// </summary>
        /// <returns></returns>
        [Column("MANAGEPLANSTATUS")]
        public string MANAGEPLANSTATUS { get; set; }
        /// <summary>
        /// Ӧ��Ԥ����λ����
        /// </summary>
        /// <returns></returns>
        [Column("PREVENTFINISHDATE")]
        public DateTime? PREVENTFINISHDATE { get; set; }
        /// <summary>
        /// Ӧ��Ԥ����λ���
        /// </summary>
        /// <returns></returns>
        [Column("PREVENTFINISHSTATUS")]
        public string PREVENTFINISHSTATUS { get; set; }
        /// <summary>
        /// ����ʱ�޵�λ����
        /// </summary>
        /// <returns></returns>
        [Column("DEADINEFINISHDATE")]
        public DateTime? DEADINEFINISHDATE { get; set; }
        /// <summary>
        /// ����ʱ�޵�λ���
        /// </summary>
        /// <returns></returns>
        [Column("DEADINEFINISHSTATUS")]
        public string DEADINEFINISHSTATUS { get; set; }
        /// <summary>
        /// �����ʽ�λ����
        /// </summary>
        /// <returns></returns>
        [Column("CAPITALFINISHDATE")]
        public DateTime? CAPITALFINISHDATE { get; set; }
        /// <summary>
        /// �����ʽ�λ���
        /// </summary>
        /// <returns></returns>
        [Column("CAPITALFINISHSTATUS")]
        public string CAPITALFINISHSTATUS { get; set; }
        /// <summary>
        /// �����ʩ��λ����
        /// </summary>
        /// <returns></returns>
        [Column("MEASUREFINISHDATE")]
        public DateTime? MEASUREFINISHDATE { get; set; }
        /// <summary>
        /// �����ʩ��λ���
        /// </summary>
        /// <returns></returns>
        [Column("MEASUREFINISHSTATUS")]
        public string MEASUREFINISHSTATUS { get; set; }
        /// <summary>
        /// �������ε�λ����
        /// </summary>
        /// <returns></returns>
        [Column("DUTYFINISHDATE")]
        public DateTime? DUTYFINISHDATE { get; set; }
        /// <summary>
        /// �������ε�λ���
        /// </summary>
        /// <returns></returns>
        [Column("DUTYFINISHSTATUS")]
        public string DUTYFINISHSTATUS { get; set; }
        /// <summary>
        /// ����Ŀ�굽λ����
        /// </summary>
        /// <returns></returns>
        [Column("TARGETFINISHDATE")]
        public DateTime? TARGETFINISHDATE { get; set; }
        /// <summary>
        /// ����Ŀ�굽λ���
        /// </summary>
        /// <returns></returns>
        [Column("TARGETFINISHSTATUS")]
        public string TARGETFINISHSTATUS { get; set; }
        /// <summary>
        /// ����ԭ��
        /// </summary>
        /// <returns></returns>
        [Column("BACKREASON")]
        public string BACKREASON { get; set; }
        /// <summary>
        /// ����������Ƭ
        /// </summary>
        /// <returns></returns>
        [Column("HIDCHANGEPHOTO")]
        public string HIDCHANGEPHOTO { get; set; }
        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        [Column("CHANGERESUME")]
        public string CHANGERESUME { get; set; }
        /// <summary>
        /// ���Ľ��
        /// </summary>
        /// <returns></returns>
        [Column("CHANGERESULT")]
        public string CHANGERESULT { get; set; }
        /// <summary>
        /// �������ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEFINISHDATE")]
        public DateTime? CHANGEFINISHDATE { get; set; }
        /// <summary>
        /// ���Ĵ�ʩ
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEMEASURE")]
        public string CHANGEMEASURE { get; set; }
        /// <summary>
        /// ʵ�������ʽ�
        /// </summary>
        /// <returns></returns>
        [Column("REALITYMANAGECAPITAL")]
        public decimal? REALITYMANAGECAPITAL { get; set; }
        /// <summary>
        /// �ƻ������ʽ�
        /// </summary>
        /// <returns></returns>
        [Column("PLANMANAGECAPITAL")]
        public decimal? PLANMANAGECAPITAL { get; set; }
        /// <summary>
        /// ��������ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("HIDESTIMATETIME")]
        public DateTime? HIDESTIMATETIME { get; set; }
        /// <summary>
        /// ���Ľ���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEDEADINE")]
        public DateTime? CHANGEDEADINE { get; set; }
        /// <summary>
        /// ���������˵绰
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEDUTYTEL")]
        public string CHANGEDUTYTEL { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEPERSONNAME")]
        public string CHANGEPERSONNAME { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEPERSON")]
        public string CHANGEPERSON { get; set; }
        /// <summary>
        /// �������ε�λ
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEDUTYDEPARTID")]
        public string CHANGEDUTYDEPARTID { get; set; }
        /// <summary>
        /// �������ε�λ����
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEDUTYDEPARTNAME")]
        public string CHANGEDUTYDEPARTNAME { get; set; }
        /// <summary>
        /// �������ε�λ
        /// </summary>
        /// <returns></returns>
        [Column("CHANGEDUTYDEPARTCODE")]
        public string CHANGEDUTYDEPARTCODE { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("HIDCODE")]
        public string HIDCODE { get; set; }
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
        /// �Զ�����
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int AUTOID { get; set; }


        /// <summary>
        /// Ӧ�ñ��(web or  android  or ios)
        /// </summary>
        [Column("APPSIGN")]
        public string APPSIGN { get; set; }

        /// <summary>
        ///�������θ�����
        /// </summary>
        [Column("CHARGEPERSON")]
        public string CHARGEPERSON { get; set; }

        /// <summary>
        /// �������θ���������
        /// </summary>
        [Column("CHARGEPERSONNAME")]
        public string CHARGEPERSONNAME { get; set; }

        /// <summary>
        /// �������θ����˵�λ����
        /// </summary>
        [Column("CHARGEDEPTID")]
        public string CHARGEDEPTID { get; set; } 

        /// <summary>
        /// �������θ����˵�λ����
        /// </summary>
        [Column("CHARGEDEPTNAME")]
        public string CHARGEDEPTNAME { get; set; }

        /// <summary>
        /// �������θ����˵�λ����
        /// </summary>
        [Column("ISAPPOINT")]
        public string ISAPPOINT { get; set; }
        #endregion


        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = DateTime.Now;
            if (string.IsNullOrEmpty(this.CREATEUSERID))
            {
                this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            }
            if (string.IsNullOrEmpty(this.CREATEUSERNAME))
            {
                this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
            }
            if (string.IsNullOrEmpty(this.CREATEUSERDEPTCODE))
            {
                this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            }
            if (string.IsNullOrEmpty(this.CREATEUSERORGCODE))
            {
                this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
            }
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
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