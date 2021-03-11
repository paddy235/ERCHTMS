using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：模板管理
    /// </summary>
    public class TempmanagerMap : EntityTypeConfiguration<TempmanagerEntity>
    {
        public TempmanagerMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_TEMPMANAGER");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
