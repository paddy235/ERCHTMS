using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：危险点预控措施
    /// </summary>
    public class MeasuresMap : EntityTypeConfiguration<OutMeasuresEntity>
    {
        public MeasuresMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_MEASURES");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
