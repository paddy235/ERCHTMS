using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� ��������Σ��Ʒ�����Ŀ�����Σ��Ʒ
    /// </summary>
    public class CarcheckitemhazardousMap : EntityTypeConfiguration<CarcheckitemhazardousEntity>
    {
        public CarcheckitemhazardousMap()
        {
            #region ������
            //��
            this.ToTable("BIS_CARCHECKITEMHAZARDOUS");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
