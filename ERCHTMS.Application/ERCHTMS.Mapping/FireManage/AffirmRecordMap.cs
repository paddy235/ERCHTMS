using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// 描 述：消防巡查确认记录
    /// </summary>
    public class AffirmRecordMap : EntityTypeConfiguration<AffirmRecordEntity>
    {
        public AffirmRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_AFFIRMRECORD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
