using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// �� ��������������
    /// </summary>
    [Table("BIS_TEAMSINFO")]
    public class TeamsInfoEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// �����û�id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �޸��û�id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// �����û��������ű���
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// �����û�������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// ����name
        /// </summary>
        /// <returns></returns>
        [Column("TEAMNAME")]
        public string TeamName { get; set; }
        /// <summary>
        /// ����code
        /// </summary>
        /// <returns></returns>
        [Column("TEAMCODE")]
        public string TeamCode { get; set; }
        /// <summary>
        /// ����id
        /// </summary>
        /// <returns></returns>
        [Column("TEAMID")]
        public string TeamId { get; set; }
        /// <summary>
        /// ��վ���鿪ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("TEAMSTARTTIME")]
        public DateTime? TeamStartTime { get; set; }
        /// <summary>
        /// ��վ�������ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("TEAMENDTIME")]
        public DateTime? TeamEndTime { get; set; }
        /// <summary>
        /// ������ҵ��Ϣ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKINFONAME")]
        public string WorkInfoName { get; set; }
        /// <summary>
        /// ������ҵ��Ϣid
        /// </summary>
        /// <returns></returns>
        [Column("WORKINFOID")]
        public string WorkInfoId { get; set; }
        /// <summary>
        /// �������id
        /// </summary>
        /// <returns></returns>
        [Column("TASKSHAREID")]
        public string TaskShareId { get; set; }

        /// <summary>
        /// �Ƿ��ύ(0:�� 1:��)
        /// </summary>
        /// <returns></returns>
        [Column("DATAISSUBMIT")]
        public string DataIsSubmit { get; set; }

        /// <summary>
        /// �Ƿ��������(������ɲ������Ͱ���)
        /// </summary>
        /// <returns></returns>
        [Column("ISACCOMPLISH")]
        public string IsAccomplish { get; set; }
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
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}