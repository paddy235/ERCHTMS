using ERCHTMS.Entity.DangerousJob;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.DangerousJob
{
    /// <summary>
    /// �� ����ä����ҵä����
    /// </summary>
    public interface BlindPlateWallSpecIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<BlindPlateWallSpecEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        BlindPlateWallSpecEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, BlindPlateWallSpecEntity entity);
        #endregion
    }
}
