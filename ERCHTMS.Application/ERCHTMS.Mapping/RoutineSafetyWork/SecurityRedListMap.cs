using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：安全红黑榜
    /// </summary>
    public class SecurityRedListMap : EntityTypeConfiguration<SecurityRedListEntity>
    {
        public SecurityRedListMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SECURITYREDLIST");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
