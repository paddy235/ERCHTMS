using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// 描 述：安全管理制度
    /// </summary>
    public class SafeInstitutionMap : EntityTypeConfiguration<SafeInstitutionEntity>
    {
        public SafeInstitutionMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFEINSTITUTION");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
