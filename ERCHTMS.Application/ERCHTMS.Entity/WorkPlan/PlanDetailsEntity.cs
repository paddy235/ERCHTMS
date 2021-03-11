using ERCHTMS.Code;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERCHTMS.Entity.WorkPlan
{
    /// <summary>
    /// �� ����EHS�ƻ�������ϸ��
    /// </summary>
    [Table("HRS_PLANDETAILS")]
    public class PlanDetailsEntity : BaseEntity
    {
        #region Ĭ���ֶ�
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        #endregion

        #region ʵ���Ա 
        /// <summary>
        /// ��������
        /// </summary>
        [Column("CONTENTS")]
        public string Contents { get; set; }
        /// <summary>
        /// ������id
        /// </summary>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        [Column("DUTYUSERNAME")]
        public string DutyUserName { get; set; }
        /// <summary>
        /// ���β���id
        /// </summary>
        [Column("DUTYDEPARTID")]
        public string DutyDepartId { get; set; }
        /// <summary>
        /// ���β���
        /// </summary>
        [Column("DUTYDEPARTNAME")]
        public string DutyDepartName { get; set; }
        /// <summary>
        /// �ƻ��������
        /// </summary>
        [Column("PLANFINDATE")]
        public DateTime? PlanFinDate { get; set; }
        /// <summary>
        /// ʵ���������
        /// </summary>
        [Column("REALFINDATE")]
        public DateTime? RealFinDate { get; set; }
        /// <summary>
        /// �����׼id
        /// </summary>
        [Column("STDID")]
        public string StdId { get; set; }
        /// <summary>
        /// �����׼����
        /// </summary>
        [Column("STDNAME")]
        public string StdName { get; set; }
        /// <summary>
        /// �Ƿ�ȡ���ƻ�
        /// </summary>
        [Column("ISCANCEL")]
        public string IsCancel { get; set; }
        /// <summary>
        /// �䶯ԭ��
        /// </summary>
        [Column("CHANGEREASON")]
        public string ChangeReason { get; set; }
        /// <summary>
        /// �ƻ�id
        /// </summary>
        [Column("APPLYID")]
        public string ApplyId { get; set; }
        /// <summary>
        /// ���ü�¼id
        /// </summary>
        [Column("BASEID")]
        public string BaseId { get; set; }
        /// <summary>
        /// ��¼�Ƿ�仯
        /// </summary>
        [Column("ISCHANGED")]
        public int IsChanged { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        [Column("ISMARK")]
        public int IsMark { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
            //this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}