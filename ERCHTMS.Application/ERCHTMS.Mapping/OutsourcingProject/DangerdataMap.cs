using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ����Σ�յ����ݿ�
    /// </summary>
    public class DangerdataMap : EntityTypeConfiguration<DangerdataEntity>
    {
        public DangerdataMap()
        {
            #region ������
            //��
            this.ToTable("EPG_DANGERDATA");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
