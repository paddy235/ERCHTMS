using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� ��������ͷ����
    /// </summary>
    public class CameramanageMap : EntityTypeConfiguration<CameramanageEntity>
    {
        public CameramanageMap()
        {
            #region ������
            //��
            this.ToTable("BIS_CAMERAMANAGE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
