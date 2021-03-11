using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：资质审查人员证件表
    /// </summary>
    public class CertificateinspectorsMap : EntityTypeConfiguration<CertificateinspectorsEntity>
    {
        public CertificateinspectorsMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_CERTIFICATEINSPECTORS");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
