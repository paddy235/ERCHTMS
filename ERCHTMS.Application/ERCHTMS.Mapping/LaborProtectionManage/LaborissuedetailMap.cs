using ERCHTMS.Entity.LaborProtectionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ��������ű�����
    /// </summary>
    public class LaborissuedetailMap : EntityTypeConfiguration<LaborissuedetailEntity>
    {
        public LaborissuedetailMap()
        {
            #region ������
            //��
            this.ToTable("BIS_LABORISSUEDETAIL");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
