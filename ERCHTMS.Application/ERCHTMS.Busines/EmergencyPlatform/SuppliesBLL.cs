using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using ERCHTMS.Service.EmergencyPlatform;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq.Expressions;

namespace ERCHTMS.Busines.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ������
    /// </summary>
    public class SuppliesBLL
    {
        private ISuppliesService service = new SuppliesService();

        #region ��ȡ����

        /// <summary>
        public string GetMaxCode()
        {
            return service.GetMaxCode();
        }

        public IEnumerable<SuppliesEntity> GetListForCon(Expression<Func<SuppliesEntity, bool>> condition)
        {
            return service.GetListForCon(condition);
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SuppliesEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SuppliesEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public IEnumerable<SuppliesEntity> GetMutipleDataJson(string Ids)
        {
            return service.GetMutipleDataJson(Ids);
        }

        /// <summary>
        /// ���������˻�ȡ���������
        /// </summary>
        /// <param name="DutyPerson"></param>
        /// <returns></returns>
        public IEnumerable<SuppliesEntity> GetDutySuppliesDataJson(string DutyPerson)
        {
            return service.GetDutySuppliesDataJson(DutyPerson);
        }

        public DataTable CheckRemove(string keyvalue)
        {
            return service.CheckRemove(keyvalue);
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
        public void SaveForm(string keyValue, SuppliesEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SaveForm(List<SuppliesEntity> slist)
        {
            try
            {
                service.SaveForm(slist);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
