using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：电厂模板管理
    /// </summary>
    public class TempConfigMap : EntityTypeConfiguration<TempConfigEntity>
    {
        public TempConfigMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_TEMPMANAGE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
