using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.SaftProductTargetManage;

namespace ERCHTMS.Mapping.SaftProductTargetManage
{
    /// <summary>
    /// 描 述：安全生产目标
    /// </summary>
    public class SafeProductMap : EntityTypeConfiguration<SafeProductEntity>
    {
        public SafeProductMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFEPRODUCT");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
