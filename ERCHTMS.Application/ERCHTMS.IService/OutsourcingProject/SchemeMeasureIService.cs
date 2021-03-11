using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// �� ����������ʩ����
    /// </summary>
    public interface SchemeMeasureIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        DataTable GetList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SchemeMeasureEntity GetEntity(string keyValue);
        IEnumerable<SchemeMeasureEntity> GetList();

                /// <summary>
        /// ��ȡ������������������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetObjectByKeyValue(string keyValue);

        /// <summary>
        /// ��ȡ���һ�����ͨ����������������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        SchemeMeasureEntity GetSchemeMeasureListByOutengineerId(string keyValue);

        /// <summary>
        /// ��ȡ��ʷ������������������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetHistoryObjectByKeyValue(string keyValue); 
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, SchemeMeasureEntity entity);
        #endregion
    }
}
