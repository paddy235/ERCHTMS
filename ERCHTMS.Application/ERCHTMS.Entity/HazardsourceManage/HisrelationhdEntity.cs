using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HazardsourceManage
{
    /// <summary>
    /// �� ������ʷ��¼
    /// </summary>
    [Table("HSD_HISRELATIONHD")]
    public class HisrelationhdEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ��ȫ���ƴ�ʩ
        /// </summary>
        /// <returns></returns>
        [Column("MEASURENUM")]
        public int? MeaSureNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("HAZARDSOURCEID")]
        public string HazardSourceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ITEMDECR")]
        public decimal? ItemDecR { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ITEMDECQ1")]
        public string ItemDecQ1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ITEMDECQ")]
        public string ItemDecQ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ITEMDECB")]
        public decimal? ItemDecB { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ITEMDECR1")]
        public decimal? ItemDecR1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ITEMDECB1")]
        public string ItemDecB1 { get; set; }





        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("WAY")]
        public string Way { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("RISKTYPE")]
        public string RiskType { get; set; }
        /// <summary>
        /// ������ĿR
        /// </summary>
        /// <returns></returns>
        [Column("ITEMR")]
        public decimal? ItemR { get; set; }
        /// <summary>
        /// ������ĿC
        /// </summary>
        /// <returns></returns>
        [Column("ITEMC")]
        public decimal? ItemC { get; set; }
        /// <summary>
        /// ������ĿB
        /// </summary>
        /// <returns></returns>
        [Column("ITEMB")]
        public decimal? ItemB { get; set; }
        /// <summary>
        /// ������ĿA
        /// </summary>
        /// <returns></returns>
        [Column("ITEMA")]
        public decimal? ItemA { get; set; }


        /// <summary>
        /// �������ֵȼ�(����յȼ���Ӧ)
        /// </summary>
        /// <returns></returns>
        [Column("GRADEVAL")]
        public int? GradeVal { get; set; }


        /// <summary>
        /// ���յȼ�
        /// </summary>
        /// <returns></returns>
        [Column("GRADE")]
        public string Grade { get; set; }

        /// <summary>
        /// Σ��Դ����/����
        /// </summary>
        /// <returns></returns>
        [Column("DANGERSOURCE")]
        public string DangerSource { get; set; }
        /// <summary>
        /// �޸ļ�¼ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// �����û�ID
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �¹�����
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTTYPE")]
        public string AccidentType { get; set; }
        /// <summary>
        /// �ල����������
        /// </summary>
        /// <returns></returns>
        [Column("JDGLZRRUSERID")]
        public string JdglzrrUserId { get; set; }
        /// <summary>
        /// ��ȫ���ƴ�ʩ
        /// </summary>
        /// <returns></returns>
        [Column("MEASURE")]
        public string MeaSure { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �¹���������
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTNAME")]
        public string AccidentName { get; set; }
        /// <summary>
        /// ��������ID
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTID")]
        public string DistrictId { get; set; }
        /// <summary>
        /// �޸��û�����
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// ״̬ 1��ģ�嵼�� 2��ƽ̨���� 3������
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public int? Status { get; set; }
        /// <summary>
        /// ��ȫ���ƴ�ʩ
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// �����嵥����ID
        /// </summary>
        /// <returns></returns>
        [Column("RISKASSESSID")]
        public string RiskassessId { get; set; }
        /// <summary>
        /// �����û�������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// �ල����������
        /// </summary>
        /// <returns></returns>
        [Column("JDGLZRRFULLNAME")]
        public string JdglzrrFullName { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [Column("DISTRICTNAME")]
        public string DistrictName { get; set; }
        /// <summary>
        /// �����û��������ű���
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// �����û�ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// �Ƿ�Ϊ�ش�Σ��Դ0���� 1����
        /// </summary>
        /// <returns></returns>
        [Column("ISDANGER")]
        public int? IsDanger { get; set; }
        /// <summary>
        /// ���β���  
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
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