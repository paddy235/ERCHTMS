using ERCHTMS.Entity.LaborProtectionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LaborProtectionManage
{
    /// <summary>
    /// 描 述：劳动防护发放记录表
    /// </summary>
    public class LaborissueMap : EntityTypeConfiguration<LaborissueEntity>
    {
        public LaborissueMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LABORISSUE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
