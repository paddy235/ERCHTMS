using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;

namespace ERCHTMS.Entity.SafetyLawManage
{
    /// <summary>
    /// �� ������׼�ƶȷ����
    /// </summary>
    [Table("HRS_STDSYSTYPE")]
    public class StdsysTypeEntity : BSEntity
    {
        #region ʵ���Ա        
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("NAME")]
        public string Name { get; set; }        
        /// <summary>
        /// �ϼ�id
        /// </summary>
        /// <returns></returns>
        [Column("PARENTID")]
        public string ParentId { get; set; }        
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("CODE")]
        public string Code { get; set; }
        /// <summary>
        /// ���ݷ�Χ
        /// </summary>
        /// <returns></returns>
        [Column("SCOPE")]
        public string Scope { get; set; }
        #endregion
    }
}