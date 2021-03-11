using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// �� �������������˱�
    /// </summary>
    [Table("EPG_APTITUDEINVESTIGATEAUDIT")]
    public class AptitudeinvestigateauditEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// ҵ�������Id
        /// </summary>
        /// <returns></returns>
        [Column("APTITUDEID")]
        public string APTITUDEID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("APPLYID")]
        public string APPLYID { get; set; }

        /// <summary>
        /// ��˽����0 ͨ�� 1 ��ͨ�� 2 �����
        /// </summary>
        /// <returns></returns>
        [Column("AUDITRESULT")]
        public string AUDITRESULT { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [Column("AUDITPEOPLEID")]
        public string AUDITPEOPLEID { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("AUDITTIME")]
        public DateTime? AUDITTIME { get; set; }
        /// <summary>
        /// ��˲���
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDEPTID")]
        public string AUDITDEPTID { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("AUDITOPINION")]
        public string AUDITOPINION { get; set; }
        /// <summary>
        /// ��ע�ֶ�
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string REMARK { get; set; }

        /// <summary>
        /// ����ͼ�Ƿ���ʾ
        /// </summary>
        /// <returns></returns>
        [Column("DISABLE")]
        public string Disable { get; set; }

        /// <summary>
        /// ��˲���
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDEPT")]
        public string AUDITDEPT { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [Column("AUDITPEOPLE")]
        public string AUDITPEOPLE { get; set; }
        /// <summary>
        /// ���ǩ��
        /// </summary>
        /// <returns></returns>
        [Column("AUDITSIGNIMG")]
        public string AUDITSIGNIMG { get; set; }
        /// <summary>
        /// �������Id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }
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