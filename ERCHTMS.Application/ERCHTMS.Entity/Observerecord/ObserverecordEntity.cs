using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.Observerecord
{
    /// <summary>
    /// �� �����۲��¼��
    /// </summary>
    [Table("BIS_OBSERVERECORD")]
    public class ObserverecordEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ��ҵ����Id
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREAID")]
        public string WorkAreaId { get; set; }
        /// <summary>
        /// ��ҵ�ص�
        /// </summary>
        /// <returns></returns>
        [Column("WORKPLACE")]
        public string WorkPlace { get; set; }
        /// <summary>
        /// �۲����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("OBSENDTIME")]
        public DateTime? ObsEndTime { get; set; }
        /// <summary>
        /// �۲���ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("OBSSTARTTIME")]
        public DateTime? ObsStartTime { get; set; }
        /// <summary>
        /// �۲���Ա
        /// </summary>
        /// <returns></returns>
        [Column("OBSPERSON")]
        public string ObsPerson { get; set; }
        /// <summary>
        /// �۲���ԱId
        /// </summary>
        /// <returns></returns>
        [Column("OBSPERSONID")]
        public string ObsPersonId { get; set; }
        /// <summary>
        /// �۲���������
        /// </summary>
        /// <returns></returns>
        [Column("OBSGIST")]
        public string ObsGist { get; set; }
        /// <summary>
        /// �۲�����
        /// </summary>
        /// <returns></returns>
        [Column("OBSGISTVALUE")]
        public string ObsGistValue { get; set; }
        /// <summary>
        /// �۲�ƻ�Id
        /// </summary>
        /// <returns></returns>
        [Column("OBSPLANID")]
        public string ObsPlanId { get; set; }
        /// <summary>
        /// ��ͨʱ��
        /// </summary>
        /// <returns></returns>
        [Column("LINKUPTIME")]
        public DateTime? LinkUpTime { get; set; }
        /// <summary>
        /// ��ͨ�ص�
        /// </summary>
        /// <returns></returns>
        [Column("LINKUPPLACE")]
        public string LinkUpPlace { get; set; }
        /// <summary>
        /// �μ���Ա
        /// </summary>
        /// <returns></returns>
        [Column("LINKPEOPLE")]
        public string LinkPeople { get; set; }
        /// <summary>
        /// �μ���ԱId
        /// </summary>
        /// <returns></returns>
        [Column("LINKPEOPLEID")]
        public string LinkPeopleId { get; set; }
        /// <summary>
        /// ��ͨ����
        /// </summary>
        /// <returns></returns>
        [Column("LINKUPCONTENT")]
        public string LinkUpContent { get; set; }
        /// <summary>
        /// ��ͨ���
        /// </summary>
        /// <returns></returns>
        [Column("LINKUPRESULT")]
        public string LinkUpResult { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("LINKUPREMARK")]
        public string LinkUpRemark { get; set; }
        /// <summary>
        /// ���ս���
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTRESULT")]
        public string AcceptResult { get; set; }
        /// <summary>
        /// ������Ա
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTPERSON")]
        public string AcceptPerson { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTPERSONID")]
        public string AcceptPersonId { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTTIME")]
        public DateTime? AcceptTime { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("RECORDREMARK")]
        public string RecordRemark { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREA")]
        public string WorkArea { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// �Ǽǲ���
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPT")]
        public string CreateUserDept { get; set; }
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
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKNAME")]
        public string WorkName { get; set; }
        /// <summary>
        /// ��ҵ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNIT")]
        public string WorkUnit { get; set; }
        /// <summary>
        /// ��ҵ����Id
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNITID")]
        public string WorkUnitId { get; set; }
        /// <summary>
        /// ��ҵ����Code
        /// </summary>
        /// <returns></returns>
         [Column("WORKUNITCODE")]
        public string WorkUnitCode { get; set; }
        /// <summary>
        /// ��ҵ��Ա
        /// </summary>
        /// <returns></returns>
        [Column("WORKPEOPLE")]
        public string WorkPeople { get; set; }
        /// <summary>
        /// ��ҵ��ԱId
        /// </summary>
        /// <returns></returns>
        [Column("WORKPEOPLEID")]
        public string WorkPeopleId { get; set; }
         [Column("ISCOMMIT")]
        public int IsCommit { get; set; }
        /// <summary>
        /// �۲�ƻ�����ֽ�Id
        /// </summary>
           [Column("OBSPLANFJID")]
         public string ObsPlanfjid { get; set; }
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
           this.CreateUserDept = OperatorProvider.Provider.Current().DeptName;
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