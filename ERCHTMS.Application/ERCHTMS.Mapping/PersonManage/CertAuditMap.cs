using ERCHTMS.Entity.PersonManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PersonManage
{
    /// <summary>
    /// 描 述：人员证书
    /// </summary>
    public class CertAuditMap : EntityTypeConfiguration<CertAuditEntity>
    {
        public CertAuditMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_CERTAUDIT");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
