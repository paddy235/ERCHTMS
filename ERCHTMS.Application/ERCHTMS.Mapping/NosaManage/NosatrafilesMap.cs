using ERCHTMS.Entity.NosaManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.NosaManage
{
    /// <summary>
    /// �� ������ѵ�ļ�
    /// </summary>
    public class NosatrafilesMap : EntityTypeConfiguration<NosatrafilesEntity>
    {
        public NosatrafilesMap()
        {
            #region ������
            //��
            this.ToTable("HRS_NOSATRAFILES");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
