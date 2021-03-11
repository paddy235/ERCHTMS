using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.RiskDatabase
{
    /// <summary>
    /// �� ��������Ԥ֪ѵ����
    /// </summary>
    [Table("BIS_RISKTRAINLIB")]
    public class RisktrainlibEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// ��Դ׼��
        /// </summary>
        /// <returns></returns>
        [Column("RESOURCES")]
        public string Resources { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKDES")]
        public string WorkDes { get; set; }
        /// <summary>
        /// ��ҵ��λ
        /// </summary>
        /// <returns></returns>
        [Column("WORKPOST")]
        public string WorkPost { get; set; }
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
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("WORKTASK")]
        public string WorkTask { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// ��ҵ���
        /// </summary>
        /// <returns></returns>
        [Column("WORKTYPE")]
        public string WorkType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// ʹ�ô���
        /// </summary>
        /// <returns></returns>
        [Column("USERNUM")]
        public int? UserNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREA")]
        public string WorkArea { get; set; }
        /// <summary>
        /// �޸Ĵ���
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYNUM")]
        public int? ModifyNum { get; set; }
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
        /// ��ҵ����Id
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREAID")]
        public string WorkAreaId { get; set; }
         /// <summary>
        /// ���յȼ�
        /// </summary>
        /// <returns></returns>
        [Column("RISKLEVEL")]
        public string RiskLevel { get; set; }
                /// <summary>
        /// ���յȼ�
        /// </summary>
        /// <returns></returns>
        [Column("RISKLEVELVAL")]
        public string RiskLevelVal { get; set; }
                    /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKTYPECODE")]
        public string WorkTypeCode { get; set; }
         /// <summary>
        /// ������Դ   1 ����Դ�����嵥 2 ��Դ���ص��� 3 �������ݣ��������޸ģ� 
        /// </summary>
        /// <returns></returns>
        [Column("DATASOURCES")]
        public string DataSources { get; set; }
        
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
           this.UserNum = 0;
           this.ModifyNum = 0;
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