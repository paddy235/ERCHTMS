using ERCHTMS.Entity.EquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EquipmentManage
{
    /// <summary>
    /// 描 述：危险化学品库存
    /// </summary>
    public class DangerChemicalsEntityMap : EntityTypeConfiguration<DangerChemicalsEntity>
    {
        public DangerChemicalsEntityMap()
        {
            #region 表、主键
            //表
            this.ToTable("XLD_DANGEROUSCHEMICAL");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
