using ERCHTMS.Entity.PersonManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PersonManage
{
    /// <summary>
    /// �� ����ת����Ϣ��
    /// </summary>
    public class TransferMap : EntityTypeConfiguration<TransferEntity>
    {
        public TransferMap()
        {
            #region ������
            //��
            this.ToTable("BIS_TRANSFER");
            //����
            this.HasKey(t => t.TID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
