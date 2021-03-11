using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// 描 述：重点防火部位
    /// </summary>
    public class KeyPartMap : EntityTypeConfiguration<KeyPartEntity>
    {
        public KeyPartMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_KEYPART");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
