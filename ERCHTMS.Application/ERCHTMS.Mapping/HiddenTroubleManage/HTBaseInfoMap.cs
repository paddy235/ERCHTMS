using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患基本信息表
    /// </summary>
    public class HTBaseInfoMap : EntityTypeConfiguration<HTBaseInfoEntity>
    {
        public HTBaseInfoMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HTBASEINFO");
            //主键
            this.HasKey(t => t.ID);
            //忽略字段
            //this.Ignore(t=>t.CHANGEMEASURE);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
