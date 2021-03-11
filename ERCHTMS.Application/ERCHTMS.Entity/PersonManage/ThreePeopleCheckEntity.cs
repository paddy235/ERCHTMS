using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.PersonManage
{
    /// <summary>
    /// �� ��������������ҵ���
    /// </summary>
    [Table("BIS_THREEPEOPLECHECK")]
    public class ThreePeopleCheckEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����״̬
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public string Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// ���̽ڵ�
        /// </summary>
        /// <returns></returns>
        [Column("NODEID")]
        public string NodeId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPT")]
        public string BelongDept { get; set; }
        /// <summary>
        /// �������ű���
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTCODE")]
        public string BelongDeptCode { get; set; }
        /// <summary>
        /// ��������ID
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTID")]
        public string BelongDeptId { get; set; }
        /// <summary>
        /// �����Ƿ����
        /// </summary>
        /// <returns></returns>
        [Column("ISOVER")]
        public int? IsOver { get; set; }
        /// <summary>
        /// ���뵥λ���ͣ��ڲ����ⲿ��
        /// </summary>
        /// <returns></returns>
        [Column("APPLYTYPE")]
        public string ApplyType { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTNAME")]
        public string ProjectName { get; set; }
        /// <summary>
        /// ����Id 
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTID")]
        public string ProjectId { get; set; }

        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ������������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// �������������ű���
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// ��������������Id
        /// <returns></returns>
        [Column("CREATEUSERDEPTID")]
        public string CreateUserDeptId { get; set; }
        /// <summary>
        /// �Ƿ��ύ��0:����,1:�ύ��
        /// </summary>
        /// <returns></returns>
        [Column("ISSUMBIT")]
        public int? IsSumbit { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// �������˺�
        /// </summary>
        /// <returns></returns>
        [Column("USERACCOUNT")]
        public string UserAccount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYSNO")]
        public string ApplySno { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
           this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
            var user= OperatorProvider.Provider.Current();
            if (user!=null)
            {
                this.CreateUserId = user.UserId;
                this.CreateUserName = user.UserName;
                this.CreateUserDeptCode = user.DeptCode;
                this.CreateUserOrgCode = user.OrganizeCode;
                this.UserAccount = user.Account;
                CreateUserDeptId = user.DeptId;
            }
            CreateTime = DateTime.Now;
            this.IsOver = 0;
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