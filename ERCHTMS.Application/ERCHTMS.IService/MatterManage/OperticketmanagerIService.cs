using ERCHTMS.Entity.MatterManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.MatterManage
{
    /// <summary>
    /// �� ������Ʊ�����볧��Ʊ
    /// </summary>
    public interface OperticketmanagerIService
    {
        #region ��ȡ����


        /// <summary>
        /// ���ɿ�Ʊ����
        /// </summary>
        /// <param name="product">����Ʒ����</param>
        /// <param name="takeGoodsName">�����</param>
        /// <param name="transportType">��������(�����ת��)</param>
        /// <returns></returns>
        string GetTicketNumber(string product, string takeGoodsName, string transportType);

        /// <summary>
        /// ��ȡDataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable GetDataTable(string sql);

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<OperticketmanagerEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        OperticketmanagerEntity GetEntity(string keyValue);
        /// <summary>
        /// ��ȡ�鿴���̹���ʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        OperticketmanagerEntity GetProcessEntity(string keyValue);

        /// <summary>
        /// ��ȡ�����볡��Ʊ��¼��Ϣ
        /// </summary>
        /// <param name="keyValue">���ƺ�</param>
        /// <returns></returns>
        OperticketmanagerEntity GetNewEntity(string keyValue);

        /// <summary>
        /// �б��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// �б��ҳ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable BackGetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ������Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        OperticketmanagerEntity GetCar(string CarNo);

        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, OperticketmanagerEntity entity);

        /// <summary>
        /// ������־
        /// </summary>
        /// <param name="entity"></param>
        void InsetDailyRecord(DailyrRecordEntity entity);

        #endregion
    }
}
