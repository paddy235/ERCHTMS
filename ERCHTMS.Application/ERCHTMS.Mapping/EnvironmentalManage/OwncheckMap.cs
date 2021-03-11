using ERCHTMS.Entity.EnvironmentalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EnvironmentalManage
{
    /// <summary>
    /// 描 述：自行检测
    /// </summary>
    public class OwncheckMap : EntityTypeConfiguration<OwncheckEntity>
    {
        public OwncheckMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_OWNCHECK");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
