using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// �� ������׼�ƶ��ղ��ļ�
    /// </summary>
    public class StdsysFilesStoreEntityMap : EntityTypeConfiguration<StdsysFilesStoreEntity>
    {
        public StdsysFilesStoreEntityMap()
        {
            #region ������
            //��
            this.ToTable("HRS_STDSYSSTOREFILES");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
