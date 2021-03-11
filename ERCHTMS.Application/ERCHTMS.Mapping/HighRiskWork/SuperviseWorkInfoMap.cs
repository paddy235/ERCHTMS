using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ������վ�ල��ҵ��Ϣ
    /// </summary>
    public class SuperviseWorkInfoMap : EntityTypeConfiguration<SuperviseWorkInfoEntity>
    {
        public SuperviseWorkInfoMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SUPERVISEWORKINFO");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
