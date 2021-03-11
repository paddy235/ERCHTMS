using ERCHTMS.Entity.NosaManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.NosaManage
{
    /// <summary>
    /// 描 述：培训文件
    /// </summary>
    public class NosatrafilesMap : EntityTypeConfiguration<NosatrafilesEntity>
    {
        public NosatrafilesMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_NOSATRAFILES");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
