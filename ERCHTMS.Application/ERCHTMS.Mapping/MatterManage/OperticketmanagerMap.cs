using ERCHTMS.Entity.MatterManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.MatterManage
{
    /// <summary>
    /// 描 述：开票管理入厂开票
    /// </summary>
    public class OperticketmanagerMap : EntityTypeConfiguration<OperticketmanagerEntity>
    {
        public OperticketmanagerMap()
        {
            #region 表、主键
            //表
            this.ToTable("WL_OPERTICKETMANAGER");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
