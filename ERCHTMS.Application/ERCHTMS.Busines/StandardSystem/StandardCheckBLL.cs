using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.IService.StandardSystem;
using ERCHTMS.Service.StandardSystem;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.StandardSystem
{
    /// <summary>
    /// �� ����Υ�±�׼��
    /// </summary>
    public class StandardCheckBLL
    {
        private StandardCheckIService service = new StandardCheckService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<StandardCheckEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��������༭��ȡ�������˼�¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public StandardCheckEntity GetLastEntityByRecId(string keyValue,string checkType)
        {
            return service.GetLastEntityByRecId(keyValue,checkType);
        }
        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public StandardCheckEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ��ǩ�Ƿ����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="checkUserId"></param>
        /// <returns></returns>
        public bool FinishSign(string keyValue,string checkUserId)
        {
            return service.FinishSign(keyValue,checkUserId);
        }
        /// <summary>
        /// ��ί������Ƿ����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="checkUserId"></param>
        /// <returns></returns>
        public bool FinishCommittee(string keyValue, string checkUserId)
        {
            return service.FinishCommittee(keyValue, checkUserId);
        }
        /// <summary>
        /// �Ƿ�ȫ��������
        /// </summary>
        /// <returns></returns>
        public bool FinishComplete(string checkUserId,string checkUserName, string checkType)
        {
            return service.FinishComplete(checkUserId, checkUserName,checkType);
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
        public void SaveForm(string keyValue, StandardCheckEntity entity)
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
