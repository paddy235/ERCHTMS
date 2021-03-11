using ERCHTMS.Entity.SafePunish;
using ERCHTMS.IService.SafePunish;
using ERCHTMS.Service.SafePunish;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.SafePunish
{
    /// <summary>
    /// �� ������ȫ������ϸ
    /// </summary>
    public class SafepunishdetailBLL
    {
        private SafepunishdetailIService service = new SafepunishdetailService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafepunishdetailEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SafepunishdetailEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
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
        public void SaveForm(string keyValue, SafepunishdetailEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// ��ȡ������ϸ�б�
        /// </summary>
        /// <param name="punishId">��ȫ����id</param>
        /// <param name="type">����</param>
        /// <returns></returns>
        public IEnumerable<SafepunishdetailEntity> GetListByPunishId(string punishId, string type)
        {
            return service.GetListByPunishId(punishId, type);
        }

        /// <summary>
        /// ���ݰ�ȫ����IDɾ������
        /// </summary>
        /// <param name="punishId">��ȫ����ID</param>
        /// <param name="type">����</param>
        public int Remove(string punishId, string type)
        {
            try
            {
                service.Remove(punishId, type);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion
    }
}
