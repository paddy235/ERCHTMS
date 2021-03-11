using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.OutsourcingProject;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全保证金历史信息
    /// </summary>
    public class HistoryMoneyMap : EntityTypeConfiguration<HistoryMoneyEntity>
    {
        public HistoryMoneyMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_HISSAFETYEAMESTMONEY");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
