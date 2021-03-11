using ERCHTMS.Entity.ToolEquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.ToolEquipmentManage
{
    /// <summary>
    /// 描 述：工器具基础信息表
    /// </summary>
    public class ToolequipmentMap : EntityTypeConfiguration<ToolequipmentEntity>
    {
        public ToolequipmentMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_TOOLEQUIPMENT");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
