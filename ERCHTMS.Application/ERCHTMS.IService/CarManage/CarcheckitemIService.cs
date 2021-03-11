using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// �� ��������Σ��Ʒ�����Ŀ��
    /// </summary>
    public interface CarcheckitemIService
    {
        #region ��ȡ����

        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<CarcheckitemEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        CarcheckitemEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡȥ�ص�Σ�������б�
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        IEnumerable<DataItemModel> GetHazardousList(string KeyValue);

        IEnumerable<DataItemModel> GetCurrentList(string KeyValue);
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
        void SaveForm(string keyValue, string CheckItemName, List<CarcheckitemhazardousEntity> HazardousArray, List<CarcheckitemmodelEntity> ItemArray);
        #endregion
    }
}
