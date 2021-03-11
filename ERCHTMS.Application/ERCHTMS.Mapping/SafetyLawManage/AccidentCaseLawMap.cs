using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// 描 述：事故案例库
    /// </summary>
    public class AccidentCaseLawMap : EntityTypeConfiguration<AccidentCaseLawEntity>
    {
        public AccidentCaseLawMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_ACCIDENTCASELAW");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
