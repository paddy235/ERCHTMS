using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.RoutineSafetyWork
{
    /// <summary>
    /// �� ������ȫ����λ���Ա��
    /// </summary>
    [Table("BIS_CONFERENCEUSER")]
    public class ConferenceUserEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ���״̬(Ĭ��Ϊ0)(0:δ�������,1:���������,2:�������׼,3���δ��׼)
        /// </summary>
        /// <returns></returns>
        [Column("REVIEWSTATE")]
        public string ReviewState { get; set; }
        /// <summary>
        /// �����ID
        /// </summary>
        /// <returns></returns>
        [Column("REVIEWUSERID")]
        public string ReviewUserID { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [Column("REVIEWUSER")]
        public string ReviewUser { get; set; }
        /// <summary>
        /// ���ԭ��
        /// </summary>
        /// <returns></returns>
        [Column("REASON")]
        public string Reason { get; set; }
        /// <summary>
        /// �Ƿ�ǩ��(0�ǣ�1��)
        /// </summary>
        /// <returns></returns>
        [Column("ISSIGN")]
        public string Issign { get; set; }
        /// <summary>
        /// ǩ����Ƭ·��
        /// </summary>
        /// <returns></returns>
        [Column("PHOTOURL")]
        public string PhotoUrl { get; set; }
        /// <summary>
        /// �����¼ID
        /// </summary>
        /// <returns></returns>
        [Column("CONFERENCEID")]
        public string ConferenceID { get; set; }
        /// <summary>
        /// ��������ID
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// �û�ID
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserID { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
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
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
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
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
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
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
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
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
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