using ERCHTMS.Entity.Observerecord;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.Observerecord
{
    /// <summary>
    /// 描 述：观察计划任务分解
    /// </summary>
    public class ObsplanworkMap : EntityTypeConfiguration<ObsplanworkEntity>
    {
        public ObsplanworkMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_OBSPLANWORK");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}