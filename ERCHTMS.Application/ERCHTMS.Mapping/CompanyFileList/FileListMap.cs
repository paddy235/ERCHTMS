using ERCHTMS.Entity.FileListManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FileListManage
{
    /// <summary>
    /// �� ������ϵ�����ļ��嵥
    /// </summary>
    public class FileListMap : EntityTypeConfiguration<FileListEntity>
    {
        public FileListMap()
        {
            #region ������
            //��
            this.ToTable("HRS_FILELIST");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
