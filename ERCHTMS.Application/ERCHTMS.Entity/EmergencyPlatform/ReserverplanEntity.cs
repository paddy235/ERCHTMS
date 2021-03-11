using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ��Ԥ��
    /// </summary>
    [Table("MAE_RESERVERPLAN")]
    public class ReserverplanEntity : BaseEntity
    {
        #region ʵ���Ա

 

        [Column("ORGXZNAME")]
        public string ORGXZNAME { get; set; }
        [Column("ORGXZTYPE")]
        public int? ORGXZTYPE { get; set; }


        [Column("FILEPS")]
        public string FILEPS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]


        public string ID { get; set; }
        [Column("ISAUDITNAME")]
        public string ISAUDITNAME { get; set; }
        [Column("PLANTYPENAME")]
        public string PLANTYPENAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("NAME")]
        public string NAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("PLANTYPE")]
        public string PLANTYPE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTID_BZ")]
        public string DEPARTID_BZ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTNAME_BZ")]
        public string DEPARTNAME_BZ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("USERID_BZ")]
        public string USERID_BZ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ISAUDIT")]
        public string ISAUDIT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTID_SH")]
        public string DEPARTID_SH { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTNAME_SH")]
        public string DEPARTNAME_SH { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("USERID_SH")]
        public string USERID_SH { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DATATIME_BZ")]
        public DateTime? DATATIME_BZ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DATATIME_SH")]
        public DateTime? DATATIME_SH { get; set; }
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
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
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
        [Column("USERNAME_BZ")]
        public string USERNAME_BZ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME_SH")]
        public string USERNAME_SH { get; set; }

        public string FILES { get; set; }
        /// <summary>
        /// �ֳ����÷�������
        /// </summary>
            [Column("PLANTYPEHANDLE")]
        public string PlanTypeHandle { get; set; }
            [Column("PLANTYPEHANDLECODE")]
            public string PlanTypeHandleCode { get; set; }
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