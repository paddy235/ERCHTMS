using ERCHTMS.Entity.LaborProtectionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ��������ձ��ϱ�����
    /// </summary>
    public class LaborrecyclingMap : EntityTypeConfiguration<LaborrecyclingEntity>
    {
        public LaborrecyclingMap()
        {
            #region ������
            //��
            this.ToTable("BIS_LABORRECYCLING");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
