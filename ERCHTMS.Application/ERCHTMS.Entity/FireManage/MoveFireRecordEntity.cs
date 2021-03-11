using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.FireManage
{
    /// <summary>
    /// �� ���������¼���ص����λ�ӱ�
    /// </summary>
    [Table("HRS_MOVEFIRERECORD")]
    public class MoveFireRecordEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("MAINID")]
        public string MainId { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AutoId { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// ����Ʊ��
        /// </summary>
        /// <returns></returns>
        [Column("WORKTICKET")]
        public string WorkTicket { get; set; }
        /// <summary>
        /// ������λ
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNIT")]
        public string WorkUnit { get; set; }
        /// <summary>
        /// ������λCode
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNITCODE")]
        public string WorkUnitCode { get; set; }
        /// <summary>
        /// �����ص�
        /// </summary>
        /// <returns></returns>
        [Column("WORKSITE")]
        public string WorkSite { get; set; }
        /// <summary>
        /// ִ�п�ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("EXECUTESTARTDATE")]
        public DateTime? ExecuteStartDate { get; set; }
        /// <summary>
        /// ִ�н���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("EXECUTEENDDATE")]
        public DateTime? ExecuteEndDate { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSER")]
        public string DutyUser { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        /// ִ����
        /// </summary>
        /// <returns></returns>
        [Column("EXECUTEUSER")]
        public string ExecuteUser { get; set; }
        /// <summary>
        /// ִ����ID
        /// </summary>
        /// <returns></returns>
        [Column("EXECUTEUSERID")]
        public string ExecuteUserId { get; set; }
        /// <summary>
        /// �Ǽ���
        /// </summary>
        /// <returns></returns>
        [Column("REGISTERUSER")]
        public string RegisterUser { get; set; }
        /// <summary>
        /// �Ǽ���ID
        /// </summary>
        /// <returns></returns>
        [Column("REGISTERUSERID")]
        public string RegisterUserId { get; set; }
        /// <summary>
        /// �Ǽ�ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("REGISTERDATE")]
        public DateTime? RegisterDate { get; set; }
        /// <summary>
        /// ����������ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("WORKENDDATE")]
        public DateTime? WorkEndDate { get; set; }
        /// <summary>
        /// �Ǽ���(������������Ϣ�ĵǼ���)
        /// </summary>
        /// <returns></returns>
        [Column("WORKREGISTERUSER")]
        public string WorkRegisterUser { get; set; }
        /// <summary>
        /// �Ǽ���ID
        /// </summary>
        /// <returns></returns>
        [Column("WORKREGISTERUSERID")]
        public string WorkRegisterUserId { get; set; }
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
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}