using ERCHTMS.Entity.EngineeringManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EngineeringManage
{
    /// <summary>
    /// 描 述：危大工程项目设置
    /// </summary>
    public class EngineeringSettingMap : EntityTypeConfiguration<EngineeringSettingEntity>
    {
        public EngineeringSettingMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_ENGINEERINGSETTING");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
