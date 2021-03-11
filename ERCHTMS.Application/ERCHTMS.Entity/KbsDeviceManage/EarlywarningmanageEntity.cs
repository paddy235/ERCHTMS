using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.KbsDeviceManage
{
    /// <summary>
    /// �� ����Ԥ�����͹���
    /// </summary>
    [Table("BIS_EARLYWARNINGMANAGE")]
    public class EarlywarningmanageEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string REMARK { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// �Ƿ�����
        /// </summary>
        /// <returns></returns>
        [Column("ISENABLE")]
        public int? Isenable { get; set; }
        /// <summary>
        /// Ԥ�����
        /// </summary>
        /// <returns></returns>
        [Column("WARNINGRESULT")]
        public string Warningresult { get; set; }
        /// <summary>
        /// ָ�굥λ
        /// </summary>
        /// <returns></returns>
        [Column("INDEXUNIT")]
        public string Indexunit { get; set; }
        /// <summary>
        /// Ԥ������
        /// </summary>
        /// <returns></returns>
        [Column("CONDITION")]
        public string Condition { get; set; }
        /// <summary>
        /// ����ָ��
        /// </summary>
        /// <returns></returns>
        [Column("INDEXNOM")]
        public int? Indexnom { get; set; }
         /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("FUNCTIONNAME")]
        public string FunctionName { get; set; }
        
     
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.Isenable = 1;
            this.ID = string.IsNullOrEmpty(ID)?Guid.NewGuid().ToString():ID;
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
                       this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
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
        }
        #endregion
    }
}