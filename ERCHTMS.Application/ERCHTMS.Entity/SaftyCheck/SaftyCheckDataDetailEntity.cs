using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.SaftyCheck
{
    /// <summary>
    /// �� ������ȫ��������
    /// </summary>
    [Table("BIS_SAFTYCHECKDATADETAILED")]
    public class SaftyCheckDataDetailEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDISTRICT")]
        public string BelongDistrict { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDISTRICTID")]
        public string BelongDistrictID { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPT")]
        public string BelongDept { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTID")]
        public string BelongDeptID { get; set; }
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
        /// <summary>
        /// �޸�����
        /// </summary>		
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// �޸��û�����
        /// </summary>		
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �޸��û�
        /// </summary>		
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("CHECKCONTENT")]
        public string CheckContent { get; set; }
        /// <summary>
        /// ���յ�����
        /// </summary>
        /// <returns></returns>
        [Column("RISKNAME")]
        public string RiskName { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [Column("RECID")]
        public string RecID { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [Column("COUNT")]
        public int? Count { get; set; }
        [NotMapped]
        /// <summary>
        /// Υ������
        /// </summary>
        public string WzCount { get; set; }
        [NotMapped]
        /// <summary>
        /// ��������
        /// </summary>
        public string WtCount { get; set; }

        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [Column("COUNTID")]
        public string CountID { get; set; }

        /// <summary>
        /// ���״̬
        /// </summary>
        /// <returns></returns>
        [Column("CHECKSTATE")]
        public int? CheckState { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMAN")]
        public string CheckMan { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMANID")]
        public string CheckManID { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDISTRICTCODE")]
        public string BelongDistrictCode { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATAID")]
        public string CheckDataId { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("CHECKOBJECT")]
        public string CheckObject { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("CHECKOBJECTID")]
        public string CheckObjectId { get; set; }
        /// <summary>
        /// ���������� 0Ϊ�豸 1ΪΣ��Դ
        /// </summary>
        /// <returns></returns>
        [Column("CHECKOBJECTTYPE")]
        public string CheckObjectType { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        public List<SaftyCheckContentEntity> Content { get; set; }
        /// <summary>
        /// �Ƿ����(0:�����ϣ�1:����)
        /// </summary>
        /// <returns></returns>
        /// <returns></returns>
        [Column("ISSURE")]
        public string IsSure { get; set; }
        /// <summary>
        ///��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        ///�����ֶ�
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AutoId { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID =string.IsNullOrEmpty(ID)? Guid.NewGuid().ToString():ID;
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