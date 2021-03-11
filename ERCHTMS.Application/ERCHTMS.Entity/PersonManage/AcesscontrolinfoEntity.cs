using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.PersonManage
{
    /// <summary>
    /// �� �����û��Ž�������
    /// </summary>
    [Table("BIS_ACESSCONTROLINFO")]
    public class AcesscontrolinfoEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("TID")]
        public string TID { get; set; }
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
        /// �û�ID
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// �û�ָ���û�ָ������
        /// </summary>
        /// <returns></returns>
        [Column("FINGER")]
        public string Finger { get; set; }
        /// <summary>
        /// �Ƿ���ָ������
        /// </summary>
        /// <returns></returns>
        [Column("ISFINGER")]
        public int? IsFinger { get; set; }
        /// <summary>
        /// �Ƿ�����������
        /// </summary>
        /// <returns></returns>
        [Column("ISFACE")]
        public int? IsFace { get; set; }
        /// <summary>
        /// ����ͼƬbase64��
        /// </summary>
        /// <returns></returns>
        [Column("FACE")]
        public string Face { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.TID = string.IsNullOrEmpty(TID) ? Guid.NewGuid().ToString() : TID;
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
            this.TID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
                    }
        #endregion
    }
}