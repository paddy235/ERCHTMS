using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� �����ճ�Ѳ����Ŀ����
    /// </summary>
    public class EverydayProjectSetMap : EntityTypeConfiguration<EverydayProjectSetEntity>
    {
        public EverydayProjectSetMap()
        {
            #region ������
            //��
            this.ToTable("HRS_EVERYDAYPROJECTSET");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
