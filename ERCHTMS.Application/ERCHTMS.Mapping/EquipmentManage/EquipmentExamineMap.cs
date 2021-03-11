using ERCHTMS.Entity.EquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EquipmentManage
{
    /// <summary>
    /// 描 述：特种设备检验记录
    /// </summary>
    public class EquipmentExamineMap : EntityTypeConfiguration<EquipmentExamineEntity>
    {
        public EquipmentExamineMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_EQUIPMENTEXAMINE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
