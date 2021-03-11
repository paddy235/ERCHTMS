using ERCHTMS.Entity.ComprehensiveManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.ComprehensiveManage
{
    /// <summary>
    /// �� ������Ϣ��������
    /// </summary>
    public interface InfoSubmitDetailsIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<InfoSubmitDetailsEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        InfoSubmitDetailsEntity GetEntity(string keyValue);
        /// <summary>
        /// ���±��״̬
        /// </summary>
        /// <param name="applyId"></param>
        void UpdateChangedData(string infoId);
        #endregion


        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValue);
        void RemoveFormByInfoId(string keyValue);        
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, InfoSubmitDetailsEntity entity);
        #endregion
    }
}
