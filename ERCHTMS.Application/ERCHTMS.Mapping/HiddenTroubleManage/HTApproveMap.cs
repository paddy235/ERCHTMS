using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class HTApproveMap : EntityTypeConfiguration<HTApprovalEntity>
    {
        public HTApproveMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HTAPPROVAL");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
