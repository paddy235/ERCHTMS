using ERCHTMS.Entity.NosaManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.NosaManage
{
    public class NosaPersonWorkSummaryMap : EntityTypeConfiguration<NosaPersonWorkSummaryEntity>
    {

        public NosaPersonWorkSummaryMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_NOSAWORKMANAGER");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
