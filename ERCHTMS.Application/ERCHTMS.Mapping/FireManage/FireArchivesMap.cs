using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// 描 述：消防档案
    /// </summary>
    public class FireArchivesMap : EntityTypeConfiguration<FireArchivesEntity>
    {
        public FireArchivesMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_FIREARCHIVES");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
