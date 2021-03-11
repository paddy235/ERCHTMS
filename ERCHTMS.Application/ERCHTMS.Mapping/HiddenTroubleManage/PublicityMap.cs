using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：
    /// </summary>
    public class PublicityMap : EntityTypeConfiguration<PublicityEntity>
    {
        public PublicityMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_PUBLICITY");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}