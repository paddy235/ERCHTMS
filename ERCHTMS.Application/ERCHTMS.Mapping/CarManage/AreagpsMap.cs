using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� ��������λ����
    /// </summary>
    public class AreagpsMap : EntityTypeConfiguration<AreagpsEntity>
    {
        public AreagpsMap()
        {
            #region ������
            //��
            this.ToTable("BIS_AREAGPS");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
