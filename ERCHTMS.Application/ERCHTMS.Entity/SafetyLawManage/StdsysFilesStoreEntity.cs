using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SafetyLawManage
{
    /// <summary>
    /// �� ������׼�ƶ��ղ��ļ�
    /// </summary>
    [Table("HRS_STDSYSSTOREFILES")]
    public class StdsysFilesStoreEntity : BSEntity
    {
        #region ʵ���Ա     
        /// <summary>
        /// �û����
        /// </summary>                           
        [Column("USERID")]        
        public string UserId { get; set; }
        /// <summary>
        /// ��׼�ƶȱ��
        /// </summary>
        [Column("STDSYSID")]
        public string StdSysId { get; set; }
        #endregion
    }
}


