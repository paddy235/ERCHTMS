using ERCHTMS.Entity.EquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EquipmentManage
{
    /// <summary>
    /// �� �������й��ϼ�¼��
    /// </summary>
    public class OperationFailureMap : EntityTypeConfiguration<OperationFailureEntity>
    {
        public OperationFailureMap()
        {
            #region ������
            //��
            this.ToTable("BIS_OPERATIONFAILURE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
