using ERCHTMS.Entity.MatterManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.MatterManage
{
    /// <summary>
    /// 工作日志映射
    /// </summary>
    public class DailyrRecordMap : EntityTypeConfiguration<DailyrRecordEntity>
    {
        public DailyrRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("WL_DAILYRRECORD");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }

    }
}
