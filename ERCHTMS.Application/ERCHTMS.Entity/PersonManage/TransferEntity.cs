using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.PersonManage
{
    /// <summary>
    /// �� ����ת����Ϣ��
    /// </summary>
    [Table("BIS_TRANSFER")]
    public class TransferEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("TID")]
        public string TID { get; set; }
        /// <summary>
        /// ������ID
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
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// ת�벿��ID
        /// </summary>
        /// <returns></returns>
        [Column("INDEPTID")]
        public string InDeptId { get; set; }
        /// <summary>
        /// ת�벿������
        /// </summary>
        /// <returns></returns>
        [Column("INDEPTNAME")]
        public string InDeptName { get; set; }
        /// <summary>
        /// ת�벿��Code
        /// </summary>
        /// <returns></returns>
        [Column("INDEPTCODE")]
        public string InDeptCode { get; set; }
        /// <summary>
        /// ת����������
        /// </summary>
        /// <returns></returns>
        [Column("OUTDEPTNAME")]
        public string OutDeptName { get; set; }
        /// <summary>
        /// ת������ID
        /// </summary>
        /// <returns></returns>
        [Column("OUTDEPTID")]
        public string OutDeptId { get; set; }
        /// <summary>
        /// ת������Code
        /// </summary>
        /// <returns></returns>
        [Column("OUTDEPTCODE")]
        public string OutDeptCode { get; set; }
        /// <summary>
        /// ת����ԱID
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// ת����Ա����
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
        /// <summary>
        /// ת������
        /// </summary>
        /// <returns></returns>
        [Column("TRANSFERTIME")]
        public DateTime? TransferTime { get; set; }
        /// <summary>
        /// �Ƿ���Ҫȷ�� 0����Ҫȷ�� 1��Ҫȷ�� 2��ȷ��
        /// </summary>
        /// <returns></returns>
        [Column("ISCONFIRM")]
        public int? IsConfirm { get; set; }
        /// <summary>
        /// ת��ǰְ��ID
        /// </summary>
        /// <returns></returns>
        [Column("INJOBID")]
        public string InJobId { get; set; }
        /// <summary>
        /// ת��ǰְ������
        /// </summary>
        /// <returns></returns>
        [Column("INJOBNAME")]
        public string InJobName { get; set; }
        /// <summary>
        /// ת��ְ��id
        /// </summary>
        /// <returns></returns>
        [Column("OUTJOBID")]
        public string OutJobId { get; set; }
        /// <summary>
        /// ת��ְ������
        /// </summary>
        /// <returns></returns>
        [Column("OUTJOBNAME")]
        public string OutJobName { get; set; }
        /// <summary>
        /// ת��ǰ��λID
        /// </summary>
        /// <returns></returns>
        [Column("INPOSTID")]
        public string InPostId { get; set; }
        /// <summary>
        /// ת��ǰ��λ����
        /// </summary>
        /// <returns></returns>
        [Column("INPOSTNAME")]
        public string InPostName { get; set; }
        /// <summary>
        /// ת����λID
        /// </summary>
        /// <returns></returns>
        [Column("OUTPOSTID")]
        public string OutPostId { get; set; }
        /// <summary>
        /// ת����λ����
        /// </summary>
        /// <returns></returns>
        [Column("OUTPOSTNAME")]
        public string OutPostName { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.TID = string.IsNullOrEmpty(TID) ? Guid.NewGuid().ToString() : TID;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.TID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
        }
        #endregion
    }

    public class AppInTransfer
    {
        public string info { get; set; }
        public string code { get; set; }

      
    }

    public class BzAppTransfer
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// �û�id
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// �û�����
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// ǰ����id
        /// </summary>
        public string olddepartmentid { get; set; }
        /// <summary>
        /// ǰ��������
        /// </summary>
        public string olddepartment { get; set; }
        /// <summary>
        /// ����id
        /// </summary>
        public string departmentid { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string department { get; set; }
        /// <summary>
        /// �Ƹ�ʱ��
        /// </summary>
        public string allocationtime { get; set; }
        /// <summary>
        /// ǰ��λ
        /// </summary>
        public string oldRoleDutyName { get; set; }
        /// <summary>
        /// ��λ
        /// </summary>
        public string RoleDutyName { get; set; }
        /// <summary>
        /// ǰ��λ
        /// </summary>
        public string RoleDutyId { get; set; }
        /// <summary>
        /// ְ��id
        /// </summary>
        public string quartersid { get; set; }
        /// <summary>
        /// ǰְ��
        /// </summary>
        public string oldquarters { get; set; }
        /// <summary>
        /// ְ��
        /// </summary>
        public string quarters { get; set; }
        /// <summary>
        /// �볧ʱ��
        /// </summary>
        public string leavetime { get; set; }
        /// <summary>
        /// �볧˵��
        /// </summary>
        public string leaveremark { get; set; }
        /// <summary>
        /// �Ƿ���
        /// </summary>
        public bool iscomplete { get; set; }
    }

    public class BzBase
    {
        public string userId { get; set; }

        public object data { get; set; }
    }

    /// <summary>
    /// ת�����ͬ��ʵ��
    /// </summary>
    public class BxwcTransfer
    {
        /// <summary>
        /// ��λ
        /// </summary>
        public string RoleDutyName { get; set; }
        /// <summary>
        /// ǰ��λ
        /// </summary>
        public string RoleDutyId { get; set; }
        /// <summary>
        /// ְ��id
        /// </summary>
        public string quartersid { get; set; }
        /// <summary>
        /// ְ��
        /// </summary>
        public string quarters { get; set; }
        /// <summary>
        /// ҵ��id
        /// </summary>
        public string id { get; set; }
    }
}