using ERCHTMS.Entity.LaborProtectionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ��������ż�¼��
    /// </summary>
    public class LaborissueMap : EntityTypeConfiguration<LaborissueEntity>
    {
        public LaborissueMap()
        {
            #region ������
            //��
            this.ToTable("BIS_LABORISSUE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
