using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using ERCHTMS.Service.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class HTAcceptInfoBLL
    {
        private HTAcceptInfoIService service = new HTAcceptInfoService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HTAcceptInfoEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HTAcceptInfoEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public HTAcceptInfoEntity GetEntityByHidCode(string hidCode)
        {
            return service.GetEntityByHidCode(hidCode);
        }

        public IEnumerable<HTAcceptInfoEntity> GetHistoryList(string hidCode)
        {
            return service.GetHistoryList(hidCode);
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
        public void SaveForm(string keyValue, HTAcceptInfoEntity entity)
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

        public void RemoveFormByCode(string hidcode)
        {
            try
            {
                service.RemoveFormByCode(hidcode);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
