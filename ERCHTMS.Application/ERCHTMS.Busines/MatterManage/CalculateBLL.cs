using ERCHTMS.Entity.MatterManage;
using ERCHTMS.IService.MatterManage;
using ERCHTMS.Service.MatterManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.MatterManage
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public class CalculateBLL
    {
        private CalculateIService service = new CalculateService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<CalculateEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// �б��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// �����б��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetNewPageList(Pagination pagination, string queryJson)
        {
            return service.GetNewPageList(pagination, queryJson);
        }

        /// <summary>
        /// ��ȡ����ͳ���б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetCountPageList(Pagination pagination, string queryJson)
        {
            return service.GetCountPageList(pagination, queryJson);
        }

        /// <summary>
        /// ��ȡ�ذ�Ա�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageUserList(Pagination pagination, string queryJson,string res)
        {
            return service.GetPageUserList(pagination, queryJson,res);
        }

        /// <summary>
        /// ��ȡ�û���Ȩ��¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public UserEmpowerRecordEntity GetUserRecord(string keyValue)
        {
            try
            {
                return service.GetUserRecord(keyValue);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public CalculateEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ���³��ؼ�����Ϣ
        /// </summary>
        /// <param name="keyValue">���ص���</param>
        /// <returns></returns>
        public CalculateEntity GetNewEntity(string keyValue)
        {
            return service.GetNewEntity(keyValue);
        }


        /// <summary>
        ///����δ��������
        /// </summary>
        /// <returns>�����б�</returns>
        public CalculateEntity GetEntranceTicket(string carNo)
        {
            return service.GetEntranceTicket(carNo);
        }


    /// <summary>
    /// ��ȡ��¼���������¼ʵ��
    /// </summary>
    /// <param name="keyValue">����ֵ</param>
    /// <returns></returns>
    public CalculateDetailedEntity GetAppDetailedEntity(string keyValue)
        {
            return service.GetAppDetailedEntity(keyValue);
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
        public void SaveForm(string keyValue, CalculateEntity entity)
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
        /// �ֻ��ӿڱ���
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveAppForm(string keyValue, CalculateEntity entity)
        {
            try
            {
                service.SaveAppForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// �ֻ��ӿڱ���
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveWeightBridgeDetail(string keyValue, CalculateDetailedEntity entity)
        {
            try
            {
                service.SaveWeightBridgeDetail(keyValue, entity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        /// <summary>
        /// �����û���Ȩ��Ϣ
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveUserForm(string keyValue, UserEmpowerRecordEntity entity)
        {
            try
            {
                service.SaveUserForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

      



        #endregion
    }
}
