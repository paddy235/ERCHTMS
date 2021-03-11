using ERCHTMS.Entity.SafeReward;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.SafeReward
{
    /// <summary>
    /// �� ������ȫ������ϸ
    /// </summary>
    public interface SaferewarddetailIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SaferewarddetailEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SaferewarddetailEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ������ϸ�б�
        /// </summary>
        /// <param name="rewardId">��ȫ����id</param>
        /// <returns></returns>
        IEnumerable<SaferewarddetailEntity> GetListByRewardId(string rewardId);

        /// <summary>
        /// ���ݰ�ȫ����IDɾ������
        /// </summary>
        /// <param name="rewardId">��ȫ����ID</param>
        int Remove(string rewardId);
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
        void SaveForm(string keyValue, SaferewarddetailEntity entity);
        #endregion
    }
}
