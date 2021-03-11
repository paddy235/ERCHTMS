using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.FireManage
{
    /// <summary>
    /// �� �����ص����λ
    /// </summary>
    [Table("HRS_KEYPART")]
    public class KeyPartEntity : BaseEntity
    {
        #region ʵ���Ա
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
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
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
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// �ص����λ����
        /// </summary>
        /// <returns></returns>
        [Column("PARTNAME")]
        public string PartName { get; set; }
        /// <summary>
        /// �ص����λ���Ʊ���
        /// </summary>
        /// <returns></returns>
        [Column("PARTNO")]
        public string PartNo { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [Column("PLACE")]
        public string Place { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICT")]
        public string District { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTID")]
        public string DistrictId { get; set; }
        /// <summary>
        /// ����Code
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTCODE")]
        public string DistrictCode { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSER")]
        public string DutyUser { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        /// ���β���
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPT")]
        public string DutyDept { get; set; }
        /// <summary>
        /// ���β���ID
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPTCODE")]
        public string DutyDeptCode { get; set; }
        /// <summary>
        /// �����˵绰
        /// </summary>
        /// <returns></returns>
        [Column("DUTYTEL")]
        public string DutyTel { get; set; }
        /// <summary>
        /// �����ṹ
        /// </summary>
        /// <returns></returns>
        [Column("STRUCTURE")]
        public string Structure { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("ACREAGE")]
        public int? Acreage { get; set; }
        /// <summary>
        /// ��Ҫ�洢��Ʒ
        /// </summary>
        /// <returns></returns>
        [Column("STOREGOODS")]
        public string StoreGoods { get; set; }
        /// <summary>
        /// ��Ҫ���װ��
        /// </summary>
        /// <returns></returns>
        [Column("OUTFIREEQUIP")]
        public string OutfireEquip { get; set; }
        /// <summary>
        /// �ص����λ����
        /// </summary>
        /// <returns></returns>
        [Column("PEOPLENUM")]
        public int? PeopleNum { get; set; }
        /// <summary>
        /// ���𼶱�
        /// </summary>
        /// <returns></returns>
        [Column("RANK")]
        public int? Rank { get; set; }
        /// <summary>
        /// ���Ѳ������
        /// </summary>
        /// <returns></returns>
        [Column("LATELYPATROLDATE")]
        public DateTime? LatelyPatrolDate { get; set; }
        /// <summary>
        /// Ѳ������
        /// </summary>
        /// <returns></returns>
        [Column("PATROLPERIOD")]
        public int? PatrolPeriod { get; set; }
        /// <summary>
        /// �´�Ѳ������
        /// </summary>
        /// <returns></returns>
        [Column("NEXTPATROLDATE")]
        public DateTime? NextPatrolDate { get; set; }
        /// <summary>
        /// ʹ��״̬
        /// </summary>
        /// <returns></returns>
        [Column("EMPLOYSTATE")]
        public int? EmployState { get; set; }
        /// <summary>
        /// ����Σ���Է���
        /// </summary>
        /// <returns></returns>
        [Column("ANALYZE")]
        public string Analyze { get; set; }
        /// <summary>
        /// �����ʩ
        /// </summary>
        /// <returns></returns>
        [Column("MEASURE")]
        public string Measure { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("SCHEME")]
        public string Scheme { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("SCHEMEID")]
        public string SchemeId { get; set; }
        /// <summary>
        /// ��������ID
        /// </summary>
        /// <returns></returns>
        [Column("SCHEMEFJID")]
        public string SchemeFjId { get; set; }
        /// <summary>
        /// ����Ҫ��
        /// </summary>
        /// <returns></returns>
        [Column("REQUIRE")]
        public string Require { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// nfc
        /// </summary>
        /// <returns></returns>
        [Column("NFC")]
        public string NFC { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AutoId { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
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