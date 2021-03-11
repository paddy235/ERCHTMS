using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：1.脚手架搭设、验收、拆除申请2.脚手架搭设、验收、拆除审批
    /// </summary>
    public class ScaffoldMap : EntityTypeConfiguration<ScaffoldEntity>
    {
        public ScaffoldMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SCAFFOLD");
            //主键
            this.HasKey(t => t.Id);
            this.Ignore(t => t.SpecialtyTypeName);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
