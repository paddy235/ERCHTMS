using ERCHTMS.Entity.MatterManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.MatterManage
{
    /// <summary>
    /// �� ������Ʊ�����볧��Ʊ
    /// </summary>
    public class OperticketmanagerMap : EntityTypeConfiguration<OperticketmanagerEntity>
    {
        public OperticketmanagerMap()
        {
            #region ������
            //��
            this.ToTable("WL_OPERTICKETMANAGER");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
