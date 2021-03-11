using ERCHTMS.Entity.EquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EquipmentManage
{
    /// <summary>
    /// 描 述：维护保养记录表
    /// </summary>
    public class MaintainingRecordMap : EntityTypeConfiguration<MaintainingRecordEntity>
    {
        public MaintainingRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_MAINTAININGRECORD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
