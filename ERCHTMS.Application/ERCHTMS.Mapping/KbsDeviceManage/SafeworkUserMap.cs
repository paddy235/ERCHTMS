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
    /// 描述：作业管理监管成员子表映射
    /// </summary>
    public class SafeworkUserMap : EntityTypeConfiguration<SafeworkUserEntity>
    {
        public SafeworkUserMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFEWORKUSER");
            //主键
            this.HasKey(t => t.ID);
            #endregion 
        }
    }


}
