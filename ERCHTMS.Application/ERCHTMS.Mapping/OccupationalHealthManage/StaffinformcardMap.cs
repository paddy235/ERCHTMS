using ERCHTMS.Entity.OccupationalHealthManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OccupationalHealthManage
{
    /// <summary>
    /// �� ����ְҵ��Σ����֪��
    /// </summary>
    public class StaffinformcardMap : EntityTypeConfiguration<StaffinformcardEntity>
    {
        public StaffinformcardMap()
        {
            #region ������
            //��
            this.ToTable("BIS_STAFFINFORMCARD");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
