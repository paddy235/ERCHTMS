using ERCHTMS.Entity.EvaluateManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EvaluateManage
{
    /// <summary>
    /// �� �����ļ�ʶ��
    /// </summary>
    public class FileSpotMap : EntityTypeConfiguration<FileSpotEntity>
    {
        public FileSpotMap()
        {
            #region ������
            //��
            this.ToTable("HRS_FILESPOT");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
