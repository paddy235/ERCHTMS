using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// �� �����ղط��ɷ���
    /// </summary>
    public class StoreLawMap : EntityTypeConfiguration<StoreLawEntity>
    {
        public StoreLawMap()
        {
            #region ������
            //��
            this.ToTable("BIS_STORELAW");
            //����
            this.HasKey(t => t.storeId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
