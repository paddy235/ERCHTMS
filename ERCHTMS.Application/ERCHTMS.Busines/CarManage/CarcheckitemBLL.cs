using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Busines.CarManage
{
    /// <summary>
    /// �� ��������Σ��Ʒ�����Ŀ��
    /// </summary>
    public class CarcheckitemBLL
    {
        private CarcheckitemIService service = new CarcheckitemService();

        #region ��ȡ����

        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<CarcheckitemEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public CarcheckitemEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡȥ�ص�Σ�������б�
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public IEnumerable<DataItemModel> GetHazardousList(string KeyValue)
        {
            return service.GetHazardousList(KeyValue);
        }

        /// <summary>
        /// ��ȡͨ���Ÿ�
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public IEnumerable<DataItemModel> GetCurrentList(string KeyValue)
        {
            return service.GetCurrentList(KeyValue);
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
        public void SaveForm(string keyValue, string CheckItemName, List<CarcheckitemhazardousEntity> HazardousArray, List<CarcheckitemmodelEntity> ItemArray)
        {
            try
            {
                service.SaveForm(keyValue, CheckItemName, HazardousArray, ItemArray);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
