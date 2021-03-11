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
    public class HTChangeInfoBLL
    {
        private HTChangeInfoIService service = new HTChangeInfoService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HTChangeInfoEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HTChangeInfoEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public HTChangeInfoEntity GetEntityByHidCode(string hidCode)
        {
            try
            {
                return service.GetEntityByHidCode(hidCode);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<HTChangeInfoEntity> GetHistoryList(string hidCode)
        {
            return service.GetHistoryList(hidCode);
        }

        /// <summary>
        /// ���� Code��ȡ����ʵ��
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public HTChangeInfoEntity GetEntityByCode(string keyValue)
        {
            return service.GetEntityByCode(keyValue);
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
        public void SaveForm(string keyValue, HTChangeInfoEntity entity)
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
