using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class HTBaseInfoMap : EntityTypeConfiguration<HTBaseInfoEntity>
    {
        public HTBaseInfoMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HTBASEINFO");
            //����
            this.HasKey(t => t.ID);
            //�����ֶ�
            //this.Ignore(t=>t.CHANGEMEASURE);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
