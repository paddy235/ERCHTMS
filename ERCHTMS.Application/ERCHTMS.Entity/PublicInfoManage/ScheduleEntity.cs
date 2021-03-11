using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERCHTMS.Entity.PublicInfoManage
{
    /// <summary>
    /// �� �����ճ̹���
    /// </summary>
    public class ScheduleEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// �ճ�����
        /// </summary>
        /// <returns></returns>
         [Column("SCHEDULEID")]
        public string ScheduleId { get; set; }
        /// <summary>
        /// �ճ�����
        /// </summary>
        /// <returns></returns>
          [Column("SCHEDULENAME")]
        public string ScheduleName { get; set; }
        /// <summary>
        /// �ճ�����
        /// </summary>
        /// <returns></returns>��
         [Column("SCHEDULECONTENT")]
        public string ScheduleContent { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        /// <returns></returns>
       [Column("CATEGORY")]
        public string Category { get; set; }
        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <returns></returns>
       [Column("STARTDATE")]
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        /// <returns></returns>
         [Column("STARTTIME")]
        public string StartTime { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
         [Column("ENDDATE")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
         [Column("ENDTIME")]
        public string EndTime { get; set; }
        /// <summary>
        /// ��ǰ����
        /// </summary>
        /// <returns></returns>
         [Column("EARLY")]
        public int? Early { get; set; }
        /// <summary>
        /// �ʼ�����
        /// </summary>
        /// <returns></returns>
         [Column("ISMAILALERT")]
        public int? IsMailAlert { get; set; }
        /// <summary>
        /// �ֻ�����
        /// </summary>
          [Column("ISMOBILEALERT")]
        public int? IsMobileAlert { get; set; }
        /// <summary>
        /// ΢������
        /// </summary>
        /// <returns></returns>
        [Column("ISWECHATALERT")]
        public int? IsWeChatAlert { get; set; }
        /// <summary>
        /// �ճ�״̬
        /// </summary>
        /// <returns></returns>
         [Column("SCHEDULESTATE")]
        public int? ScheduleState { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("SORTCODE")]
        public int? SortCode { get; set; }
        /// <summary>
        /// ɾ�����
        /// </summary>
        /// <returns></returns>
         [Column("DELETEMARK")]
        public int? DeleteMark { get; set; }
        /// <summary>
        /// ��Ч��־
        /// </summary>
        /// <returns></returns>
          [Column("ENABLEDMARK")]
        public int? EnabledMark { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
         [Column("DESCRIPTION")]
        public string Description { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
          [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �����û�����
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �޸�����
        /// </summary>
        /// <returns></returns>
         [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// �޸��û�����
        /// </summary>
        /// <returns></returns>
         [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <returns></returns>
         [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ScheduleId = Guid.NewGuid().ToString();
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ScheduleId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}