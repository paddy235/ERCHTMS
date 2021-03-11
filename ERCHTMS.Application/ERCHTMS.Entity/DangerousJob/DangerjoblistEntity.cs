using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.DangerousJob
{
    /// <summary>
    /// �� ����Σ����ҵ�嵥
    /// </summary>
    [Table("BIS_DANGERJOBLIST")]
    public class DangerjoblistEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// �����û�ID
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
        /// �����û�����
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �޸��û�ID
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �޸ļ�¼ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// �޸��û�����
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
        /// Σ����ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("DANGERJOBNAME")]
        public string DangerJobName { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("NUMBEROFPEOPLE")]
        public int? NumberofPeople { get; set; }

        /// <summary>
        /// ��ҵ����Text
        /// </summary>
        /// <returns></returns>
        [Column("NUMBEROFPEOPLENAME")]
        public string NumberofPeopleName { get; set; }

        /// <summary>
        /// ��ҵ��λ����
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODES")]
        public string DeptCodes { get; set; }

        /// <summary>
        /// ��ҵ��λID
        /// </summary>
        /// <returns></returns>
        [Column("DEPTIDS")]
        public string DeptIds { get; set; }
        
        /// <summary>
        /// ��ҵ��λ
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAMES")]
        public string DeptNames { get; set; }
        /// <summary>
        /// ��ҵƵ��
        /// </summary>
        /// <returns></returns>
        [Column("JOBFREQUENCY")]
        public string JobFrequency { get; set; }
        /// <summary>
        /// ���ڵ�Σ������
        /// </summary>
        /// <returns></returns>
        [Column("DANGERFACTORS")]
        public string DangerFactors { get; set; }
        /// <summary>
        /// ���ܷ������¹����
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTCATEGORIES")]
        public string AccidentCategories { get; set; }
        /// <summary>
        /// ��ȫ��ʩ
        /// </summary>
        /// <returns></returns>
        [Column("SAFETYMEASURES")]
        public string SafetyMeasures { get; set; }
        /// <summary>
        /// Σ����ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("JOBLEVEL")]
        public string JobLevel { get; set; }

        /// <summary>
        /// Σ����ҵ����Text
        /// </summary>
        /// <returns></returns>
        [Column("JOBLEVELNAME")]
        public string JobLevelName { get; set; }
        /// <summary>
        /// �ֳ��໤������
        /// </summary>
        /// <returns></returns>
        [Column("PRINCIPALIDS")]
        public string PrincipalIds { get; set; }
        /// <summary>
        /// �ֳ��໤������
        /// </summary>
        /// <returns></returns>
        [Column("PRINCIPALNAMES")]
        public string PrincipalNames { get; set; }

        /// <summary>
        /// ǩ����ַ
        /// </summary>
        /// <returns></returns>
        [Column("SIGNATUREPATH")]
        public string SignaturePath { get; set; }

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