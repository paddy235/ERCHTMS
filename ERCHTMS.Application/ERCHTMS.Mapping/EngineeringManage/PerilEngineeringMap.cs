using ERCHTMS.Entity.EngineeringManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EngineeringManage
{
    /// <summary>
    /// �� ����Σ�󹤳̹���
    /// </summary>
    public class PerilEngineeringMap : EntityTypeConfiguration<PerilEngineeringEntity>
    {
        public PerilEngineeringMap()
        {
            #region ������
            //��
            this.ToTable("BIS_PERILENGINEERING");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
