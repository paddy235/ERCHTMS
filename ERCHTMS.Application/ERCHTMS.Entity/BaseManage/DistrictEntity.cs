using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.BaseManage
{
    /// <summary>
    /// �� ������������
    /// </summary>
    [Table("BIS_DISTRICT")]
    public class DistrictEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTID")]
        public string DistrictID { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPT")]
        public string BelongDept { get; set; }
        /// <summary>
        /// ���Ÿ�����
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCHARGEPERSON")]
        public string DeptChargePerson { get; set; }

        /// <summary>
        /// ���Ÿ���������
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCHARGEPERSONID")]
        public string DeptChargePersonID { get; set; }
        /// <summary>
        /// ��������
        /// </summary>	
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �����û�����
        /// </summary>		
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>	
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
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
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("SORTCODE")]
        public int? SortCode { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("DISREICTCHARGEPERSON")]
        public string DisreictChargePerson { get; set; }

        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [Column("DISREICTCHARGEPERSONID")]
        public string DisreictChargePersonID { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTNAME")]
        public string DistrictName { get; set; }
        /// <summary>
        /// �ܿز���
        /// </summary>
        /// <returns></returns>
        [Column("CHARGEDEPT")]
        public string ChargeDept { get; set; }

        /// <summary>
        /// �ܿز�������
        /// </summary>
        /// <returns></returns>
        [Column("CHARGEDEPTID")]
        public string ChargeDeptID { get; set; }
        /// <summary>
        /// �ܿز���CODE
        /// </summary>
        /// <returns></returns>
        [Column("CHARGEDEPTCODE")]
        public string ChargeDeptCode { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTCODE")]
        public string DistrictCode { get; set; }
        
        /// <summary>
        /// ������˾
        /// </summary>
        /// <returns></returns>
        [Column("BELONGCOMPANY")]
        public string BelongCompany { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("PARENTID")]
        public string ParentID { get; set; }

        /// <summary>
        /// ��˾����
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZEID")]
        public string OrganizeId { get; set; }

        /// <summary>
        /// ��ϵ��
        /// </summary>
        /// <returns></returns>
        [Column("LINKMAN")]
        public string LinkMan { get; set; }
        /// <summary>
        /// ��ϵ������
        /// </summary>
        /// <returns></returns>
        [Column("LINKEMAIL")]
        public string LinkMail { get; set; }
       
        /// <summary>
        /// ��ϵ�绰
        /// </summary>
        /// <returns></returns>
        [Column("LINKTEL")]
        public string LinkTel { get; set; }

        /// <summary>
        /// �������������ű���
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }

        /// <summary>
        /// ������������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        /// <summary>
        /// ������˾������
        /// </summary>
        /// <returns></returns>
        [Column("LINKTOCOMPANY")]
        public string LinkToCompany { get; set; }

        /// <summary>
        /// ������˾������ID
        /// </summary>
        /// <returns></returns>
        [Column("LINKTOCOMPANYID")]
        public string LinkToCompanyID { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("LATLNG")]
        public string LatLng { get; set; }

        /// <summary>
        /// ��ȫ��첿����������
        /// </summary>
        [Column("SAFETYDEPTCHARGEPERSON")]
        public string SafetyDeptChargePerson { get; set; }

        /// <summary>
        /// ��ȫ��첿����������
        /// </summary>
        [Column("SAFETYDEPTCHARGEPERSONID")]
        public string SafetyDeptChargePersonID { get; set; }

        /// <summary>
        /// ��ȫ��첿������������ϵ�绰
        /// </summary>
        [Column("SAFETYLINKTEL")]
        public string SafetyLinkTel { get; set; }

        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.DistrictID = Guid.NewGuid().ToString();
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
            this.DistrictID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}