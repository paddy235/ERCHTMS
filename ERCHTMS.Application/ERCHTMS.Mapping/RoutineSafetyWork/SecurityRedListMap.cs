using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    /// <summary>
    /// �� ������ȫ��ڰ�
    /// </summary>
    public class SecurityRedListMap : EntityTypeConfiguration<SecurityRedListEntity>
    {
        public SecurityRedListMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SECURITYREDLIST");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
