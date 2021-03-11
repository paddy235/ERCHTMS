using ERCHTMS.Entity.EngineeringManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EngineeringManage
{
    /// <summary>
    /// 描 述：危大工程管理
    /// </summary>
    public class PerilEngineeringMap : EntityTypeConfiguration<PerilEngineeringEntity>
    {
        public PerilEngineeringMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_PERILENGINEERING");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
