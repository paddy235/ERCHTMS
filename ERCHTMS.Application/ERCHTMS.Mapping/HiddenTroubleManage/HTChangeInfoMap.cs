using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class HTChangeInfoMap : EntityTypeConfiguration<HTChangeInfoEntity>
    {
        public HTChangeInfoMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HTCHANGEINFO");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
