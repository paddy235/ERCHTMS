using ERCHTMS.Entity.HazardsourceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HazardsourceManage
{
    /// <summary>
    /// �� ������ѧƷ��ȫ����˵����
    /// </summary>
    public class AqsmsMap : EntityTypeConfiguration<AqsmsEntity>
    {
        public AqsmsMap()
        {
            #region ������
            //��
            this.ToTable("HSD_AQSMS");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
