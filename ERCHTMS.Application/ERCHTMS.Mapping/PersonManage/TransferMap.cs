using ERCHTMS.Entity.PersonManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PersonManage
{
    /// <summary>
    /// 描 述：转岗信息表
    /// </summary>
    public class TransferMap : EntityTypeConfiguration<TransferEntity>
    {
        public TransferMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_TRANSFER");
            //主键
            this.HasKey(t => t.TID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
