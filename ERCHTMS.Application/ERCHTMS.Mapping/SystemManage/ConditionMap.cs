using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// �� ����������ת
    /// </summary>
    public class ConditionMap : EntityTypeConfiguration<ConditionEntity>
    {
        public ConditionMap()
        {
            #region ��������
            //��
            this.ToTable("SYS_WFTBCONDITION");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}