using ERCHTMS.Entity.PersonManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PersonManage
{
    /// <summary>
    /// �� �����û��Ž�������
    /// </summary>
    public class AcesscontrolinfoMap : EntityTypeConfiguration<AcesscontrolinfoEntity>
    {
        public AcesscontrolinfoMap()
        {
            #region ������
            //��
            this.ToTable("BIS_ACESSCONTROLINFO");
            //����
            this.HasKey(t => t.TID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
