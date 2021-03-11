using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.KbsDeviceManage
{
    /// <summary>
    /// �� ������ҵ�ֳ���ȫ�ܿ�
    /// </summary>
    [Table("BIS_SAFEWORKCONTROL")]
    public class SafeworkcontrolEntity : BaseEntity
    {
        #region ʵ���Ա

        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }


        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// �뾶
        /// </summary>
        /// <returns></returns>
        [Column("RADIUS")]
        public int? Radius { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        /// <returns></returns>
        [Column("AREACODE")]
        public string Areacode { get; set; }
        /// <summary>
        /// ����״̬0������1Բ��2�ֻ�
        /// </summary>
        /// <returns></returns>
        [Column("AREASTATE")]
        public int? Areastate { get; set; }
        /// <summary>
        /// ʧЧʱ��
        /// </summary>
        /// <returns></returns>
        [Column("INVALIDTIME")]
        public DateTime? Invalidtime { get; set; }
        /// <summary>
        /// ��Чʱ��
        /// </summary>
        /// <returns></returns>
        [Column("TAKEEFFECTTIME")]
        public DateTime? Takeeffecttime { get; set; }
        /// <summary>
        /// ��������id
        /// </summary>
        /// <returns></returns>
        [Column("QUARANTINEID")]
        public string Quarantineid { get; set; }
        /// <summary>
        /// ��������code
        /// </summary>
        /// <returns></returns>
        [Column("QUARANTINECODE")]
        public string Quarantinecode { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [Column("QUARANTINENAME")]
        public string Quarantinename { get; set; }

        /// <summary>
        /// �ƻ���ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("PLANENSTARTTIME")]
        public DateTime? PlanenStarttime { get; set; }

        /// <summary>
        /// �ƻ�����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("PLANENDTIME")]
        public DateTime? Planendtime { get; set; }
        /// <summary>
        /// ʵ�ʿ�ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ACTUALSTARTTIME")]
        public DateTime? Actualstarttime { get; set; }
        /// <summary>
        /// ʵ�ʽ���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ACTUALENDTIME")]
        public DateTime? ActualEndTime { get; set; }
      
        /// <summary>
        /// ��ҵ��ԱId
        /// </summary>
        /// <returns></returns>
        [Column("TASKMEMBERID")]
        public string Taskmemberid { get; set; }
        /// <summary>
        /// ��ҵ��Ա����
        /// </summary>
        /// <returns></returns>
        [Column("TASKMEMBERNAME")]
        public string Taskmembername { get; set; }
        /// <summary>
        /// ��ҵ������Id
        /// </summary>
        /// <returns></returns>
        [Column("TASKMANAGEID")]
        public string Taskmanageid { get; set; }
        /// <summary>
        /// ��ҵ����������
        /// </summary>
        /// <returns></returns>
        [Column("TASKMANAGENAME")]
        public string Taskmanagename { get; set; }
        /// <summary>
        /// ����/����Id
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string Deptid { get; set; }
        /// <summary>
        /// ����/�������
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string Deptcode { get; set; }
        /// <summary>
        /// ����/��������
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string Deptname { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("TASKCONTENT")]
        public string Taskcontent { get; set; }
        /// <summary>
        /// ��ҵ����ID
        /// </summary>
        /// <returns></returns>
        [Column("TASKREGIONID")]
        public string Taskregionid { get; set; }
        /// <summary>
        /// ��ҵ�������
        /// </summary>
        /// <returns></returns>
        [Column("TASKREGIONCODE")]
        public string Taskregioncode { get; set; }
        /// <summary>
        /// ��ҵ��������
        /// </summary>
        /// <returns></returns>
        [Column("TASKREGIONNAME")]
        public string Taskregionname { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("TASKTYPE")]
        public string Tasktype { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("TASKNAME")]
        public string Taskname { get; set; }
        /// <summary>
        /// ����Ʊ���
        /// </summary>
        /// <returns></returns>
        [Column("WORKNO")]
        public string Workno { get; set; }

        /// <summary>
        /// ״̬ 0�������� 1�ύ����ʼ 2�������
        /// </summary>
        [Column("STATE")]
        public int State { get; set; }

        /// <summary>
        /// ���յȼ�
        /// </summary>
        [Column("DANGERLEVEL")]
        public string DangerLevel { get; set; }

        /// <summary>
        /// ����Χ��������ͷID
        /// </summary>
        [Column("COMERID")]
        public string comerid { get; set; }

       
        /// <summary>
        /// /����ǩ����
        /// </summary>
        [Column("ISSUENAME")]
        public string IssueName { get; set; }
        /// <summary>
        /// /����ǩ����Id
        /// </summary>
        [Column("ISSUEUSERID")]
        public string IssueUserid { get; set; }

        /// <summary>
        /// /���������
        /// </summary>
        [Column("PERMITNAME")]
        public string PermitName { get; set; }
        
        /// <summary>
        /// /���������
        /// </summary>
        [Column("PERMITUSERID")]
        public string PermitUserid { get; set; }

        /// <summary>
        /// �໤�ˣ�ʩ����λ��
        /// </summary>
        /// <returns></returns>
        [Column("GUARDIANID")]
        public string Guardianid { get; set; }
        /// <summary>
        /// �໤�ˣ�ʩ����λ��
        /// </summary>
        /// <returns></returns>
        [Column("GUARDIANNAME")]
        public string Guardianname { get; set; }

        /// <summary>
        /// �໤�ˣ����ܲ��ţ�
        /// </summary>
        [Column("EXECUTIVENAMES")]
        public string ExecutiveNames { get; set; }
        /// <summary>
        /// �໤�ˣ����ܲ��ţ�
        /// </summary>
        [Column("EXECUTIVEIDS")]
        public string ExecutiveIds { get; set; }

        /// <summary>
        /// �໤�ˣ���첿�ţ�
        /// </summary>
        [Column("SUPERVISIONNAMES")]
        public string SupervisionNames { get; set; }
        /// <summary>
        /// �໤�ˣ���첿�ţ�
        /// </summary>
        [Column("SUPERVISIONIDS")]
        public string SupervisionIds { get; set; }
         

        #endregion

        #region ��չ����
        /// <summary>
        /// ��ҵ�໤��
        /// </summary>
        [NotMapped]
        public List<SafeworkUserEntity> MonitorUsers { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
        }
        #endregion
    }

    /// <summary>
    /// �ֻ��ӿ����л�ʵ��
    /// </summary>
    public class Safeworkcontro
    {
        [NotMapped]
        public string userId { get; set; }
        public SafeworkcontrolEntity data { get; set; }
    }

}