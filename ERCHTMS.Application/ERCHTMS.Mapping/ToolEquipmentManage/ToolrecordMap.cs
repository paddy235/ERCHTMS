using ERCHTMS.Entity.ToolEquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.ToolEquipmentManage
{
    /// <summary>
    /// �� �����綯�����������¼
    /// </summary>
    public class ToolrecordMap : EntityTypeConfiguration<ToolrecordEntity>
    {
        public ToolrecordMap()
        {
            #region ������
            //��
            this.ToTable("BIS_TOOLRECORD");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
