using System.Data.Entity.ModelConfiguration;
using ERCHTMS.Entity.PersonManage;

namespace ERCHTMS.Mapping.PersonManage
{
    public class EarlyWarningMap : EntityTypeConfiguration<EarlyWarningEntity>
    {
        public EarlyWarningMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HIKCAMERAWARNING");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
