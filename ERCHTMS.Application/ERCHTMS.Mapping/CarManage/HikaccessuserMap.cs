using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� �����Ž�ʹ���û���
    /// </summary>
    public class HikaccessuserMap : EntityTypeConfiguration<HikaccessuserEntity>
    {
        public HikaccessuserMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HIKACCESSUSER");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
