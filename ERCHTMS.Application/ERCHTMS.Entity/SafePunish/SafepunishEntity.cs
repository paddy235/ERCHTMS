using System;
using System.ComponentModel;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SafePunish
{
    /// <summary>
    /// �� ������ȫ�ͷ�
    /// </summary>
    [Table("BIS_SAFEPUNISH")]
    public class SafepunishEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
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
        /// �ͷ����
        /// </summary>
        /// <returns></returns>
        [Column("SAFEPUNISHCODE")]
        public string SafePunishCode { get; set; }
        /// <summary>
        /// ������id
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERID")]
        public string ApplyUserId { get; set; }
        /// <summary>
        /// ������name
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERNAME")]
        public string ApplyUserName { get; set; }

        /// <summary>
        /// �����˲���id
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERDEPTID")]
        public string ApplyUserDeptId { get; set; }


        /// <summary>
        /// �����˲���name
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERDEPTNAME")]
        public string ApplyUserDeptName { get; set; }

        /// <summary>
        /// �ͷ�ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("APPLYTIME")]
        public DateTime? ApplyTime { get; set; }
        /// <summary>
        /// �ͷ��ܽ��
        /// </summary>
        /// <returns></returns>
        [Column("APPLYPUNISHRMB")]
        public string ApplyPunishRmb { get; set; }
        /// <summary>
        /// �ͷ��ܻ���
        /// </summary>
        /// <returns></returns>
        [Column("APPLYPUNISHSCORE")]
        public string ApplyPunishScore { get; set; }

        /// <summary>
        /// �¼�����
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHTYPE")]
        public string PunishType { get; set; }

        /// <summary>
        /// �ͷ�����
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHOBJECTNAMES")]
        public string PunishObjectNames { get; set; }
        
        /// <summary>
        /// �ͷ����1���¹��¼���2��������
        /// </summary>
        /// <returns></returns>
        [Column("AMERCETYPE")]
        public string AmerceType { get; set; }


        /// <summary>
        /// �����˵�λid
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTID")]
        public string ApplyDeptId { get; set; }
   
        /// <summary>
        /// �����˵�λcode
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTCODE")]
        public string ApplyDeptCode { get; set; }
    
        /// <summary>
        /// �����˵�λname
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTNAME")]
        public string ApplyDeptName { get; set; }


        /// <summary>
        /// ��������Աid
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHUSERID")]
        public string PunishUserId { get; set; }
     
	
        /// <summary>
        /// ��������Աname
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHUSERNAME")]
        public string PunishUserName { get; set; }

        /// <summary>
        /// ���˽��
        /// </summary>
        /// <returns></returns>
        [Column("AMERCEAMOUNT")]
        public string AmerceAmount { get; set; }



        /// <summary>
        /// �����λ����
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHDEPTNAME")]
        public string PunishDeptName { get; set; }
        /// <summary>
        /// �����λ����
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHDEPTID")]
        public string PunishDeptId { get; set; }
        /// <summary>
        /// �ͷ���������
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHREMARK")]
        public string PunishRemark { get; set; }
        /// <summary>
        /// ����״̬(0���ڴ���.,1.�Ѵ���,2.δ����)
        /// </summary>
        /// <returns></returns>
        [Column("FLOWSTATE")]
        public string FlowState { get; set; }
        /// <summary>
        /// ����״̬(1.רҵ���,2.�������)
        /// </summary>
        /// <returns></returns>
        [Column("APPLYSTATE")]
        public string ApplyState { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("APPROVERPEOPLEIDS")]
        public string ApproverPeopleIds { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("APPROVERPEOPLENAMES")]
        public string ApproverPeopleNames { get; set; }



        /// <summary>
        /// רҵ����
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYMANAGERID")]
        public string SpecialtyManagerId { get; set; }


        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("DEPTMANAGERID")]
        public string DeptManagerId { get; set; }

        /// <summary>
        /// רҵ������id
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYPRINCIPALID")]
        public string SpecialtyPrincipalId { get; set; }

        /// <summary>
        /// רҵ������name
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYPRINCIPALNAME")]
        public string SpecialtyPrincipalName { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Column("BELONGDEPT")]
        public string BelongDept { get; set; }


        /// <summary>
        /// ��������ID
        /// </summary>
        [Column("BELONGDEPTID")]
        public string BelongDeptId { get; set; }

        /// <summary>
        /// ��������    
        /// </summary>
        [Column("EXAMINETYPE")]
        public string ExamineType { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Column("PUNISHACCORD")]
        public string PunishAccord { get; set; }

        /// <summary>
        /// רҵ���
        /// </summary>
        [Column("SPECIALTYOPINION")]
        public string SpecialtyOpinion { get; set; }

        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            if (string.IsNullOrEmpty( this.ApplyPunishRmb))
            {
                this.ApplyPunishRmb = "0";
            }
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
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
            if (string.IsNullOrEmpty(this.ApplyPunishRmb))
            {
                this.ApplyPunishRmb = "0";
            }
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}