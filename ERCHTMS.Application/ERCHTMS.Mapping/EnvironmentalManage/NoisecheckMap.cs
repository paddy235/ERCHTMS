using ERCHTMS.Entity.EnvironmentalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EnvironmentalManage
{
    /// <summary>
    /// 描 述：噪音检测
    /// </summary>
    public class NoisecheckMap : EntityTypeConfiguration<NoisecheckEntity>
    {
        public NoisecheckMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_NOISECHECK");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
