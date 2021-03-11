using ERCHTMS.Entity.OccupationalHealthManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：危害因素人员表
    /// </summary>
    public class HazardfactoruserMap : EntityTypeConfiguration<HazardfactoruserEntity>
    {
        public HazardfactoruserMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HAZARDFACTORUSER");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
