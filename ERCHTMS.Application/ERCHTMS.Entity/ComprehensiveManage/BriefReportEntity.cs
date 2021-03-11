using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.ComprehensiveManage
{
    /// <summary>
    /// �� ����֪ͨ����
    /// </summary>
    [Table("HRS_BRIEFREPORT")]
    public class BriefReportEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// ��ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("REPORTDATE")]
        public string ReportDate { get; set; }
        /// <summary>
        /// ���Ķ���ID���ϣ�
        /// </summary>
        /// <returns></returns>
        [Column("READUSERIDLIST")]
        public string ReadUserIdList { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("ISSUERNAME")]
        public string IssuerName { get; set; }
        /// <summary>
        /// ������Χ���������������ϣ�
        /// </summary>
        /// <returns></returns>
        [Column("ISSUERUSERNAMELIST")]
        public string IssuerUserNameList { get; set; }
        /// <summary>
        /// ���Ķ����������ϣ�
        /// </summary>
        /// <returns></returns>
        [Column("READUSERNAMELIST")]
        public string ReadUserNameList { get; set; }
        /// <summary>
        /// �򱨲�������
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ISSUETIME")]
        public DateTime? IssueTime { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
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
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
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
        [Column("PERIODS")]
        public string Periods { get; set; }
        /// <summary>
        /// �Ƿ���
        /// </summary>
        [Column("ISSEND")]
        public string IsSend { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
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