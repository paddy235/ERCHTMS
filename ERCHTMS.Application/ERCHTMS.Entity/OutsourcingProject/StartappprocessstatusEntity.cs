using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// �� ������������״̬��
    /// </summary>
    [Table("EPG_STARTAPPPROCESSSTATUS")]
    public class StartappprocessstatusEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// ��������Id
        /// </summary>
        /// <returns></returns>
        [Column("STARTAPPLYID")]
        public string STARTAPPLYID { get; set; }
        /// <summary>
        /// �豸����������״̬(0:δ���1:���)
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTTOOLSTATUS")]
        public string EQUIPMENTTOOLSTATUS { get; set; }
        /// <summary>
        /// ��ͬ��Э��״̬(0:δ���1:���)
        /// </summary>
        /// <returns></returns>
        [Column("PACTSTATUS")]
        public string PACTSTATUS { get; set; }
        /// <summary>
        /// ��ȫ��������״̬(0:δ���1:���)
        /// </summary>
        /// <returns></returns>
        [Column("TECHNICALSTATUS")]
        public string TECHNICALSTATUS { get; set; }
        /// <summary>
        /// ��������״̬(0:δ���1:���)
        /// </summary>
        /// <returns></returns>
        [Column("THREETWOSTATUS")]
        public string THREETWOSTATUS { get; set; }
        /// <summary>
        /// �������Id
        /// </summary>
        /// <returns></returns>
        [Column("OUTENGINEERID")]
        public string OUTENGINEERID { get; set; }
        /// <summary>
        /// ��֤��״̬(0:δ���1:���)
        /// </summary>
        /// <returns></returns>
        [Column("SECURITYSTATUS")]
        public string SECURITYSTATUS { get; set; }
        /// <summary>
        /// �������״̬(0:δ���1:���)
        /// </summary>
        /// <returns></returns>
        [Column("EXAMSTATUS")]
        public string EXAMSTATUS { get; set; }
        /// <summary>
        /// ��ע�ֶ�״̬(0:δ���1:���)
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string REMARK { get; set; }
        /// <summary>
        /// �����λId
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTID")]
        public string OUTPROJECTID { get; set; }

        /// <summary>
        /// ��Ա���״̬(0:δ���1:���)
        /// </summary>
        /// <returns></returns>
        [Column("PEOPLESTATUS")]
        public string PEOPLESTATUS { get; set; }
        
        /// <summary>
        /// ��ͬ���״̬(0:δ���1:���)
        /// </summary>
        /// <returns></returns>
        [Column("COMPACTSTATUS")]
        public string COMPACTSTATUS { get; set; }
        
        /// <summary>
        /// �����豸���״̬(0:δ���1:���)
        /// </summary>
        /// <returns></returns>
        [Column("SPTOOLSSTATUS")]
        public string SPTOOLSSTATUS { get; set; }
        
            
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;

        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
        }
        #endregion
    }
}