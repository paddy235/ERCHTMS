using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// �� �������������䱸
    /// </summary>
    public class FireEquipMap : EntityTypeConfiguration<FireEquipEntity>
    {
        public FireEquipMap()
        {
            #region ������
            //��
            this.ToTable("HRS_FIREEQUIP");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
