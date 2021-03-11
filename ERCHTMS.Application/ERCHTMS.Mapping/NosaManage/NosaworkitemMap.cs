using ERCHTMS.Entity.NosaManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.NosaManage
{
    /// <summary>
    /// 描 述：NOSA工作任务明细
    /// </summary>
    public class NosaworkitemMap : EntityTypeConfiguration<NosaworkitemEntity>
    {
        public NosaworkitemMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_NOSAWORKITEM");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
