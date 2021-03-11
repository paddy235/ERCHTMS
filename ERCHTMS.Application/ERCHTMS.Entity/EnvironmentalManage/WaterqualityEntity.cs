using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EnvironmentalManage
{
    /// <summary>
    /// �� ����ˮ�ʷ���
    /// </summary>
    [Table("BIS_WATERQUALITY")]
    public class WaterqualityEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
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
        /// ������Ա
        /// </summary>
        /// <returns></returns>
        [Column("TESTUSERNAME")]
        public string TestUserName { get; set; }
        /// <summary>
        /// ������Աid
        /// </summary>
        /// <returns></returns>
        [Column("TESTUSERID")]
        public string TestUserId { get; set; }
        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        [Column("SAMPLETYPE")]
        public string SampleType { get; set; }
        /// <summary>
        /// ��Ʒ���
        /// </summary>
        /// <returns></returns>
        [Column("SAMPLENO")]
        public string SampleNo { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("TESTDATE")]
        public DateTime? TestDate { get; set; }

        /// <summary>
        /// PH
        /// </summary>
        /// <returns></returns>
        [Column("PH")]
        public string PH { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("XFW")]
        public string XFW { get; set; }

        /// <summary>
        /// CODcr
        /// </summary>
        /// <returns></returns>
        [Column("CODCR")]
        public string CODCR { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("FHW")]
        public string FHW { get; set; }
   
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ZS")]
        public string ZS { get; set; }

        /// <summary>
        /// ��Ӳ��
        /// </summary>
        /// <returns></returns>
        [Column("ZYD")]
        public string ZYD { get; set; }

        /// <summary>
        /// ��
        /// </summary>
        /// <returns></returns>
        [Column("GE")]
        public string GE { get; set; }

        /// <summary>
        /// ��
        /// </summary>
        /// <returns></returns>
        [Column("GONG")]
        public string GONG { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ZGE")]
        public string ZGE { get; set; }

        /// <summary>
        /// �ܸ�
        /// </summary>
        /// <returns></returns>
        [Column("ZL")]
        public string ZL { get; set; }
         
        /// <summary>
        /// ��Ǧ
        /// </summary>
        /// <returns></returns>
        [Column("ZQ")]
        public string ZQ { get; set; }

        /// <summary>
        /// ��п
        /// </summary>
        /// <returns></returns>
        [Column("ZX")]
        public string ZX { get; set; }
	
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ZXIU")]
        public string ZXIU { get; set; }
     
        /// <summary>
        /// ��ֲ����
        /// </summary>
        /// <returns></returns>
        [Column("DZWY")]
        public string DZWY { get; set; }

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