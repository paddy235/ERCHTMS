using ERCHTMS.Entity.NosaManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.NosaManage
{
    /// <summary>
    /// 描 述：Nosa区域表
    /// </summary>
    public class NosaareaMap : EntityTypeConfiguration<NosaareaEntity>
    {
        public NosaareaMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_NOSAAREA");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
