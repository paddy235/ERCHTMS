using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� �����糧ģ�����
    /// </summary>
    public class TempConfigMap : EntityTypeConfiguration<TempConfigEntity>
    {
        public TempConfigMap()
        {
            #region ������
            //��
            this.ToTable("EPG_TEMPMANAGE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
