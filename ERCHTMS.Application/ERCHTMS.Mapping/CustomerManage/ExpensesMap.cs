using ERCHTMS.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CustomerManage
{
    /// <summary>
    /// �� ��������֧��
    /// </summary>
    public class ExpensesMap : EntityTypeConfiguration<ExpensesEntity>
    {
        public ExpensesMap()
        {
            #region ������
            //��
            this.ToTable("CLIENT_EXPENSES");
            //����
            this.HasKey(t => t.ExpensesId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
