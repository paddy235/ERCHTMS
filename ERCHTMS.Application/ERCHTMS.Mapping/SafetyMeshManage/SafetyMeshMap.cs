using ERCHTMS.Entity.SafetyMeshManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyMeshManage
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public class SafetyMeshMap : EntityTypeConfiguration<SafetyMeshEntity>
    {
        public SafetyMeshMap()
        {
            #region ������
            //��
            this.ToTable("HD_SAFETYMESH");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
