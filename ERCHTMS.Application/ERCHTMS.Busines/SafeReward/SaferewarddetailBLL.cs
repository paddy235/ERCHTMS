using ERCHTMS.Entity.SafeReward;
using ERCHTMS.IService.SafeReward;
using ERCHTMS.Service.SafeReward;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.SafeReward
{
    /// <summary>
    /// �� ������ȫ������ϸ
    /// </summary>
    public class SaferewarddetailBLL
    {
        private SaferewarddetailIService service = new SaferewarddetailService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SaferewarddetailEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SaferewarddetailEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ������ϸ�б�
        /// </summary>
        /// <param name="rewardId">��ȫ����id</param>
        /// <returns></returns>
        public IEnumerable<SaferewarddetailEntity> GetListByRewardId(string rewardId)
        {
            return service.GetListByRewardId(rewardId);
        }

        /// <summary>
        /// ���ݰ�ȫ����idɾ������
        /// </summary>
        /// <param name="rewardId">��ȫ����id</param>
        public int Remove(string rewardId)
        {
            try
            {
                service.Remove(rewardId);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SaferewarddetailEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
