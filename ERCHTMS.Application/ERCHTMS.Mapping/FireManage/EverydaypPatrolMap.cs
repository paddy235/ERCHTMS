using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// �� �����ճ�Ѳ��
    /// </summary>
    public class EverydaypPatrolMap : EntityTypeConfiguration<EverydayPatrolEntity>
    {
        public EverydaypPatrolMap()
        {
            #region ������
            //��
            this.ToTable("HRS_EVERYDAYPATROL");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
