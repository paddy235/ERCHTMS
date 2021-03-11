using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// 描 述：消防队伍
    /// </summary>
    public class FireTroopsMap : EntityTypeConfiguration<FireTroopsEntity>
    {
        public FireTroopsMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_FIRETROOPS");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
