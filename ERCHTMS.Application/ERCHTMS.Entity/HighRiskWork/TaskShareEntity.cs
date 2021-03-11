using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// �� ������������
    /// </summary>
    [Table("BIS_TASKSHARE")]
    public class TaskShareEntity : BaseEntity
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
        [Column("PROJECTNAME")]
        public string ProjectName { get; set; }
        /// <summary>
        /// ��ҵ��λname
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// ���̿���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTWORKTIME")]
        public DateTime? ProjectWorkTime { get; set; }
        /// <summary>
        /// ������վ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("DEPTENDTIME")]
        public DateTime? DeptEndTime { get; set; }
        /// <summary>
        /// ��վ�ල����name
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISEDEPTNAME")]
        public string SuperviseDeptName { get; set; }
        /// <summary>
        /// ��ҵ��λid
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }

        /// <summary>
        /// ������վ��ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("DEPTSTARTTIME")]
        public DateTime? DeptStartTime { get; set; }
        /// <summary>
        /// ��ҵ��λcode
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// ��վ�ල����code
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISEDEPTCODE")]
        public string SuperviseDeptCode { get; set; }
        /// <summary>
        /// ����id
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTID")]
        public string ProjectId { get; set; }
        /// <summary>
        /// ��������(0:������վ���� 1:������վ���� 2:��Ա��վ����)
        /// </summary>
        /// <returns></returns>
        [Column("TASKTYPE")]
        public string TaskType { get; set; }
        /// <summary>
        /// ��վ�ල����id
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISEDEPTID")]
        public string SuperviseDeptId { get; set; }
        /// <summary>
        /// ��ҵ��λ���(0:��λ�ڲ� 1:�����λ)
        /// </summary>
        /// <returns></returns>
        [Column("DEPTTYPE")]
        public string DeptType { get; set; }

        /// <summary>
        /// ���̲���(0:�������ŷ����� 1:���ŷ����� 2:���������,3:����ɷ���)
        /// </summary>
        /// <returns></returns>
        [Column("FLOWSTEP")]
        public string FlowStep { get; set; }

        /// <summary>
        /// �Ƿ��ύ(0:δ�ύ 1:���ύ)
        /// </summary>
        /// <returns></returns>
        [Column("ISSUBMIT")]
        public string IsSubmit { get; set; }

        /// <summary>
        /// ���̽�ɫ����
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLENAME")]
        public string FlowRoleName { get; set; }

        /// <summary>
        /// ���̲��ű���/ID 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPT")]
        public string FlowDept { get; set; }

        /// <summary>
        /// ���̲�������
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPTNAME")]
        public string FlowDeptName { get; set; }

        /// <summary>
        /// ��ҵ��Ϣ
        /// </summary>
        public List<SuperviseWorkInfoEntity> WorkSpecs { get; set; }

        /// <summary>
        /// �������������Ϣ
        /// </summary>
        public List<TeamsInfoEntity> TeamSpec { get; set; }

        /// <summary>
        /// ��Ա���������Ϣ
        /// </summary>
        public List<StaffInfoEntity> StaffSpec { get; set; }


        /// <summary>
        /// ɾ����Աids
        /// </summary>
        public string DelIds { get; set; }

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