using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������������Ӧ�ù�����
    /// </summary>
    public class HTRelevanceMap : EntityTypeConfiguration<HTRelevanceEntity>
    {
        public HTRelevanceMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HTRELEVANCE");
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
