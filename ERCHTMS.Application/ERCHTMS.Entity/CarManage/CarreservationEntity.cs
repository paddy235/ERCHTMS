using System;
using System.Collections.Generic;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.CarManage
{
    /// <summary>
    /// �� �����೵ԤԼ��¼
    /// </summary>
    [Table("BIS_CARRESERVATION")]
    public class CarreservationEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 0����12�� 1����17��
        /// </summary>
        /// <returns></returns>
        [Column("TIME")]
        public int? Time { get; set; }
        /// <summary>
        /// ԤԼ����
        /// </summary>
        /// <returns></returns>
        [Column("RESDATE")]
        public DateTime? RESDate { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// ��������ID
        /// </summary>
        /// <returns></returns>
        [Column("CID")]
        public string CID { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// ������ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ���복�����ƺ�
        /// </summary>
        /// <returns></returns>
        [Column("CARNO")]
        public string CarNo { get; set; }

        /// <summary>
        /// ��ʼ�ص�
        /// </summary>
        /// <returns></returns>
        [Column("SADDRESS")]
        public string Saddress { get; set; }

        /// <summary>
        /// �����ص�
        /// </summary>
        /// <returns></returns>
        [Column("EADDRESS")]
        public string Eaddress { get; set; }

        /// <summary>
        /// �������� 0˾��ԤԼ 1��ͨԱ��ԤԼ
        /// </summary>
        [Column("DATATYPE")]
        public int DataType { get; set; }

        /// <summary>
        /// ԤԼ����Id
        /// </summary>
        [Column("BASEID")]
        public string BaseId { get; set; }


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
            this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
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
        }
        #endregion
    }

    public class ReserVation
    {
        /// <summary>
        /// ���ƺ�
        /// </summary>
        public string CarNo { get; set; }
        /// <summary>
        /// ����ID
        /// </summary>
        public string CID { get; set; }
        /// <summary>
        /// �����ͺ�
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public int NumberLimit { get; set; }

        /// <summary>
        /// ԤԼ����
        /// </summary>
        public List<ReserList> RList { get; set; }

    }

    public class ReserList
    {
        /// <summary>
        /// ʱ�� 0Ϊ���� 1Ϊ����
        /// </summary>
        public int Time { get; set; }
        /// <summary>
        /// ԤԼ����
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// ���ڸ�ʽ�ַ���
        /// </summary>
        public string DateStr { get; set; }

        /// <summary>
        /// ��ǰ�û��Ƿ�ԤԼ
        /// </summary>
        public int IsReser { get; set; }
    }
}