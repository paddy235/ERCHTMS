using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� ��������Σ��Ʒ�����Ŀ��
    /// </summary>
    public class CarcheckitemMap : EntityTypeConfiguration<CarcheckitemEntity>
    {
        public CarcheckitemMap()
        {
            #region ������
            //��
            this.ToTable("BIS_CARCHECKITEM");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
