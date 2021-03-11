using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// �� �������������Ա��
    /// </summary>
    [Table("EPG_APTITUDEINVESTIGATEPEOPLE")]
    public class AptitudeinvestigatepeopleEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// �û�����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// �û�����
        /// </summary>
        /// <returns></returns>
        [Column("ENCODE")]
        public string ENCODE { get; set; }
        /// <summary>
        /// ��ʵ����
        /// </summary>
        /// <returns></returns>
        [Column("REALNAME")]
        public string REALNAME { get; set; }
        /// <summary>
        /// ͷ��
        /// </summary>
        /// <returns></returns>
        [Column("HEADICON")]
        public string HEADICON { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("BIRTHDAY")]
        public DateTime? BIRTHDAY { get; set; }
        /// <summary>
        /// �ֻ�
        /// </summary>
        /// <returns></returns>
        [Column("MOBILE")]
        public string MOBILE { get; set; }
        /// <summary>
        /// �绰
        /// </summary>
        /// <returns></returns>
        [Column("TELEPHONE")]
        public string TELEPHONE { get; set; }
        /// <summary>
        /// �����ʼ�
        /// </summary>
        /// <returns></returns>
        [Column("EMAIL")]
        public string EMAIL { get; set; }
        /// <summary>
        /// QQ��
        /// </summary>
        /// <returns></returns>
        [Column("OICQ")]
        public string OICQ { get; set; }
        /// <summary>
        /// ΢�ź�
        /// </summary>
        /// <returns></returns>
        [Column("WECHAT")]
        public string WECHAT { get; set; }
        /// <summary>
        /// MSN
        /// </summary>
        /// <returns></returns>
        [Column("MSN")]
        public string MSN { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZEID")]
        public string ORGANIZEID { get; set; }
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [Column("DUTYID")]
        public string DUTYID { get; set; }
        /// <summary>
        /// ��λ����
        /// </summary>
        /// <returns></returns>
        [Column("DUTYNAME")]
        public string DUTYNAME { get; set; }
        /// <summary>
        /// ְλ����
        /// </summary>
        /// <returns></returns>
        [Column("POSTID")]
        public string POSTID { get; set; }
        /// <summary>
        /// ְλ����
        /// </summary>
        /// <returns></returns>
        [Column("POSTNAME")]
        public string POSTNAME { get; set; }
        /// <summary>
        /// �Ա�
        /// </summary>
        /// <returns></returns>
        [Column("GENDER")]
        public string GENDER { get; set; }
        /// <summary>
        /// �Ƿ������豸������Ա
        /// </summary>
        /// <returns></returns>
        [Column("ISSPECIALEQU")]
        public string ISSPECIALEQU { get; set; }
        /// <summary>
        /// �Ƿ�������ҵ��Ա
        /// </summary>
        /// <returns></returns>
        [Column("ISSPECIAL")]
        public string ISSPECIAL { get; set; }
        /// <summary>
        /// �������Id
        /// </summary>
        /// <returns></returns>
        [Column("OUTENGINEERID")]
        public string OUTENGINEERID { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("NATION")]
        public string NATION { get; set; }
        /// <summary>
        /// ��Ա����
        /// </summary>
        /// <returns></returns>
        [Column("USERTYPE")]
        public string USERTYPE { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("NATIVE")]
        public string NATIVE { get; set; }
        /// <summary>
        /// �Ƿ������Ա
        /// </summary>
        /// <returns></returns>
        [Column("ISEPIBOLY")]
        public string ISEPIBOLY { get; set; }
        /// <summary>
        /// ѧ������
        /// </summary>
        /// <returns></returns>
        [Column("DEGREESID")]
        public string DEGREESID { get; set; }
        /// <summary>
        /// ѧ��
        /// </summary>
        /// <returns></returns>
        [Column("DEGREES")]
        public string DEGREES { get; set; }
        /// <summary>
        /// ���֤��
        /// </summary>
        /// <returns></returns>
        [Column("IDENTIFYID")]
        public string IDENTIFYID { get; set; }
        /// <summary>
        /// �����λ����
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTCODE")]
        public string OUTPROJECTCODE { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZECODE")]
        public string ORGANIZECODE { get; set; }
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
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// �����û�����
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// �޸�����
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// �޸��û�����
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// �޸��û�
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
        /// ��Ա���Id
        /// </summary>
        /// <returns></returns>
        [Column("PEOPLEREVIEWID")]
        public string PEOPLEREVIEWID { get; set; }
        [Column("WORKOFTYPE")]
        public string WORKOFTYPE { get; set; }
        [Column("WORKYEAR")]
        public string WORKYEAR { get; set; }
        [Column("STATEOFHEALTH")]
        public string STATEOFHEALTH { get; set; }
        [Column("DESCRIPTION")]
        public string DESCRIPTION { get; set; }
        /// <summary>
        /// �Ƿ�������
        /// </summary>
        [Column("ISFOURPERSON")]
        public string ISFOURPERSON { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        [Column("FOURPERSONTYPE")]
        public string FOURPERSONTYPE { get; set; }

        /// <summary>
        /// �˺�
        /// </summary>
        [Column("ACCOUNTS")]
        public string ACCOUNTS { get; set; }
        ///// <summary>
        ///// �Ƿ���ͬ�� 1 ͬ�� 0 δͬ��
        ///// </summary>
        //public string ISSYNCH { get; set; }
       
        /// <summary>
        /// ����֢����
        /// </summary>
        [Column("COMTRAINDICATIONNAME")]
        public string ComtraindicationName { get; set; }
        /// <summary>
        /// �Ƿ��д���ְҵ�Ľ���֢
        /// </summary>
        [Column("ISCOMTRAINDICATION")]
        public string IsComtraindication { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        [Column("PHYSICALTIME")]
        public DateTime? PhysicalTime { get; set; }
        /// <summary>
        /// ��쵥λ
        /// </summary>
        [Column("PHYSICALUNIT")]
        public string PhysicalUnit { get; set; }
        /// <summary>
        /// �˺���Դ��0:���أ�1:LDAP��
        /// </summary>
        [Column("ACCOUNTTYPE")]
        public string AccountType { get; set; }
        /// <summary>
        /// �Ƿ�����ldap�˺ţ�0:���ǣ�1:�ǣ�
        /// </summary>
        [Column("ISAPPLICATIONLDAP")]
        public string IsApplicationLdap { get; set; }

        /// <summary>
        /// �Ƿ��� ��0:���ǣ�1:�ǣ�
        /// </summary>
        [Column("ISOVERAGE")]
        public string IsOverAge { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Column("AGE")]
        public string Age { get; set; }

        /// <summary>
        ///ʡ��
        /// </summary>		
        [Column("QUICKQUERY")]
        public string QuickQuery { get; set; }
        /// <summary>
        /// ʡ�ݱ���
        /// </summary>		
        [Column("SIMPLESPELLING")]
        public string SimpleSpelling { get; set; }

        /// <summary>
        /// �б���
        /// </summary>		
        [Column("MANAGERID")]
        public string ManagerId { get; set; }
        /// <summary>
        /// ��
        /// </summary>	
        [Column("MANAGER")]
        public string Manager { get; set; }

        /// <summary>
        /// ��
        /// </summary>		
        [Column("DISTRICT")]
        public string District { get; set; }
        /// <summary>
        /// ������
        /// </summary>		
        [Column("DISTRICTCODE")]
        public string DistrictCode { get; set; }
        /// <summary>
        /// �ֵ�
        /// </summary>		
        [Column("STREET")]
        public string Street { get; set; }
        /// <summary>
        /// ��ϸ��ַ
        /// </summary>		
        [Column("ADDRESS")]
        public string Address { get; set; }
        /// <summary>
        /// �ù���ʽ
        /// </summary>
        [Column("WORKERTYPE")]
        public string WorkerType { get; set; }

        /// <summary>
        /// רҵ����
        /// </summary>
        [Column("SPECIALTYTYPE")]
        public string SpecialtyType { get; set; }

        /// <summary>
        /// ��Ա�����Ƿ��ύ 0��δ�ύ   1���Ѿ��ύ    Ĭ��Ϊ0   
        /// </summary>
        [Column("SUBMITSTATE")]
        public int? SubmitState { get; set; }
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
    public class PhyInfoEntity {
        /// <summary>
        /// ��쵥λ
        /// </summary>
        public string PhysicalUnit { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        public DateTime? PhysicalTime { get; set; }
        /// <summary>
        /// �Ƿ��д���ְҵ�Ľ���֢
        /// </summary>
        public string IsComtraindication { get; set; }
        /// <summary>
        /// ����֢����
        /// </summary>
        public string ComtraindicationName { get; set; }
        /// <summary>
        /// ��Ҫ���µ���Աid����
        /// </summary>
        public string ids { get; set; }
    }
}