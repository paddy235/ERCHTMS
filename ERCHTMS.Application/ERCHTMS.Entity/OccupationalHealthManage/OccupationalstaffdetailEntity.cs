using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OccupationalHealthManage
{
    /// <summary>
    /// �� ����ְҵ���������
    /// </summary>
    [Table("BIS_OCCUPATIONALSTAFFDETAIL")]
    public class OccupationalstaffdetailEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ���ID
        /// </summary>
        /// <returns></returns>
        [Column("OCCDETAILID")]
        public string OccDetailId { get; set; }
        /// <summary>
        /// ���ID
        /// </summary>
        /// <returns></returns>
        [Column("OCCID")]
        public string OccId { get; set; }
        /// <summary>
        /// �����ID
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// ��Ա����
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
        /// <summary>
        /// ����ƴ��������ĸ��
        /// </summary>
        /// <returns></returns>
        [Column("USERNAMEPINYIN")]
        public string UserNamePinYin { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("INSPECTIONTIME")]
        public DateTime? InspectionTime { get; set; }
        /// <summary>
        /// �Ƿ���ְҵ��
        /// </summary>
        /// <returns></returns>
        [Column("ISSICK")]
        public int? Issick { get; set; }
        /// <summary>
        /// ְҵ������
        /// </summary>
        /// <returns></returns>
        [Column("SICKTYPE")]
        public string SickType { get; set; }

        /// <summary>
        /// ְҵ����������
        /// </summary>
        /// <returns></returns>
        [Column("SICKTYPENAME")]
        public string SickTypeName { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("NOTE")]
        public string Note { get; set; }
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
        /// �쳣����
        /// </summary>
        [Column("UNUSUALNOTE")]
        public string UnusualNote { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.OccDetailId = string.IsNullOrEmpty(OccDetailId) ? Guid.NewGuid().ToString() : OccDetailId;
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
            this.OccDetailId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
        }
        #endregion
    }
}