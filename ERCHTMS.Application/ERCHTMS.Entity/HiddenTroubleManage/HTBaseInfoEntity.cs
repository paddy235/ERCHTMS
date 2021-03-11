using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    [Table("BIS_HTBASEINFO")]
    public class HTBaseInfoEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("HIDCODE")]
        public string HIDCODE { get; set; }
        /// <summary>
        /// ��ʱ�����û���
        /// </summary>
        /// <returns></returns>
        [Column("PARTICIPANT")]
        public string PARTICIPANT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// �����ص�
        /// </summary>
        /// <returns></returns>
        [Column("HIDPLACE")]
        public string HIDPLACE { get; set; }
        /// <summary>
        /// ���յ�����
        /// </summary>
        /// <returns></returns>
        [Column("RISKNAME")]
        public string RISKNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// ������Ƭ
        /// </summary>
        /// <returns></returns>
        [Column("HIDPHOTO")]
        public string HIDPHOTO { get; set; }
        /// <summary>
        /// ѡ��λ
        /// </summary>
        /// <returns></returns>
        [Column("CHOOSEDEPART")]
        public string CHOOSEDEPART { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("HIDPOINT")]
        public string HIDPOINT { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [Column("HIDPOINTNAME")]
        public string HIDPOINTNAME { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("HIDPROJECT")]
        public string HIDPROJECT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMAN")]
        public string CHECKMAN { get; set; }
        /// <summary>
        /// �ع�ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("EXPOSUREDATETIME")]
        public DateTime? EXPOSUREDATETIME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// �Ų�����
        /// </summary>
        /// <returns></returns>
        [Column("CHECKTYPE")]
        public string CHECKTYPE { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("HIDDESCRIBE")]
        public string HIDDESCRIBE { get; set; }
        /// <summary>
        /// �Ų鵥λ
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPART")]
        public string CHECKDEPART { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDEPTCODE")]
        public string CREATEDEPTCODE { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("HIDTYPE")]
        public string HIDTYPE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// ��ѵģ������
        /// </summary>
        /// <returns></returns>
        [Column("TRAINTEMPLATENAME")]
        public string TRAINTEMPLATENAME { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("HIDRANK")]
        public string HIDRANK { get; set; }
        /// <summary>
        /// ����״̬
        /// </summary>
        /// <returns></returns>
        [Column("STATES")]
        public string STATES { get; set; }

        /// <summary>
        /// ������׼
        /// </summary>
        /// <returns></returns>
        [Column("HIDDANGERNORM")]
        public string HIDDANGERNORM { get; set; }
        /// <summary>
        /// Υ����Ա����
        /// </summary>
        /// <returns></returns>
        [Column("BREAKRULEUSERNAMES")]
        public string BREAKRULEUSERNAMES { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPARTCODE")]
        public string CHECKDEPARTCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// ��������ժҪ
        /// </summary>
        /// <returns></returns>
        [Column("REPORTDIGEST")]
        public string REPORTDIGEST { get; set; }
        /// <summary>
        /// �Ƿ�Υ��
        /// </summary>
        /// <returns></returns>
        [Column("ISBREAKRULE")]
        public string ISBREAKRULE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CHOOSEDEPARTNAME")]
        public string CHOOSEDEPARTNAME { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("HIDDANGERNAME")]
        public string HIDDANGERNAME { get; set; }
        /// <summary>
        /// ��ѵģ��ID
        /// </summary>
        /// <returns></returns>
        [Column("TRAINTEMPLATEID")]
        public string TRAINTEMPLATEID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// ����������������
        /// </summary>
        /// <returns></returns>
        [Column("HIDDEPART")]
        public string HIDDEPART { get; set; }
        /// <summary>
        /// �������ڵ�
        /// </summary>
        /// <returns></returns>
        [Column("WORKSTREAM")]
        public string WORKSTREAM { get; set; }
        /// <summary>
        /// �Ų�����
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATE")]
        public DateTime? CHECKDATE { get; set; }
        /// <summary>
        /// �����Ų�����
        /// </summary>
        /// <returns></returns>
        [Column("CHECKNUMBER")]
        public string CHECKNUMBER { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 15��δ���������Ų鱶��
        /// </summary>
        /// <returns></returns>
        [Column("OVERMULTIPLE")]
        public int? OVERMULTIPLE { get; set; }
        /// <summary>
        /// ����������������
        /// </summary>
        /// <returns></returns>
        [Column("HIDDEPARTNAME")]
        public string HIDDEPARTNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPARTID")]
        public string CHECKDEPARTID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// Υ����ԱID
        /// </summary>
        /// <returns></returns>
        [Column("BREAKRULEUSERIDS")]
        public string BREAKRULEUSERIDS { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [Column("FINDDATE")]
        public DateTime? FINDDATE { get; set; }
        /// <summary>
        /// �Ǽ����� ����������/δ��������
        /// </summary>
        /// <returns></returns>
        [Column("ADDTYPE")]
        public string ADDTYPE { get; set; }
        /// <summary>
        /// �ع�״̬
        /// </summary>
        /// <returns></returns>
        [Column("EXPOSURESTATE")]
        public string EXPOSURESTATE { get; set; }
        /// <summary>
        /// ��ȫ�����ID
        /// </summary>
        [Column("SAFETYCHECKOBJECTID")]
        public string SAFETYCHECKOBJECTID { get; set; }
        /// <summary>
        /// ��ȫ�������
        /// </summary>
        [Column("SAFETYCHECKTYPE")]
        public string SAFETYCHECKTYPE { get; set; }

        /// <summary>
        /// ����������ԭ��
        /// </summary>
        [Column("HIDREASON")]
        public string HIDREASON { get; set; }

        /// <summary>
        /// ����Σ���ĳ̶�
        /// </summary>
        [Column("HIDDANGERLEVEL")]
        public string HIDDANGERLEVEL { get; set; }

        /// <summary>
        /// ���ش�ʩ
        /// </summary>
        [Column("PREVENTMEASURE")]
        public string PREVENTMEASURE { get; set; }

        /// <summary>
        /// �������ļƻ�
        /// </summary>
        [Column("HIDCHAGEPLAN")]
        public string HIDCHAGEPLAN { get; set; }

        /// <summary>
        /// Ӧ��Ԥ������
        /// </summary>
        [Column("EXIGENCERESUME")]
        public string EXIGENCERESUME { get; set; }

        /// <summary>
        /// �Ƿ���ƶ���
        /// </summary>
        [Column("ISGETAFTER")]
        public string ISGETAFTER { get; set; }

        /// <summary>
        /// ������������
        /// </summary>
        [Column("HIDPROJECTNAME")]
        public string HIDPROJECTNAME { get; set; }

        /// <summary>
        /// �Ų�������
        /// </summary>
        [Column("CHECKMANNAME")]
        public string CHECKMANNAME { get; set; }
        /// <summary>
        /// �Ų鵥λ����
        /// </summary>
        [Column("CHECKDEPARTNAME")]
        public string CHECKDEPARTNAME { get; set; }
        /// <summary>
        /// �豸��ʩId
        /// </summary>
        [Column("DEVICEID")]
        public string DEVICEID { get; set; }
        /// <summary>
        /// �豸��ʩ����
        /// </summary>
        [Column("DEVICENAME")]
        public string DEVICENAME { get; set; }
        /// <summary>
        /// �豸��ʩ����
        /// </summary>
        [Column("DEVICECODE")]
        public string DEVICECODE { get; set; }
        /// <summary>
        /// �����ԱID
        /// </summary>
        [Column("MONITORPERSONID")]
        public string MONITORPERSONID { get; set; }
        /// <summary>
        /// �����Ա����
        /// </summary>
        [Column("MONITORPERSONNAME")]
        public string MONITORPERSONNAME { get; set; }  
        /// <summary>
        /// ����Id
        /// </summary>
        [Column("RELEVANCEID")]
        public string RELEVANCEID { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        [Column("RELEVANCETYPE")]
        public string RELEVANCETYPE { get; set; }

        /// <summary>
        /// רҵ����
        /// </summary>
        [Column("MAJORCLASSIFY")]
        public string MAJORCLASSIFY { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        [Column("HIDNAME")]
        public string HIDNAME { get; set; } 
        /// <summary>
        /// ������״
        /// </summary>
        [Column("HIDSTATUS")]
        public string HIDSTATUS { get; set; }
        /// <summary>
        /// ���ܵ��µĺ��
        /// </summary>
        [Column("HIDCONSEQUENCE")] 
        public string HIDCONSEQUENCE { get; set; }

        /// <summary>
        /// Ӧ�ñ��(web or  android  or ios)
        /// </summary>
        [Column("APPSIGN")]
        public string APPSIGN { get; set; }

        /// <summary>
        /// Ӧ�ñ��(web or  android  or ios)
        /// </summary>
        [Column("UPSUBMIT")]
        public string UPSUBMIT { get; set; }

        /// <summary>
        /// �Ƿ񱾲������� 
        /// </summary>
        [Column("ISSELFCHANGE")]
        public string ISSELFCHANGE { get; set; }

        /// <summary>
        /// �Ƿ��ƶ����ļƻ� 
        /// </summary>
        [Column("ISFORMULATE")]
        public string ISFORMULATE { get; set; }

        /// <summary>
        /// ��ȫ������� 
        /// </summary>
        [Column("SAFETYCHECKNAME")]
        public string SAFETYCHECKNAME { get; set; }

        /// <summary>
        /// ������������ 
        /// </summary>
        [Column("HIDBMNAME")]
        public string HIDBMNAME { get; set; }

        /// <summary>
        /// ��������id 
        /// </summary>
        [Column("HIDBMID")] 
        public string HIDBMID { get; set; }
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