using ERCHTMS.Entity.OccupationalHealthManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OccupationalHealthManage
{
    /// <summary>
    /// �� ����ְҵ�������
    /// </summary>
    public class InspectionMap : EntityTypeConfiguration<InspectionEntity>
    {
        public InspectionMap()
        {
            #region ������
            //��
            this.ToTable("BIS_INSPECTION");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
