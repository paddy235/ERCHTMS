using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// �� �������̻
    /// </summary>
    public class ActivityMap : EntityTypeConfiguration<ActivityEntity>
    {
        public ActivityMap()
        {
            #region ��������
            //��
            this.ToTable("SYS_WFTBACTIVITY");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}