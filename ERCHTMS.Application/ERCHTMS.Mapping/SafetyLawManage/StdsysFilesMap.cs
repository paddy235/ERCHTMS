using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// �� ������׼�ƶ��ļ�
    /// </summary>
    public class StdsysFilesEntityMap : EntityTypeConfiguration<StdsysFilesEntity>
    {
        public StdsysFilesEntityMap()
        {
            #region ������
            //��
            this.ToTable("HRS_STDSYSFILES");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
