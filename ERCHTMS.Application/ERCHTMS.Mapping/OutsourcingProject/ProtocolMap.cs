using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：协议
    /// </summary>
    public class ProtocolMap : EntityTypeConfiguration<ProtocolEntity>
    {
        public ProtocolMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_PROTOCOL");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
