using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// �� ��������糧�ύ����˵����
    /// </summary>
    public class OutcommitfileMap : EntityTypeConfiguration<OutcommitfileEntity>
    {
        public OutcommitfileMap()
        {
            #region ������
            //��
            this.ToTable("EPG_OUTCOMMITFILE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
