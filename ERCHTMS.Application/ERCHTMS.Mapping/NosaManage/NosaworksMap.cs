using ERCHTMS.Entity.NosaManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.NosaManage
{
    /// <summary>
    /// 描 述：工作任务
    /// </summary>
    public class NosaworksMap : EntityTypeConfiguration<NosaworksEntity>
    {
        public NosaworksMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_NOSAWORKS");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
