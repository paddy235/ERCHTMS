using ERCHTMS.Entity.OccupationalHealthManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病危害告知卡
    /// </summary>
    public class StaffinformcardMap : EntityTypeConfiguration<StaffinformcardEntity>
    {
        public StaffinformcardMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_STAFFINFORMCARD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
