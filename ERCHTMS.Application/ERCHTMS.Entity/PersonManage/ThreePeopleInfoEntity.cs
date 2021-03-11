using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.PersonManage
{
    /// <summary>
    /// �� ��������������ҵ���
    /// </summary>
    [Table("BIS_THREEPEOPLEINFO")]
    public class ThreePeopleInfoEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����ҵ���¼Id
        /// </summary>
        /// <returns></returns>
        [Column("APPLYID")]
        public string ApplyId { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// ���Գɼ�
        /// </summary>
        /// <returns></returns>
        [Column("SCORE")]
        public int? Score { get; set; }
        /// <summary>
        /// ����Ʊ����
        /// </summary>
        /// <returns></returns>
        [Column("TICKETTYPE")]
        public string TicketType { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [Column("ORGCODE")]
        public string OrgCode { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
        
        /// <summary>
        /// ���֤��
        /// </summary>
        /// <returns></returns>
        [Column("IDCARD")]
        public string IdCard { get; set; }
        /// <summary>
        /// ����
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
            var user = OperatorProvider.Provider.Current();
            if (user != null)
            {
                OrgCode = user.OrganizeCode;
            }
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
                                            }
        #endregion
    }
}