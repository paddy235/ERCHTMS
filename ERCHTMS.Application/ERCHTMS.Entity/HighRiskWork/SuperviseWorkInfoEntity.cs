using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// �� ������վ�ල��ҵ��Ϣ
    /// </summary>
    [Table("BIS_SUPERVISEWORKINFO")]
    public class SuperviseWorkInfoEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// �����û��������ű���
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �����û�id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// �޸��û�id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKCONTENT")]
        public string WorkContent { get; set; }
        /// <summary>
        /// ��ҵ��λ
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTID")]
        public string WorkDeptId { get; set; }
        /// <summary>
        /// �����û�������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGID")]
        public string EngineeringId { get; set; }
        /// <summary>
        /// ��ҵ�ص�
        /// </summary>
        /// <returns></returns>
        [Column("WORKPLACE")]
        public string WorkPlace { get; set; }
        /// <summary>
        /// ��ҵ��λ
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTNAME")]
        public string WorkDeptName { get; set; }
        /// <summary>
        /// ��ҵ���
        /// </summary>
        /// <returns></returns>
        [Column("WORKINFOTYPE")]
        public string WorkInfoType { get; set; }
        /// <summary>
        /// ��ҵ���id
        /// </summary>
        /// <returns></returns>
        [Column("WORKINFOTYPEID")]
        public string WorkInfoTypeId { get; set; }
        /// <summary>
        /// ��ҵ��λ
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTCODE")]
        public string WorkDeptCode { get; set; }
        /// <summary>
        /// ��ҵ��λ���(0:��λ�ڲ� 1:�����λ)
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTTYPE")]
        public string WorkDeptType { get; set; }
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
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �ල����id
        /// </summary>
        /// <returns></returns>
        [Column("TASKSHAREID")]
        public string TaskShareId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGNAME")]
        public string EngineeringName { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKNAME")]
        public string WorkName { get; set; }
        /// <summary>
        /// ��ҵ��ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("WORKSTARTTIME")]
        public DateTime? WorkStartTime { get; set; }
        /// <summary>
        /// ��ҵ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("WORKENDTIME")]
        public DateTime? WorkEndTime { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREANAME")]
        public string WorkAreaName { get; set; }
        /// <summary>
        /// ��ҵ����code
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREACODE")]
        public string WorkAreaCode { get; set; }
        /// <summary>
        /// ����Ʊ��
        /// </summary>
        /// <returns></returns>
        [Column("WORKTICKETNO")]
        public string WorkTicketNo { get; set; }

        /// <summary>
        /// ��ҵ��Ա
        /// </summary>
        /// <returns></returns>
        [Column("WORKUSERIDS")]
        public string WorkUserIds { get; set; }

        /// <summary>
        /// ��ҵ��Ա
        /// </summary>
        /// <returns></returns>
        [Column("WORKUSERNAMES")]
        public string WorkUserNames { get; set; }
        
        /// <summary>
        /// �ֶ��������ҵ���
        /// </summary>
        /// <returns></returns>
        [Column("HANDTYPE")]
        public string HandType { get; set; }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKPROJECTNAME")]
        public string WorkProjectName { get; set; }

        
        #region ����վ�ලԱ�ɸ���ҵ���
        /// <summary>
        /// ��ҵ���
        /// </summary>
        /// <returns></returns>
        [Column("WORKINFOTYPE1")]
        public string WorkInfoType1 { get; set; }
        /// <summary>
        /// ��ҵ���id
        /// </summary>
        /// <returns></returns>
        [Column("WORKINFOTYPEID1")]
        public string WorkInfoTypeId1 { get; set; }

        /// <summary>
        /// �ֶ��������ҵ���
        /// </summary>
        /// <returns></returns>
        [Column("HANDTYPE1")]
        public string HandType1 { get; set; }
        #endregion

        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            //this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            //this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            //this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            //this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
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