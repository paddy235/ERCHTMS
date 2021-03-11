using ERCHTMS.Entity.CarManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 拜访人员映射表
    /// </summary>
    public class CarUserMap : EntityTypeConfiguration<CarUserEntity>
    {
        public CarUserMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_USERCAR");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
