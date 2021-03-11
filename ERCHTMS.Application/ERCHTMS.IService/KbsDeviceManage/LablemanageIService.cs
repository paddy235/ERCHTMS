using ERCHTMS.Entity.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.KbsDeviceManage
{
    /// <summary>
    /// �� ������ǩ����
    /// </summary>
    public interface LablemanageIService
    {
        #region ��ȡ����

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        List<LablemanageEntity> GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ȡ��ǩ����
        /// </summary>
        /// <returns></returns>
        int GetCount();
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<LablemanageEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        LablemanageEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ��ǩͳ��ͼ
        /// </summary>
        /// <returns></returns>
        string GetLableChart();

        /// <summary>
        /// ��ȡ��ǩͳ����Ϣ
        /// </summary>
        /// <returns></returns>
        DataTable GetLableStatistics();

        /// <summary>
        /// ��ȡ�û��󶨱�ǩ
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        LablemanageEntity GetUserLable(string userid);

        /// <summary>
        /// ��ȡ�����Ƿ�󶨱�ǩ
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        LablemanageEntity GetCarLable(string CarNo);

        /// <summary>
        /// ��ȡ��ǩ�Ƿ��ظ���
        /// </summary>
        /// <param name="LableId"></param>
        /// <returns></returns>
        bool GetIsBind(string LableId);

        /// <summary>
        /// ����lableId��ȡ�Ƿ��а���Ϣ
        /// </summary>
        /// <param name="LableId"></param>
        /// <returns></returns>
        LablemanageEntity GetLable(string LableId);
        #endregion

        #region �ύ����

        /// <summary>
        /// ����ǩ
        /// </summary>
        /// <param name="keyValue"></param>
        void Untie(string keyValue);
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
        void SaveForm(string keyValue, LablemanageEntity entity);
        #endregion
    }
}
