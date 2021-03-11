using ERCHTMS.Entity.EquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EquipmentManage
{
    /// <summary>
    /// 描 述：特种设备安全技术交底
    /// </summary>
    public class EquipmentTechnologyMap : EntityTypeConfiguration<EquipmentTechnologyEntity>
    {
        public EquipmentTechnologyMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_EQUIPMENTTECHNOLOGY");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
