using ERCHTMS.Entity.NosaManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.NosaManage
{
    /// <summary>
    /// �� ����NOSA����������ϸ
    /// </summary>
    public class NosaworkitemMap : EntityTypeConfiguration<NosaworkitemEntity>
    {
        public NosaworkitemMap()
        {
            #region ������
            //��
            this.ToTable("HRS_NOSAWORKITEM");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
