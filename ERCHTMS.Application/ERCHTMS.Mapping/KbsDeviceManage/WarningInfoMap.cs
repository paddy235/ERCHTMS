using ERCHTMS.Entity.KbsDeviceManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// 描述：预警信息映射
    /// </summary>
    public class WarningInfoMap: EntityTypeConfiguration<WarningInfoEntity>
    {
         public WarningInfoMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_EARLYWARNING");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }

    }
}
