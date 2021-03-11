using ERCHTMS.Entity.EmergencyPlatform;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ��������¼�����
    /// </summary>
    public interface DrillplanrecordstepIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<DrillplanrecordstepEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        DrillplanrecordstepEntity GetEntity(string keyValue);

        /// <summary>
        /// Ӧ����¼�����б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// ����recid��ȡ�����б�
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        IList<DrillplanrecordstepEntity> GetListByRecid(string recid);
        #endregion

        #region �ύ����

        /// <summary>
        /// ���ݹ���IDɾ������
        /// </summary>
        /// <param name="recid"></param>
        void RemoveFormByRecid(string recid);

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
        void SaveForm(string keyValue, DrillplanrecordstepEntity entity);
        #endregion
    }
}
