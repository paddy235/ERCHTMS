using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ��������������ñ�
    /// </summary>
    public class OutprocessconfigMap : EntityTypeConfiguration<OutprocessconfigEntity>
    {
        public OutprocessconfigMap()
        {
            #region ������
            //��
            this.ToTable("EPG_OUTPROCESSCONFIG");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
