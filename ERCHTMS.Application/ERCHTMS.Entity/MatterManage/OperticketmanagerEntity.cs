using System;
using System.ComponentModel.DataAnnotations.Schema;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.MatterManage
{
    /// <summary>
    /// �� ������Ʊ�����볧��Ʊ
    /// </summary>
    [Table("WL_OPERTICKETMANAGER")]
    public class OperticketmanagerEntity : BaseEntity
    {
        #region Ĭ���ֶ�
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
        /// <summary>
        /// �����û�Id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// �����û�����
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �����û�����ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTID")]
        public string Createuserdeptid { get; set; }
        /// <summary>
        /// �����û����ű���
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// �����û���������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// �޸��û�ID
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �޸��û�����
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        #endregion

        #region ʵ���Ա
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [Column("TAKEGOODSNAME")]
        public string Takegoodsname { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        /// <returns></returns>
        [Column("TAKEGOODSID")]
        public string Takegoodsid { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("SUPPLYNAME")]
        public string Supplyname { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("SUPPLYID")]
        public string Supplyid { get; set; }
        /// <summary>
        /// ��ƱԱ����
        /// </summary>
        /// <returns></returns>
        [Column("OPERNAME")]
        public string Opername { get; set; }
        /// <summary>
        /// ��ƱԱ�˻�
        /// </summary>
        /// <returns></returns>
        [Column("OPERACCOUNT")]
        public string Operaccount { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("LETMAN")]
        public string LetMan { get; set; }
        /// <summary>
        /// �Ƿ����1����
        /// </summary>
        /// <returns></returns>
        [Column("ISDELETE")]
        public int? Isdelete { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("NUMBERS")]
        public string Numbers { get; set; }
        /// <summary>
        /// �볡ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("GETDATA")]
        public DateTime? Getdata { get; set; }

        /// <summary>
        /// �볡��ӡʱ��
        /// </summary>
        /// <returns></returns>
        [Column("GETSTAMPTIME")]
        public DateTime? GetStampTime { get; set; }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        /// <returns></returns>
        [Column("PRODUCTTYPE")]
        public string Producttype { get; set; }

        /// <summary>
        /// ��Ʒ���ͼ�ֵ
        /// </summary>
        /// <returns></returns>
        [Column("PRODUCTTYPEID")]
        public string ProducttypeId { get; set; }

        /// <summary>
        /// ���ƺ�
        /// </summary>
        /// <returns></returns>
        [Column("PLATENUMBER")]
        public string Platenumber { get; set; }
        /// <summary>
        /// װ�ϵ�
        /// </summary>
        /// <returns></returns>
        [Column("DRESS")]
        public string Dress { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Column("TRANSPORTTYPE")]
        public string Transporttype { get; set; }

        /// <summary>
        /// ɾ��ԭ��
        /// </summary>
        [Column("DELETECONTENT")]
        public string DeleteContent { get; set; }

        /// <summary>
        /// �쳣���б�ע/ɾ��ԭ��ע
        /// </summary>
        [Column("PASSREMARK")]
        public string PassRemark { get; set; }

        /// <summary>
        /// �Ƿ��һ���볡
        /// </summary>
        [Column("ISFIRST")]
        public string IsFirst { get; set; }

        /// <summary>
        /// �Ƿ񰴹켣��ʻ
        /// </summary>
        [Column("ISTRAJECTORY")]
        public string IsTrajectory { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [Column("DATABASENUM")]
        public int? DataBaseNum { get; set; }

        /// <summary>
        /// ���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("GODATABASETIME")]
        public DateTime? GoDatabasetime { get; set; }

        /// <summary>
        /// ������ӡʱ��
        /// </summary>
        /// <returns></returns>
        [Column("OUTDATABASETIME")]
        public DateTime? OutDatabasetime { get; set; }

        /// <summary>
        /// ���ش���
        /// </summary>
        [Column("WEIGHINGNUM")]
        public int? WeighingNum { get; set; }

        /// <summary>
        /// �볧ʱ��
        /// </summary>
        [Column("OUTDATE")]
        public DateTime? OutDate { get; set; }
        /// <summary>
        /// ���ڶ���ʱ��(����)
        /// </summary>
        [Column("STAYTIME")]
        public double? StayTime { get; set; }
        /// <summary>
        /// ����״̬
        /// </summary>
        [Column("STATUS")]
        public string Status { get; set; }
        /// <summary> 
        /// ����״̬��0����1˾���ϴ�  2Ϊ��¼��GPS���� 3����ͨ�� 4�볧 99�ܾ��볡��
        /// </summary>
        [Column("EXAMINESTATUS")]
        public int ExamineStatus { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Column("WEIGHT")]
        public double? Weight { get; set; }

        /// <summary>
        /// ׼��
        /// </summary>
        [Column("ADMITTANCE")]
        public string Admittance { get; set; }
        /// <summary>
        /// ����״̬1�ѳ���
        /// </summary>
        [Column("OUTCU")]
        public string OutCu { get; set; }
        /// <summary>
        /// ˾������
        /// </summary>
        [Column("DRIVERNAME")]
        public string DriverName { get; set; }
        /// <summary>
        /// ˾���绰
        /// </summary>
        [Column("DRIVERTEL")]
        public string DriverTel { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Column("HZWEIGHT")]
        public double HzWeight { get; set; }
        /// <summary>
        /// ��ʻ֤ͼƬ·��
        /// </summary>
        [Column("JSIMGPATH")]
        public string JsImgpath { get; set; }
        /// <summary>
        /// ��ʻ֤ͼƬ·��
        /// </summary>
        [Column("XSIMGPATH")]
        public string XsImgpath { get; set; }
        /// <summary>
        /// Gps����
        /// </summary>
        [Column("GPSNAME")]
        public string GpsName { get; set; }
        /// <summary>
        /// Gps����
        /// </summary>
        [Column("GPSID")]
        public string GpsId { get; set; }
        /// <summary>
        /// ����ذ���ʱ��
        /// </summary>
        [Column("BALANCETIME")]
        public DateTime? BalanceTime { get; set; }

        /// <summary>
        /// ���������ֶ�
        /// </summary>
        [Column("ORDERNUM")]
        public int OrderNum { get; set; }

        /// <summary>
        /// �볡��Ʊ����
        /// </summary>
        [Column("ORDERNUMR")]
        public int OrderNumR { get; set; }

        /// <summary>
        /// �Ƿ���ͷ��1��Ĭ�Ϸ�0��
        /// </summary>
        [Column("ISWHARF")]
        public int ISwharf { get; set; }

        /// <summary>
        /// ������ʻ·��״̬
        /// </summary>
        [Column("TRAVELSTATUS")]
        public string TravelStatus { get; set; }

        /// <summary>
        /// ��ʻ�����֤��Ƭ
        /// </summary>
        [Column("IDENTITETIIMG")]
        public string IdentitetiImg { get; set; }


        /// <summary>
        ///����״̬�������ų���
        /// </summary>
        [NotMapped]
        public string NetweightStatus { get; set; }
        /// <summary>
        ///���״̬�������ų���
        /// </summary>
        [NotMapped]
        public string DatabaseStatus { get; set; }
        /// <summary>
        ///����ͣ��ʱ��״̬�������ų���
        /// </summary>
        [NotMapped]
        public string StayTimeStatus { get; set; }

        /// <summary>
        ///�볧���ذ�ʱ�䣨�����ų���
        /// </summary>
        [NotMapped]
        public string RCdbTime { get; set; }

        /// <summary>
        ///�ذ�������ʱ�䣨�����ų���
        /// </summary>
        [NotMapped]
        public string DbOutTime { get; set; }

        /// <summary>
        ///ģ�������ֶ�
        /// </summary>
        [Column("TEMPLATESORT")]
        public int? TemplateSort { get; set; }

        /// <summary>
        /// �Ƿ�װ��
        /// </summary>
        [Column("SHIPLOADING")]
        public int ShipLoading { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            if (string.IsNullOrEmpty(this.CreateUserId) && OperatorProvider.Provider.Current() != null)
                this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            if (string.IsNullOrEmpty(this.CreateUserName) && OperatorProvider.Provider.Current() != null)
                this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            if (string.IsNullOrEmpty(this.CreateUserDeptCode) && OperatorProvider.Provider.Current() != null)
                this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            if (string.IsNullOrEmpty(this.CreateUserOrgCode) && OperatorProvider.Provider.Current() != null)
                this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
            if (string.IsNullOrEmpty(this.Createuserdeptid) && OperatorProvider.Provider.Current() != null)
                this.Createuserdeptid = OperatorProvider.Provider.Current().DeptId;
            this.Isdelete = 1;
            this.Weight = 0;
            this.HzWeight = 0;
            this.ExamineStatus = 0;
            this.OrderNum = this.OrderNumR = 0;
            this.TravelStatus = this.Status = "����";

        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            //this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            //this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}