using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    /// <summary>
    /// �� �����ļ�����
    /// </summary>
    public class FileManageMap : EntityTypeConfiguration<FileManageEntity>
    {
        public FileManageMap()
        {
            #region ������
            //��
            this.ToTable("BIS_FILEMANAGE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
