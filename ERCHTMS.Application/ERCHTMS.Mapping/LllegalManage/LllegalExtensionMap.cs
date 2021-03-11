using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.LllegalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LllegalManage
{
    /// <summary>
    /// 描 述：整改延期对象
    /// </summary>
    public class LllegalExtensionMap : EntityTypeConfiguration<LllegalExtensionEntity>
    {
        public LllegalExtensionMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LLLEGALEXTENSION");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}