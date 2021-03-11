using ERCHTMS.Entity.NosaManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.NosaManage
{
    /// <summary>
    /// 描 述：元素表
    /// </summary>
    public class NosaeleMap : EntityTypeConfiguration<NosaeleEntity>
    {
        public NosaeleMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_NOSAELE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
