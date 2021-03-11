using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// �� ������ʷ
    /// </summary>
    [Table("EPG_HISTORYDAILYEXAMINE")]
    public class HistorydailyexamineEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// ���˽��
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEMONEY")]
        public double? ExamineMoney { get; set; }
        /// <summary>
        /// ���˱��
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINECODE")]
        public string ExamineCode { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEPERSONID")]
        public string ExaminePersonId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINECONTENT")]
        public string ExamineContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINETYPE")]
        public string ExamineType { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("CONTRACTID")]
        public string ContractId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINETIME")]
        public DateTime? ExamineTime { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEPERSON")]
        public string ExaminePerson { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// �����˲���ID
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINETODEPTID")]
        public string ExamineToDeptId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINEBASIS")]
        public string ExamineBasis { get; set; }
        /// <summary>
        /// �����˲���
        /// </summary>
        /// <returns></returns>
        [Column("EXAMINETODEPT")]
        public string ExamineToDept { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }

        /// <summary>
        /// ���̲�������
        /// </summary>
        [Column("FLOWDEPTNAME")]
        public string FlowDeptName { get; set; }

        /// <summary>
        /// ���̲��ű���/ID
        /// </summary>
        [Column("FLOWDEPT")]
        public string FlowDept { get; set; }

        /// <summary>
        /// ���̽�ɫ����
        /// </summary>
        [Column("FLOWROLENAME")]
        public string FlowRoleName { get; set; }

        /// <summary>
        /// ���̽�ɫ����/ID
        /// </summary>
        [Column("FLOWROLE")]
        public string FlowRole { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Column("FLOWNAME")]
        public string FlowName { get; set; }

        /// <summary>
        /// ���˲���
        /// </summary>
        [Column("EXAMINEDEPT")]
        public string ExamineDept { get; set; }

        /// <summary>
        /// ���˲���ID
        /// </summary>
        [Column("EXAMINEDEPTID")]
        public string ExamineDeptId { get; set; }

        /// <summary>
        /// �����˲���ID
        /// </summary>
        [Column("CREATEUSERDEPTID")]
        public string CreateUserDeptId { get; set; }
        /// <summary>
        /// �Ƿ񱣴�ɹ�
        /// </summary>
        [Column("ISSAVED")]
        public int? IsSaved { get; set; }

        /// <summary>
        /// �����Ƿ����
        /// </summary>
        [Column("ISOVER")]
        public int? IsOver { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        [Column("PROJECT")]
        public string Project { get; set; }
        [Column("PROJECTID")]
        public string ProjectId { get; set; }
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
            this.CreateUserDeptId = OperatorProvider.Provider.Current().DeptId;


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