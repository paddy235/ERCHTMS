using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;

namespace ERCHTMS.Entity.PowerPlantInside
{
    /// <summary>
    /// �� �����¹��¼���������
    /// </summary>
    [Table("BIS_POWERPLANTREFORM")]
    public class PowerplantreformEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONPERSONID")]
        public string RectificationPersonId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// ���Ĵ�ʩ
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONMEASURES")]
        public string RectificationMeasures { get; set; }
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
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �������β���ID
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONDUTYDEPTID")]
        public string RectificationDutyDeptId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// ������ǩ��
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONPERSONSIGNIMG")]
        public string RectificationPersonSignImg { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONTIME")]
        public DateTime? RectificationTime { get; set; }
        /// <summary>
        /// �����������
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONSITUATION")]
        public string RectificationSituation { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONPERSON")]
        public string RectificationPerson { get; set; }
        /// <summary>
        /// �������β���
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONDUTYDEPT")]
        public string RectificationDutyDept { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �������ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONENDTIME")]
        public DateTime? RectificationEndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }

        /// <summary>
        /// �����¹��¼������¼ID
        /// </summary>
        [Column("POWERPLANTHANDLEID")]
        public string PowerPlantHandleId { get; set; }

        /// <summary>
        /// �����¹��¼������¼��ϸID
        /// </summary>
        [Column("POWERPLANTHANDLEDETAILID")]
        public string PowerPlantHandleDetailId { get; set; }

        /// <summary>
        /// �Ƿ�ʧЧ 0:��Ч 1:ʧЧ
        /// </summary>
        [Column("DISABLE")]
        public int? Disable { get; set; }

        [NotMapped]
        public IList<Photo> filelist { get; set; } //����

        /// <summary>
        /// ԭ�򼰱�¶����
        /// </summary>
        [Column("REASONANDPROBLEM")]
        public string ReasonAndProblem { get; set; }

        /// <summary>
        /// ǩ�ղ���
        /// </summary>
        [Column("SIGNDEPTNAME")]
        public string SignDeptName { get; set; }

        /// <summary>
        /// ǩ�ղ���
        /// </summary>
        [Column("SIGNDEPTID")]
        public string SignDeptId { get; set; }

        /// <summary>
        /// ǩ����
        /// </summary>
        [Column("SIGNPERSONNAME")]
        public string SignPersonName { get; set; }

        /// <summary>
        /// ǩ����
        /// </summary>
        [Column("SIGNPERSONID")]
        public string SignPersonId { get; set; }

        /// <summary>
        /// �Ƿ�ָ��������
        /// </summary>
        [Column("ISASSIGNPERSON")]
        public string IsAssignPerson { get; set; }

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