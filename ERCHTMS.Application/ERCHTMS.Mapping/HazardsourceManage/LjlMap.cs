using ERCHTMS.Entity.HazardsourceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HazardsourceManage
{
    /// <summary>
    /// �� ����Σ�ջ�ѧƷֵ��Ϣ
    /// </summary>
    public class LjlMap : EntityTypeConfiguration<LjlEntity>
    {
        public LjlMap()
        {
            #region ������
            //��
            this.ToTable("HSD_LJL");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
