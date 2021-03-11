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
    /// �� ������Ʊ�����볧��Ʊ
    /// </summary>
    public class OperticketmanagerBLL
    {
        private OperticketmanagerIService service = new OperticketmanagerService();

        #region ��ȡ����


        /// <summary>
        /// ���ɿ�Ʊ����
        /// </summary>
        /// <param name="product">����Ʒ����</param>
        /// <param name="takeGoodsName">�����</param>
        /// <param name="transportType">��������(�����ת��)</param>
        /// <returns></returns>
        public string GetTicketNumber(string product, string takeGoodsName, string transportType)
        {

            return service.GetTicketNumber(product,  takeGoodsName,  transportType);
        }

        /// <summary>
        /// ��ȡDataTable
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            return service.GetDataTable(sql);
        }
        
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OperticketmanagerEntity> GetList(string queryJson)
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
        /// �б��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable BackGetPageList(Pagination pagination, string queryJson)
        {
            return service.BackGetPageList(pagination, queryJson);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OperticketmanagerEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ�鿴���̹���ʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OperticketmanagerEntity GetProcessEntity(string keyValue)
        {
            return service.GetProcessEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ�����볡��Ʊ��¼��Ϣ
        /// </summary>
        /// <param name="keyValue">���ƺ�</param>
        /// <returns></returns>
        public OperticketmanagerEntity GetNewEntity(string keyValue)
        {
            return service.GetNewEntity(keyValue);
        }

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ������Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public OperticketmanagerEntity GetCar(string CarNo)
        {
            return service.GetCar(CarNo);
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
        public void SaveForm(string keyValue, OperticketmanagerEntity entity)
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
        /// ��ӹ�����¼��־
        /// </summary>
        /// <param name="entity"></param>
        public void InsetDailyRecord(DailyrRecordEntity entity)
        {
            try
            {
                service.InsetDailyRecord(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }


        
        #endregion
    }
}
