using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// �� ��������ʵ��
    /// </summary>
    public class WfTBProcessMap : EntityTypeConfiguration<WfTBProcessEntity>
    {
        public WfTBProcessMap()
        {
            #region ������
            //��
            this.ToTable("SYS_WFTBPROCESS");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
