using ERCHTMS.Entity.ComprehensiveManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.ComprehensiveManage
{
    /// <summary>
    /// �� ������Ϣ������ϸ��
    /// </summary>
    public class InfoSubmitDetailsEntityMap : EntityTypeConfiguration<InfoSubmitDetailsEntity>
    {
        public InfoSubmitDetailsEntityMap()
        {
            #region ������
            //��
            this.ToTable("HRS_INFOSUBMITDETAILS");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
