using ERCHTMS.Entity.EquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EquipmentManage
{
    /// <summary>
    /// 描 述：普通设备基本信息表
    /// </summary>
    public class EquipmentMap : EntityTypeConfiguration<EquipmentEntity>
    {
        public EquipmentMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_EQUIPMENT");
            //主键
            this.HasKey(t => t.Id);
            this.Ignore(t => t.AutoId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
