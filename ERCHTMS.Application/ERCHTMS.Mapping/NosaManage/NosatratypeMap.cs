using ERCHTMS.Entity.NosaManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.NosaManage
{
    /// <summary>
    /// 描 述：培训文件分类表
    /// </summary>
    public class NosatratypeMap : EntityTypeConfiguration<NosatratypeEntity>
    {
        public NosatratypeMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_NOSATRATYPE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
