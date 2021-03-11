using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.OutsourcingProject;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    public class HistorySpecificToolsMap: EntityTypeConfiguration<HistorySpecificToolsEntity>
    {
        public HistorySpecificToolsMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_HISTORYSPECIFICTOOLS");
            //主键
            this.HasKey(t => t.SPECIFICTOOLSID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
