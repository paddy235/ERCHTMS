using ERCHTMS.Entity.EngineeringManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EngineeringManage
{
    /// <summary>
    /// �� ����Σ�󹤳���Ŀ����
    /// </summary>
    public class EngineeringSettingMap : EntityTypeConfiguration<EngineeringSettingEntity>
    {
        public EngineeringSettingMap()
        {
            #region ������
            //��
            this.ToTable("BIS_ENGINEERINGSETTING");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
