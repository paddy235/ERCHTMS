using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.MatterManage
{
    /// <summary>
    /// �� ������������ 
    /// </summary>
    [Table("WL_CALCULATE")]
    public class CalculateEntity : BaseEntity
    {
        #region �����ֶ�
        /// <summary>
        /// �޸��û�����
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// �޸��û�ID
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �����û���������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// �����û����ű���
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// �����û�����ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTID")]
        public string Createuserdeptid { get; set; }
        /// <summary>
        /// �����û�����
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �����û�Id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        #endregion

        #region ҵ���Ա

        /// <summary>
        /// ���Ӵ�ӡʱ��
        /// </summary>
        /// <returns></returns>
        [Column("STAMPTIME")]
        public DateTime? Stamptime { get; set; }
        /// <summary>
        /// Ƥ��˾��Ա
        /// </summary>
        /// <returns></returns>
        [Column("TAREUSERNAME")]
        public string Tareusername { get; set; }
        /// <summary>
        /// ë��˾��Ա
        /// </summary>
        /// <returns></returns>
        [Column("ROUGHUSERNAME")]
        public string Roughusername { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// Ƥ��ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("TARETIME")]
        public DateTime? Taretime { get; set; }
        /// <summary>
        /// ë��ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ROUGHTIME")]
        public DateTime? Roughtime { get; set; }
        /// <summary>
        /// ���ƺ���
        /// </summary>
        /// <returns></returns>
        [Column("PLATENUMBER")]
        public string Platenumber { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("TRANSPORTTYPE")]
        public string Transporttype { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        /// <returns></returns>
        [Column("TAKEGOODSID")]
        public string Takegoodsid { get; set; }
        /// <summary>
        /// �����(�˻���λ)
        /// </summary>
        /// <returns></returns>
        [Column("TAKEGOODSNAME")]
        public string Takegoodsname { get; set; }
        /// <summary>
        /// �쳣����
        /// </summary>
        /// <returns></returns>
        [Column("UNUSUALREMIND")]
        public string Unusualremind { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("NETWNEIGHT")]
        public double? Netwneight { get; set; }
        /// <summary>
        /// Ƥ��
        /// </summary>
        /// <returns></returns>
        [Column("TARE")]
        public double? Tare { get; set; }
        /// <summary>
        /// ë��
        /// </summary>
        /// <returns></returns>
        [Column("ROUGH")]
        public double? Rough { get; set; }

        /// <summary>
        /// Ƥ���ܺ�
        /// </summary>
        /// <returns></returns>
        [Column("TARECOUNT")]
        public double? TareCount { get; set; }
        /// <summary>
        /// ë���ܺ�
        /// </summary>
        /// <returns></returns>
        [Column("ROUGHCOUNT")]
        public double? RoughCount { get; set; }

        /// <summary>
        ///����Ʒ����(����)
        /// </summary>
        /// <returns></returns>
        [Column("GOODSNAME")]
        public string Goodsname { get; set; }
        /// <summary>
        /// ���ص���
        /// </summary>
        /// <returns></returns>
        [Column("NUMBERS")]
        public string Numbers { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("WEIGHT")]
        public double? Weight { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("POUNDNAME")]
        public string Poundname { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [Column("POUNDCODE")]
        public string Poundcode { get; set; }

        /// <summary>
        /// ���뷽ʽ1�Զ�0�ֶ�
        /// </summary>
        [Column("INSERTTYPE")]
        public string InsertType { get; set; }

        /// <summary>
        /// �Ƿ����1����0������
        /// </summary>
        [Column("ISDELETE")]
        public int IsDelete { get; set; }

        /// <summary>
        /// ɾ��ԭ��
        /// </summary>
        [Column("DELETECONTENT")]
        public string DeleteContent { get; set; }

        /// <summary>
        /// �볡��Ʊ������Σ��Ʒ����
        /// </summary>
        [Column("BASEID")]
        public string BaseId { get; set; }

        /// <summary>
        /// �Ƿ��ѳ���1��
        /// </summary>
        [Column("ISOUT")]
        public int? IsOut { get; set; }

        /// <summary>
        /// �������� ��4���ϳ����볡��Ʊ����5Σ��Ʒ��99����������
        /// </summary>
        [Column("DATATYPE")]
        public string DataType { get; set; }

        /// <summary>
        /// �Ƿ�����⣨1��0��
        /// </summary>
        [Column("ISSTORAGE")]
        public int? IsStorage { get; set; }

        /// <summary>
        /// �ɵذ���¼����
        /// </summary>
        [Column("OLDID")]
        public string OldId { get; set; }

       
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
            this.IsOut = 0; this.IsDelete = 1;
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