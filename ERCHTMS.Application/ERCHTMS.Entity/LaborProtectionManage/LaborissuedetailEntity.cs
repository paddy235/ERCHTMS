using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ��������ű�����
    /// </summary>
    [Table("BIS_LABORISSUEDETAIL")]
    public class LaborissuedetailEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ʹ�ò���ID
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }
        /// <summary>
        /// ʹ�ò���Code
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// ʹ�ø�λ����
        /// </summary>
        /// <returns></returns>
        [Column("POSTNAME")]
        public string PostName { get; set; }
        /// <summary>
        /// ʹ�ø�λID
        /// </summary>
        /// <returns></returns>
        [Column("POSTID")]
        public string PostId { get; set; }
        /// <summary>
        /// Ӧ������
        /// </summary>
        /// <returns></returns>
        [Column("SHOULDNUM")]
        public int? ShouldNum { get; set; }
        /// <summary>
        /// �������ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("RECENTTIME")]
        public DateTime? RecentTime { get; set; }
        /// <summary>
        /// �´η���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("NEXTTIME")]
        public DateTime? NextTime { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ISSUENUM")]
        public int? IssueNum { get; set; }
        /// <summary>
        /// ������Ʒ��ID
        /// </summary>
        /// <returns></returns>
        [Column("INFOID")]
        public string InfoId { get; set; }
        /// <summary>
        /// ���ż�¼��ID
        /// </summary>
        /// <returns></returns>
        [Column("SUEID")]
        public string SueId { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
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
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// ��Ʒ���
        /// </summary>
        /// <returns></returns>
        [Column("NO")]
        public string No { get; set; }
        /// <summary>
        /// ��Ʒ��λ
        /// </summary>
        /// <returns></returns>
        [Column("UNIT")]
        public string Unit { get; set; }
        /// <summary>
        /// ��Ʒ�ͺ�
        /// </summary>
        /// <returns></returns>
        [Column("MODEL")]
        public string Model { get; set; }
        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        [Column("TYPE")]
        public string Type { get; set; }
        /// <summary>
        /// ʹ������ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("TIMENUM")]
        public int? TimeNum { get; set; }
        /// <summary>
        /// ʹ�����޵�λ �ꡢ�¡���
        /// </summary>
        /// <returns></returns>
        [Column("TIMETYPE")]
        public string TimeType { get; set; }
        /// <summary>
        /// ʹ��˵��
        /// </summary>
        /// <returns></returns>
        [Column("NOTE")]
        public string Note { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("LABOROPERATIONUSERNAME")]
        public string LaborOperationUserName { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("LABOROPERATIONTIME")]
        public DateTime? LaborOperationTime { get; set; }
        /// <summary>
        /// ʹ�õ�λ����
        /// </summary>
        /// <returns></returns>
        [Column("ORGNAME")]
        public string OrgName { get; set; }
        /// <summary>
        /// ʹ�õ�λID
        /// </summary>
        /// <returns></returns>
        [Column("ORGID")]
        public string OrgId { get; set; }
        /// <summary>
        /// ʹ�õ�λCODE
        /// </summary>
        /// <returns></returns>
        [Column("ORGCODE")]
        public string OrgCode { get; set; }
        /// <summary>
        /// ʹ�ò�������
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
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
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
                    }
        #endregion
    }
}