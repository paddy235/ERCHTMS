using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ����ģ�����
    /// </summary>
    public class TempmanagerMap : EntityTypeConfiguration<TempmanagerEntity>
    {
        public TempmanagerMap()
        {
            #region ������
            //��
            this.ToTable("EPG_TEMPMANAGER");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
