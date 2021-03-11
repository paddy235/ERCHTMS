using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using ERCHTMS.Service.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.KbsDeviceManage
{
    /// <summary>
    /// �� ������ǩ��������־
    /// </summary>
    public class LableonlinelogBLL
    {
        private LableonlinelogIService service = new LableonlinelogService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<LableonlinelogEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LableonlinelogEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ���ݱ�ǩID��ȡ���߱�ǩ
        /// </summary>
        /// <param name="LableId"></param>
        /// <returns></returns>
        public LableonlinelogEntity GetOnlineEntity(string LableId)
        {
            return service.GetOnlineEntity(LableId);
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
        public void SaveForm(string keyValue, LableonlinelogEntity entity)
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

        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool SaveStatus(LableonlinelogEntity entity)
        {
           return service.SaveStatus(entity);
        }

        #endregion
    }
}
