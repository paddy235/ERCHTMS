using ERCHTMS.Entity.SafetyWorkSupervise;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyWorkSupervise
{
    /// <summary>
    /// �� ������ȫ�ص㹤�����췴����Ϣ
    /// </summary>
    public class SuperviseconfirmationMap : EntityTypeConfiguration<SuperviseconfirmationEntity>
    {
        public SuperviseconfirmationMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SUPERVISECONFIRMATION");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
