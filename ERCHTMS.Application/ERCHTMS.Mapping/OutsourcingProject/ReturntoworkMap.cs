using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� �������������
    /// </summary>
    public class ReturntoworkMap : EntityTypeConfiguration<ReturntoworkEntity>
    {
        public ReturntoworkMap()
        {
            #region ������
            //��
            this.ToTable("EPG_RETURNTOWORK");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
