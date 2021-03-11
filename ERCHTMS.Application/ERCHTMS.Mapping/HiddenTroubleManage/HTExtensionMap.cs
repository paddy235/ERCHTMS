using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：整改延期对象
    /// </summary>
    public class HTExtensionMap : EntityTypeConfiguration<HTExtensionEntity>
    {
        public HTExtensionMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HTEXTENSION");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}