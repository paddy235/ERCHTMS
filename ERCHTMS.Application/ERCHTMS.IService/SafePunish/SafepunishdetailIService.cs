using ERCHTMS.Entity.SafePunish;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.SafePunish
{
    /// <summary>
    /// �� ������ȫ������ϸ
    /// </summary>
    public interface SafepunishdetailIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SafepunishdetailEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SafepunishdetailEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, SafepunishdetailEntity entity);

        /// <summary>
        /// ��ȡ������ϸ�б�
        /// </summary>
        /// <param name="punishId">��ȫ����id</param>
        /// <param name="type">���� 0:�����˶��� 1:�������˶���</param>
        /// <returns></returns>
        IEnumerable<SafepunishdetailEntity> GetListByPunishId(string punishId, string type);

        /// <summary>
        /// ���ݰ�ȫ����IDɾ������
        /// </summary>
        /// <param name="punishId">��ȫ����ID</param>
        /// <param name="type">����</param>
        int Remove(string punishId, string type);
        #endregion
    }
}
