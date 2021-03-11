using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    /// <summary>
    /// �� �����嶨��ȫ��� �������ձ�
    /// </summary>
    [Table("BIS_FIVESAFETYCHECKAUDIT")]
    public class FivesafetycheckauditEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTCONTENT")]
        public string ACCEPTCONTENT { get; set; }
        /// <summary>
        /// ���ս��  
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTREUSLT")]
        public string ACCEPTREUSLT { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("BEIZHU")]
        public string BEIZHU { get; set; }
        /// <summary>
        /// ʵ�����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ACTUALDATE")]
        public DateTime? ACTUALDATE { get; set; }
        /// <summary>
        /// ���Ľ��
        /// </summary>
        /// <returns></returns>
        [Column("ACTIONRESULT")]
        public string ACTIONRESULT { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTUSERID")]
        public string ACCEPTUSERID { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTUSER")]
        public string ACCEPTUSER { get; set; }
        /// <summary>
        /// Ҫ�����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("FINISHDATE")]
        public DateTime? FINISHDATE { get; set; }
        /// <summary>
        /// ���β���ID
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPTID")]
        public string DUTYDEPTID { get; set; }
        /// <summary>
        /// ���β���
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPT")]
        public string DUTYDEPT { get; set; }
        /// <summary>
        /// ���β���code
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPTCODE")]
        public string DUTYDEPTCODE { get; set; }
        /// <summary>
        /// ������D
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERID")]
        public string DUTYUSERID { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERNAME")]
        public string DUTYUSERNAME { get; set; }
        /// <summary>
        /// ���Ĵ�ʩ
        /// </summary>
        /// <returns></returns>
        [Column("ACTIONCONTENT")]
        public string ACTIONCONTENT { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("FINDQUESTION")]
        public string FINDQUESTION { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
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
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
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
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// ����������id
        /// </summary>
        /// <returns></returns>
        [Column("CHECKID")]
        public string CHECKID { get; set; }

        /// <summary>
        /// ��������  0:�������ֱ������ͨ��  1����������������
        /// </summary>
        /// <returns></returns>
        [Column("CHECKPASS")]
        public string CHECKPASS { get; set; }

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
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}