using ERCHTMS.Entity.SaftyCheck;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.IService.SaftyCheck
{
    /// <summary>
    /// �� ������ȫ��������
    /// </summary>
    public interface SaftyCheckDataDetailIService
    {
        #region ��ȡ����
        /// <summary>
        /// ���ĵǼ�״̬
        /// </summary>
        void RegisterPer(string userAccount, string id);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SaftyCheckDataDetailEntity> GetList(string queryJson);
        DataTable GetDetails(string ids);
          /// <summary>
        /// ��ȡ����¼������ݵ�����
        /// </summary>
        /// <param name="recId"></param>
        /// <returns></returns>
        int GetCount(string recId);
        int GetCheckItemCount(string recId);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SaftyCheckDataDetailEntity GetEntity(string keyValue);
        /// <summary>
        /// ��ȫ���������б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        IEnumerable<SaftyCheckDataDetailEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȫ�����б�(ϵͳ����)
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetPageOfSysCreate(Pagination pagination, string queryJson);

          /// <summary>
        /// ��ȫ�����б�(ϵͳ����)
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetListOfSysCreate(string queryJson);

        
        /// <summary>
        /// ��ȡ �������
        /// </summary>
        /// <param name="baseID">���յ�ID</param>
        DataTable GetPageContent(string baseID);

         /// <summary>
        /// ��ȡ��Ա��Ҫ������Ŀ����
        /// </summary>
        /// <param name="recId">���ƻ�Id</param>
        /// <param name="userAccount">�û��˺�</param>
        /// <returns></returns>
        int GetCheckCount(string recId, string userAccount);
        DataTable GetDataTableList(Pagination pagination, string queryJson);
        
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
        void SaveForm(string keyValue, List<SaftyCheckDataDetailEntity> list);
        void SaveFormToContent(string keyValue, List<SaftyCheckDataDetailEntity> list);
        void SaveResultForm(List<SaftyCheckDataDetailEntity> list);
        int Remove(string recid);
        void Update(string keyValue, SaftyCheckDataDetailEntity entity);
         /// <summary>
        /// ���ݼ���¼ɾ�������Ŀ
        /// </summary>
        /// <param name="recId"></param>
        /// <returns></returns>
      
                /// <summary>
        /// ��������Ŀ��Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="list">�����Ŀ</param>
        /// <param name="deptCode">������Ĳ��ţ����Ӣ�Ķ��ŷָ���</param>
        void Save(string keyValue, List<SaftyCheckDataDetailEntity> list, string deptCode="");
        void Save(string keyValue, List<SaftyCheckDataDetailEntity> list, SaftyCheckDataRecordEntity entity, Operator user, string deptCode = "");
        #endregion

        #region ��ȡ����(�ֻ���)
        IEnumerable<SaftyCheckDataDetailEntity> GetSaftyDataDetail(string safeCheckIdItem);
        void insertIntoDetails(string checkExcelId, string recid);
        #endregion
    }
}
