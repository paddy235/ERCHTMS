using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// �� �����ճ�Ѳ�飨���ݣ�
    /// </summary>
    public class EverydayPatrolDetailMap : EntityTypeConfiguration<EverydayPatrolDetailEntity>
    {
        public EverydayPatrolDetailMap()
        {
            #region ������
            //��
            this.ToTable("HRS_EVERYDAYPATROLDETAIL");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
