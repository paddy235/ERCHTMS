using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// �� ����������Աץ�ļ�¼��
    /// </summary>
    public class WorkcameracaptureMap : EntityTypeConfiguration<WorkcameracaptureEntity>
    {
        public WorkcameracaptureMap()
        {
            #region ������
            //��
            this.ToTable("BIS_WORKCAMERACAPTURE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
