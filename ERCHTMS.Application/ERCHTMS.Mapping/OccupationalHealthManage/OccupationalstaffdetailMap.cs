using ERCHTMS.Entity.OccupationalHealthManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病人详情表
    /// </summary>
    public class OccupationalstaffdetailMap : EntityTypeConfiguration<OccupationalstaffdetailEntity>
    {
        public OccupationalstaffdetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_OCCUPATIONALSTAFFDETAIL");
            //主键
            this.HasKey(t => t.OccDetailId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
