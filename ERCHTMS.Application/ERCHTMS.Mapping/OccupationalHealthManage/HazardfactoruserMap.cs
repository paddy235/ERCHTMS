using ERCHTMS.Entity.OccupationalHealthManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OccupationalHealthManage
{
    /// <summary>
    /// �� ����Σ��������Ա��
    /// </summary>
    public class HazardfactoruserMap : EntityTypeConfiguration<HazardfactoruserEntity>
    {
        public HazardfactoruserMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HAZARDFACTORUSER");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
