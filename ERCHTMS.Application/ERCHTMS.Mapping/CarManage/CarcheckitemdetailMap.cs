using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� ����Σ��Ʒ���������Ŀ��
    /// </summary>
    public class CarcheckitemdetailMap : EntityTypeConfiguration<CarcheckitemdetailEntity>
    {
        public CarcheckitemdetailMap()
        {
            #region ������
            //��
            this.ToTable("BIS_CARCHECKITEMDETAIL");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
