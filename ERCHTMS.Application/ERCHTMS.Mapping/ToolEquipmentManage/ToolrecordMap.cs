using ERCHTMS.Entity.ToolEquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.ToolEquipmentManage
{
    /// <summary>
    /// 描 述：电动工器具试验记录
    /// </summary>
    public class ToolrecordMap : EntityTypeConfiguration<ToolrecordEntity>
    {
        public ToolrecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_TOOLRECORD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
