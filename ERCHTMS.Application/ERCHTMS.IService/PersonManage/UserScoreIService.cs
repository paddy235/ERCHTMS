using ERCHTMS.Entity.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.PersonManage
{
    /// <summary>
    /// �� ������Ա����
    /// </summary>
    public interface UserScoreIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        IEnumerable<UserScoreEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        DataTable GetList(string userId);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        UserScoreEntity GetEntity(string keyValue);
         /// <summary>
        /// �洢���̷�ҳ��ѯ
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetPageJsonList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ��Ա���ֿ�����ϸ
        /// </summary>
        /// <param name="keyValue">��¼Id</param>
        /// <returns></returns>
        object GetInfo(string keyValue);
         /// <summary>
        /// ��ȡ�û�ָ����ݵĻ���
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <param name="year">���</param>
        /// <returns></returns>
        decimal GetUserScore(string userId, string year);
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
        void SaveForm(string keyValue, UserScoreEntity entity);
        /// <summary>
        /// ����������ֿ��˼�¼
        /// </summary>
        /// <param name="list"></param>
        void Save(List<UserScoreEntity> list);
         /// <summary>
        /// ��ȡ��Ա����׻��ֺ��ۼƻ���
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <returns></returns>
        string GetScoreInfo(string userId);
        #endregion
    }
}
