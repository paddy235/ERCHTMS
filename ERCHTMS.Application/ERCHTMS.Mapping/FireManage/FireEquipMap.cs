using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// 描 述：消防队伍配备
    /// </summary>
    public class FireEquipMap : EntityTypeConfiguration<FireEquipEntity>
    {
        public FireEquipMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_FIREEQUIP");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
