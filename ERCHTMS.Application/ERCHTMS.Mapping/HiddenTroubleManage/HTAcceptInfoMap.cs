using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class HTAcceptInfoMap : EntityTypeConfiguration<HTAcceptInfoEntity>
    {
        public HTAcceptInfoMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HTACCEPTINFO");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
