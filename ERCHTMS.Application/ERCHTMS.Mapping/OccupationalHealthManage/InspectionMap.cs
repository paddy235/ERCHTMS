using ERCHTMS.Entity.OccupationalHealthManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业机构检测
    /// </summary>
    public class InspectionMap : EntityTypeConfiguration<InspectionEntity>
    {
        public InspectionMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_INSPECTION");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
