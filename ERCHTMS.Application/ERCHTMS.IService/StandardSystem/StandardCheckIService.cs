using ERCHTMS.Entity.StandardSystem;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.StandardSystem
{
    /// <summary>
    /// �� ������׼�ޱ������
    /// </summary>
    public interface StandardCheckIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<StandardCheckEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        StandardCheckEntity GetEntity(string keyValue);
        /// <summary>
        /// ��ǩ�Ƿ����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="checkUserId"></param>
        /// <returns></returns>
        bool FinishSign(string keyValue, string checkUserId);
        /// <summary>
        /// ��ί������Ƿ����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="checkUserId"></param>
        /// <returns></returns>
        bool FinishCommittee(string keyValue, string checkUserId);
        /// <summary>
        /// �Ƿ�ȫ��������
        /// </summary>
        /// <returns></returns>
        bool FinishComplete(string checkUserId, string checkUserName, string checkType);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        StandardCheckEntity GetLastEntityByRecId(string keyValue,string checkType);
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
        void SaveForm(string keyValue, StandardCheckEntity entity);
        #endregion
    }
}
