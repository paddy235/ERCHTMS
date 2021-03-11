using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ERCHTMS.Entity.CarManage;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描述：拜访人员表附件表映射
    /// </summary>
    public class CarUserFileImgMap : EntityTypeConfiguration<CarUserFileImgEntity>
    {
        public CarUserFileImgMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_USERCARFILEIMG");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
