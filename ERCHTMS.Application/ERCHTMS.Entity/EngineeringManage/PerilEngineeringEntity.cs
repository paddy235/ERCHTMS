using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EngineeringManage
{
    /// <summary>
    /// �� ����Σ�󹤳̹���
    /// </summary>
    [Table("BIS_PERILENGINEERING")]
    public class PerilEngineeringEntity : BaseEntity
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
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGNAME")]
        public string EngineeringName { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGTYPE")]
        public string EngineeringType { get; set; }
        /// <summary>
        /// ���̷��յ�
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGSPOT")]
        public string EngineeringSpot { get; set; }
        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ESTARTTIME")]
        public DateTime? EStartTime { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("EFINISHTIME")]
        public DateTime? EFinishTime { get; set; }
        /// <summary>
        /// ��λ���
        /// </summary>
        /// <returns></returns>
        [Column("UNITTYPE")]
        public string UnitType { get; set; }
        /// <summary>
        /// ������λid
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTID")]
        public string BelongDeptId { get; set; }
        /// <summary>
        /// ������λ����
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTNAME")]
        public string BelongDeptName { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEPERSON")]
        public string ExaminePerson { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINETIME")]
        public DateTime? ExamineTime { get; set; }
        /// <summary>
        /// ʩ�������ļ�id
        /// </summary>
        /// <returns></returns>
        [Column("CONSTRUCTFILES")]
        public string ConstructFiles { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("TASKTIME")]
        public DateTime? TaskTime { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("TASKPERSON")]
        public string TaskPerson { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("TASKCONTENT")]
        public string TaskContent { get; set; }
        /// <summary>
        /// ���׸���
        /// </summary>
        /// <returns></returns>
        [Column("TASKFILES")]
        public string TaskFiles { get; set; }
        /// <summary>
        /// ��չ���
        /// </summary>
        /// <returns></returns>
        [Column("EVOLVECASE")]
        public string EvolveCase { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("WRITEDATE")]
        public DateTime? WriteDate { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("WRITEUSERNAME")]
        public string WriteUserName { get; set; }

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