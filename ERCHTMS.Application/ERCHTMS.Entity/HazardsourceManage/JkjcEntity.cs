using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HazardsourceManage
{
    /// <summary>
    /// �� �������
    /// </summary>
    [Table("HSD_JKJC")]
    public class JkjcEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// �����Ա
        /// </summary>
        /// <returns></returns>
        [Column("JKUSERNAME")]
        public string JkUserName { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("JKTIMESTART")]
        public DateTime? JkTimeStart { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        /// <returns></returns>
        [Column("JKZF")]
        public string Jkzf { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("JKTIMEEND")]
        public DateTime? JkTimeEnd { get; set; }
        /// <summary>
        /// �����˲���code
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// Һλ
        /// </summary>
        /// <returns></returns>
        [Column("JKYW")]
        public string Jkyw { get; set; }


        /// <summary>
        /// �޸���
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("JKFILES")]
        public string JkFiles { get; set; }
        /// <summary>
        /// �����Ǽ�����id����
        /// </summary>
        /// <returns></returns>
        [Column("JKYHZGIDS")]
        public string JkyhzgIds { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// ������orgcode
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("JKOTHER")]
        public string JkOther { get; set; }
        /// <summary>
        /// �ܿ�״̬ 0���ܿ�,1�ܿ�
        /// </summary>
        /// <returns></returns>
        [Column("JKSKSTATUS")]
        public int? JkskStatus { get; set; }
        /// <summary>
        /// �����Ա
        /// </summary>
        /// <returns></returns>
        [Column("JKUSERID")]
        public string JkUserId { get; set; }
        /// <summary>
        /// �������ش�Σ��Դid
        /// </summary>
        /// <returns></returns>
        [Column("HDID")]
        public string HdId { get; set; }
        /// <summary>
        /// �����л�ѧ����Ũ��
        /// </summary>
        /// <returns></returns>
        [Column("JKHXWZND")]
        public string Jkhxwznd { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �޸���
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("JKLL")]
        public string Jkll { get; set; }
        /// <summary>
        /// �¶�
        /// </summary>
        /// <returns></returns>
        [Column("JKWD")]
        public string Jkwd { get; set; }
        /// <summary>
        /// ѹ��
        /// </summary>
        /// <returns></returns>
        [Column("JKYL")]
        public string Jkyl { get; set; }
        /// <summary>
        /// ��صص�
        /// </summary>
        /// <returns></returns>
        [Column("JKAREAR")]
        public string JkArear { get; set; }
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
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}