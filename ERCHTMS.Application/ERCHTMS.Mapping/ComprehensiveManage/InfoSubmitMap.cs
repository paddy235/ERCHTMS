using ERCHTMS.Entity.ComprehensiveManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.ComprehensiveManage
{
    /// <summary>
    /// �� ������Ϣ���ͱ�
    /// </summary>
    public class InfoSubmitEntityMap : EntityTypeConfiguration<InfoSubmitEntity>
    {
        public InfoSubmitEntityMap()
        {
            #region ������
            //��
            this.ToTable("HRS_INFOSUBMIT");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
