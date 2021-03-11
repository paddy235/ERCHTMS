using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.PowerPlantInside
{
    /// <summary>
    /// �� �����¹��¼�����
    /// </summary>
    [Table("BIS_POWERPLANTCHECK")]
    public class PowerplantcheckEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("AUDITPEOPLEID")]
        public string AuditPeopleId { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("AUDITPEOPLE")]
        public string AuditPeople { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("AUDITTIME")]
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// ���ղ���ID
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDEPTID")]
        public string AuditDeptId { get; set; }
        /// <summary>
        /// ���ղ���
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDEPT")]
        public string AuditDept { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("AUDITOPINION")]
        public string AuditOpinion { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// ����ǩ��
        /// </summary>
        /// <returns></returns>
        [Column("AUDITSIGNIMG")]
        public string AuditSignImg { get; set; }
        /// <summary>
        /// �Ƿ�ʧЧ 0:��Ч 1:ʧЧ
        /// </summary>
        /// <returns></returns>
        [Column("DISABLE")]
        public int? Disable { get; set; }
        /// <summary>
        /// �������Id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }
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
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// �����¹��¼������¼ID
        /// </summary>
        /// <returns></returns>
        [Column("POWERPLANTHANDLEID")]
        public string PowerPlantHandleId { get; set; }
        /// <summary>
        /// �����¹��¼������¼��ϸID
        /// </summary>
        /// <returns></returns>
        [Column("POWERPLANTHANDLEDETAILID")]
        public string PowerPlantHandleDetailId { get; set; }
        /// <summary>
        /// �����¹��¼�����ID
        /// </summary>
        /// <returns></returns>
        [Column("POWERPLANTREFORMID")]
        public string PowerPlantReformId { get; set; }
        /// <summary>
        /// ���ս��
        /// </summary>
        /// <returns></returns>
        [Column("AUDITRESULT")]
        public int? AuditResult { get; set; }

        [NotMapped]
        public IList<Photo> filelist { get; set; } //����
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

    //�ļ�ʵ��
    public class Photo
    {
        public string fileid { get; set; }
        public string filename { get; set; }
        public string fileurl { get; set; }

        public string folderid { get; set; }
    }
}