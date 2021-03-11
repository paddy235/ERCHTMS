using ERCHTMS.Entity.FireManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Collections;

namespace ERCHTMS.IService.FireManage
{
    /// <summary>
    /// �� ����������ʩ
    /// </summary>
    public interface FirefightingIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<FirefightingEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        FirefightingEntity GetEntity(string keyValue);

        DataTable StatisticsData(string queryJson);
        #endregion

        #region �ύ����
        /// <summary>
        /// ����id��������ɾ��
        /// </summary>
        /// <param name="Ids"></param>
        void Remove(string Ids);
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
        void SaveForm(string keyValue, FirefightingEntity entity);
        #endregion

        /// <summary>
        /// ͬһ���ͣ���Ų����ظ�
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Code"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        bool ExistCode(string Type, string Code, string keyValue);
        IList GetCountByArea(List<string> areaCodes);
    }
}
