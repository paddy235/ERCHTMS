using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public class FireTroopsMap : EntityTypeConfiguration<FireTroopsEntity>
    {
        public FireTroopsMap()
        {
            #region ������
            //��
            this.ToTable("HRS_FIRETROOPS");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
