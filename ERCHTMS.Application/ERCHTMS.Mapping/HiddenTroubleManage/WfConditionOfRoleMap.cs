using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// �� ������������������ɫ���ӱ�
    /// </summary>
    public class WfConditionOfRoleMap : EntityTypeConfiguration<WfConditionOfRoleEntity>
    {
        public WfConditionOfRoleMap()
        {
            #region ������
            //��
            this.ToTable("BIS_WFCONDITIONOFROLE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
