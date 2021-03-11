using ERCHTMS.Entity.EquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EquipmentManage
{
    /// <summary>
    /// 描 述：运行故障记录表
    /// </summary>
    public class OperationFailureMap : EntityTypeConfiguration<OperationFailureEntity>
    {
        public OperationFailureMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_OPERATIONFAILURE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
