using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.RiskDatabase
{
    /// <summary>
    /// �� ������ҵ����豸��ʩ�嵥
    /// </summary>
    [Table("BIS_BASELISTING")]
    public class BaseListingEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("AREANAME")]
        public string AreaName { get; set; }
        /// <summary>
        /// �Ƿ������豸 0���� 1����
        /// </summary>
        /// <returns></returns>
        [Column("ISSPECIALEQU")]
        public int? IsSpecialEqu { get; set; }
        /// <summary>
        /// ��ҵ�����/�豸����
        /// </summary>
        /// <returns></returns>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �޸��û�����
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("OTHERS")]
        public string Others { get; set; }
        /// <summary>
        /// �޸ļ�¼ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// �Ƿ񳣹�  0������ 1���ǳ���
        /// </summary>
        /// <returns></returns>
        [Column("ISCONVENTIONAL")]
        public int? IsConventional { get; set; }
        /// <summary>
        /// ����id
        /// </summary>
        /// <returns></returns>
        [Column("AREAID")]
        public string AreaId { get; set; }
        /// <summary>
        /// �����û�������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [Column("ACTIVITYSTEP")]
        public string ActivityStep { get; set; }
        /// <summary>
        /// �޸��û�ID
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �����û�����
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// ����Code
        /// </summary>
        /// <returns></returns>
        [Column("AREACODE")]
        public string AreaCode { get; set; }
        /// <summary>
        /// �����û�ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// �����û��������ű���
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// ���� 0����ҵ��嵥 1���豸��ʩ�嵥
        /// </summary>
        [Column("TYPE")]
        public int Type { get; set; }
        /// <summary>
        /// �ܿز���
        /// </summary>
        [Column("CONTROLSDEPT")]
        public string ControlsDept { get; set; }
        /// <summary>
        /// �ܿز���
        /// </summary>
        [Column("CONTROLSDEPTID")]
        public string ControlsDeptId { get; set; }
        /// <summary>
        /// �ܿز���
        /// </summary>
        [Column("CONTROLSDEPTCODE")]
        public string ControlsDeptCode { get; set; }

        /// <summary>
        /// ��λ(����)
        /// </summary>
        [Column("POST")]
        public string Post { get; set; }

        /// <summary>
        /// ��λ(����)
        /// </summary>
        [Column("POSTID")]
        public string PostId { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
            this.CreateDate = this.CreateDate == null ? DateTime.Now : this.CreateDate;
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