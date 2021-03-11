using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患整改信息表
    /// </summary>
    public class HTChangeInfoMap : EntityTypeConfiguration<HTChangeInfoEntity>
    {
        public HTChangeInfoMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HTCHANGEINFO");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
