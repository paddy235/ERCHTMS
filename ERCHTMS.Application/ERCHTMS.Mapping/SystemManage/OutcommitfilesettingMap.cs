using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// �� �����������˵���û����ñ�
    /// </summary>
    public class OutcommitfilesettingMap : EntityTypeConfiguration<OutcommitfilesettingEntity>
    {
        public OutcommitfilesettingMap()
        {
            #region ������
            //��
            this.ToTable("EPG_OUTCOMMITFILESETTING");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
