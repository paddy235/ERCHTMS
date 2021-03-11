using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
 
namespace ERCHTMS.Entity.ComprehensiveManage
{
    /// <summary>
    /// �� ����sdf
    /// </summary>
    [Table("HRS_MEETINGRECORD")]
    public class MeetingRecordEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// ��Ҫ������Ա
        /// </summary>
        /// <returns></returns>
        [Column("SETTLEPERSON")]
        public string SettlePerson { get; set; }
        /// <summary>
        /// ��ַ
        /// </summary>
        /// <returns></returns>
        [Column("ADDRESS")]
        public string Address { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("CONTENT")]
        public string Content { get; set; }
        /// <summary>
        /// �Ƿ��� 0 �� 1 ��
        /// </summary>
        /// <returns></returns>
        [Column("ISSEND")]
        public string IsSend { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ���Ķ���ID���ϣ�
        /// </summary>
        /// <returns></returns>
        [Column("READUSERIDLIST")]
        public string ReadUserIdList { get; set; }
        /// <summary>
        /// ������Χ��������ID���ϣ�
        /// </summary>
        /// <returns></returns>
        [Column("ISSUERUSERIDLIST")]
        public string IssuerUserIdList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// �λ���Ա
        /// </summary>
        /// <returns></returns>
        [Column("ATTENDPERSON")]
        public string AttendPerson { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("DIRECT")]
        public string Direct { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MEETINGTIME")]
        public DateTime? MeetingTime { get; set; }
        /// <summary>
        /// ���Ķ����������ϣ�
        /// </summary>
        /// <returns></returns>
        [Column("READUSERNAMELIST")]
        public string ReadUserNameList { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ISSUETIME")]
        public DateTime? IssueTime { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        /// <returns></returns>
        [Column("CODE")]
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �λ�����
        /// </summary>
        /// <returns></returns>
        [Column("PERSONNUM")]
        public string PersonNum { get; set; }
        /// <summary>
        /// ������Χ���������������ϣ�
        /// </summary>
        /// <returns></returns>
        [Column("ISSUERUSERNAMELIST")]
        public string IssuerUserNameList { get; set; }
        /// <summary>
        /// �ܼ�
        /// </summary>
        /// <returns></returns>
        [Column("SECURITY")]
        public string Security { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
        }
        #endregion
    }
}
