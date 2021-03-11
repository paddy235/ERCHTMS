using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：危化品车辆检查项目表
    /// </summary>
    public class CarcheckitemdetailMap : EntityTypeConfiguration<CarcheckitemdetailEntity>
    {
        public CarcheckitemdetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_CARCHECKITEMDETAIL");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
