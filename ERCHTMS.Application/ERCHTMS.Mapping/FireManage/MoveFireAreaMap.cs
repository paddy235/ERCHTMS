using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// �� ������������ά��
    /// </summary>
    public class MoveFireAreaMap : EntityTypeConfiguration<MoveFireAreaEntity>
    {
        public MoveFireAreaMap()
        {
            #region ������
            //��
            this.ToTable("HRS_MOVEFIREAREA");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
