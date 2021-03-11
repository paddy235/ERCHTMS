using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：门禁使用用户表
    /// </summary>
    public class HikaccessuserMap : EntityTypeConfiguration<HikaccessuserEntity>
    {
        public HikaccessuserMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HIKACCESSUSER");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
