using ERCHTMS.Entity.EquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EquipmentManage
{
    /// <summary>
    /// 描 述：特种设备基本信息表
    /// </summary>
    public class SpecialEquipmentMap : EntityTypeConfiguration<SpecialEquipmentEntity>
    {
        public SpecialEquipmentMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SPECIALEQUIPMENT");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
