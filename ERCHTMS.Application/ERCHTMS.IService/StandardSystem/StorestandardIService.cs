using ERCHTMS.Entity.StandardSystem;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.StandardSystem
{
    /// <summary>
    /// �� �����ղر�׼
    /// </summary>
    public interface StorestandardIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<StorestandardEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        StorestandardEntity GetEntity(string keyValue);

        /// <summary>
        /// ���ݱ�׼ID�ж��Ƿ����ղ�
        /// </summary>
        /// <param name="standardID"></param>
        /// <returns></returns>
        int GetStoreByStandardID(string standardID);
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
        void SaveForm(string keyValue, StorestandardEntity entity);
        #endregion
    }
}
