using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ERCHTMS.Entity.SaftyCheck
{
    /// <summary>
    /// �� ������ȫ����¼
    /// </summary>
    [Table("BIS_SAFTYCHECKDATARECORD")]
    public class SaftyCheckDataRecordEntity : BaseEntity
    {
        #region ʵ���Ա
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
        [Column("CHECKDATARECORDNAME")]
        public string CheckDataRecordName { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATATYPE")]
        public int? CheckDataType { get; set; }
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
        /// Ҫ���鿪ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CHECKBEGINTIME")]
        public DateTime? CheckBeginTime { get; set; }
        /// <summary>
        /// Ҫ�������ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CHECKENDTIME")]
        public DateTime? CheckEndTime { get; set; }

        /// <summary>
        /// ��鿪ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("STARTDATE")]
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// ������ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ENDDATE")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// ��鼶��
        /// </summary>
        /// <returns></returns>
        [Column("CHECKLEVEL")]
        public string CheckLevel { get; set; }

        /// <summary>
        /// �ϼ���鼶��
        /// </summary>
        /// <returns></returns>
        [Column("SJCHECKLEVEL")]
        public string SJCheckLevel { get; set; }
        /// <summary>
        /// ������Ա�˺ţ�������ŷָ���
        /// </summary>
        /// <returns></returns>
        [Column("CHECKLEVELID")]
        public string CheckLevelID { get; set; }
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
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTID")]
        public string BelongDeptID { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPT")]
        public string BelongDept { get; set; }
        /// <summary>
        /// ��鲿������
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTID")]
        public string CheckDeptID { get; set; }
        /// <summary>
        /// ��鲿��
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPT")]
        public string CheckDept { get; set; }
        /// <summary>
        /// ����鵥λ����
        /// </summary>
        /// <returns></returns>
        [Column("CHECKEDDEPARTID")]
        public string CheckedDepartID { get; set; }
        /// <summary>
        /// ����鵥λ
        /// </summary>
        /// <returns></returns>
        [Column("CHECKEDDEPART")]
        public string CheckedDepart { get; set; }
        /// <summary>
        /// �Ƿ�ͬ���糧�ɼ���0����1���ǣ�
        /// </summary>
        [Column("ISSYNVIEW")]
        public string IsSynView { get; set; }
        /// <summary>
        /// ���з��յ����
        /// </summary>
        /// <returns></returns>
        [Column("COUNT")]
        public int Count { get; set; }
        /// <summary>
        /// Υ������
        /// </summary>
        [NotMapped]
        public int? WzCount { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        [NotMapped]
        public int? WtCount { get; set; }

        /// <summary>
        /// ���з��յ����
        /// </summary>
        /// <returns></returns>

        [NotMapped]
        [DefaultValue(0)]
        public decimal? Count1 { get; set; }
        /// <summary>
        /// Υ������
        /// </summary>
        [NotMapped]
        [DefaultValue(0)]
        public decimal? WzCount1 { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        [NotMapped]
        [DefaultValue(0)]
        public decimal? WtCount1 { get; set; }

        /// <summary>
        /// �Ѽ����յ����
        /// </summary>
        /// <returns></returns>
        [Column("SOLVECOUNT")]
        public double SolveCount { get; set; }
        /// <summary>
        /// ����鳤
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMANAGEMAN")]
        public string CheckManageMan { get; set; }

        /// <summary>
        /// ����鳤ID
        /// </summary>
        /// <returns></returns>
        [Column("CHECKMANAGEMANID")]
        public string CheckManageManID { get; set; }

        /// <summary>
        /// ��������ڲ���code���߼�鲿��code
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTCODE")]
        public string CheckDeptCode { get; set; }

        /// <summary>
        /// �Ѽ����Ա
        /// </summary>
        /// <returns></returns>
        [Column("SOLVEPERSON")]
        public string SolvePerson { get; set; }
        /// <summary>
        /// ����Ա
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERS")]
        public string CheckUsers { get; set; }
        /// <summary>
        /// ����ԱId
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERIDS")]
        public string CheckUserIds { get; set; }

        /// <summary>
        /// �Ѽ����Ա����
        /// </summary>
        /// <returns></returns>
        [Column("SOLVEPERSONNAME")]
        public string SolvePersonName { get; set; }

        /// <summary>
        ///�Ƿ�ʡ��˾�·��������
        /// </summary>
        /// <returns></returns>
        [Column("DATATYPE")]
        public int? DataType { get; set; }

        /// <summary>
        ///��������ˣ�����ö��ŷָ���
        /// </summary>
        /// <returns></returns>
        [Column("RECEIVEUSERS")]
        public string ReceiveUsers { get; set; }
        /// <summary>
        ///���������Id������ö��ŷָ���
        /// </summary>
        /// <returns></returns>
        [Column("RECEIVEUSERIDS")]
        public string ReceiveUserIds { get; set; }

        /// <summary>
        ///��鸺����
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSER")]
        public string DutyUser { get; set; }
        /// <summary>
        ///��鸺����Id
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        ///�����˵�λ
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPT")]
        public string DutyDept { get; set; }


        /// <summary>
        ///���������
        /// </summary>
        /// <returns></returns>
        [Column("ALLOTUSER")]
        public string AllotUser { get; set; }
        /// <summary>
        ///��������˵�λ
        /// </summary>
        /// <returns></returns>
        [Column("ALLOTDEPT")]
        public string AllotDept { get; set; }
        /// <summary>
        ///�������ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ALLOTTIME")]
        public DateTime? AllotTime { get; set; }

        /// <summary>
        ///���Ҫ��
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        ///״̬��0�������䣬1�������ƣ�2�������ƣ�
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        [DefaultValue(0)]
        public int Status { get; set; }
        /// <summary>
        ///�Ƿ��ύ��0����1���ǣ�
        /// </summary>
        /// <returns></returns>
        [Column("ISSUBMIT")]
        [DefaultValue(0)]
        public int IsSubmit { get; set; }

        /// <summary>
        ///�����������¼Id(����)
        /// </summary>
        /// <returns></returns>
        [Column("RID")]
        public string RId { get; set; }
        /// <summary>
        ///�Ƿ������Լƻ���0����1���ǣ�
        /// </summary>
        /// <returns></returns>
        [Column("ISAUTO")]
        public int? IsAuto { get; set; }
        /// <summary>
        ///�������ͣ�0�����죬1�����ܣ�2�����£�
        /// </summary>
        /// <returns></returns>
        [Column("AUTOTYPE")]
        public int? AutoType { get; set; }
        /// <summary>
        ///�Ƿ�����˫�ݣ�0����1���ǣ�
        /// </summary>
        /// <returns></returns>
        [Column("ISSKIP")]
        public int? IsSkip { get; set; }
        /// <summary>
        ///������ʾ
        /// </summary>
        /// <returns></returns>
        [Column("DISPLAY")]
        public string Display { get; set; }
        /// <summary>
        ///���ڣ������Ӣ�Ķ��ŷָ���
        /// </summary>
        /// <returns></returns>
        [Column("WEEKS")]
        public string Weeks { get; set; }
        /// <summary>
        ///���ڻ����ڣ�0�������ڣ�1�������ڣ�
        /// </summary>
        /// <returns></returns>
        [Column("SELTYPE")]
        public int? SelType { get; set; }
        /// <summary>
        ///�ڼ��ܣ������Ӣ�Ķ��ŷָ���
        /// </summary>
        /// <returns></returns>
        [Column("THWEEKS")]
        public string ThWeeks { get; set; }
        /// <summary>
        ///���ڣ������Ӣ�Ķ��ŷָ���
        /// </summary>
        /// <returns></returns>
        [Column("DAYS")]
        public string Days { get; set; }
        /// <summary>
        ///�·ݣ������Ӣ�Ķ��ŷָ���
        /// </summary>
        /// <returns></returns>
        [Column("MONTHS")]
        public string Months { get; set; }
        /// <summary>
        ///����
        /// </summary>
        /// <returns></returns>
        [Column("ROUNDS")]
        public string Rounds { get; set; }
        /// <summary>
        ///�Ƿ���������Լƻ���1����ֹ��0��ִ�У�
        /// </summary>
        /// <returns></returns>
        [Column("ISOVER")]
        public int? IsOver { get; set; }

        /// <summary>
        ///�������
        /// </summary>
        /// <returns></returns>
        [Column("AREANAME")]
        public string AreaName { get; set; }
        [Required]
        [DisplayName("���Ŀ��")]
        [MaxLength(10)]
        /// <summary>
        ///���Ŀ��
        /// </summary>
        /// <returns></returns>
        [Column("AIM")]
        public string Aim { get; set; }

        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ?Guid.NewGuid().ToString(): ID;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
           this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
           this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
           this.DataType = DataType != 1 ? 0 : DataType;
           this.IsOver = IsOver != 1 ? 0 : IsOver;
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
        public void insertInto(SaftyCheckDataRecordEntity se, Operator user)
        {
            se.ID = Guid.NewGuid().ToString();
            se.CreateDate = DateTime.Now;
            se.CreateUserId = user.UserId;
            se.CreateUserName = user.UserName;
            se.CreateUserDeptCode = user.DeptCode;
            se.CreateUserOrgCode = user.OrganizeCode;
        }
        #endregion
    }
}