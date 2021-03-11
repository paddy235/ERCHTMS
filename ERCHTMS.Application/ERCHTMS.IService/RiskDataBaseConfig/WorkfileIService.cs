using ERCHTMS.Entity.RiskDataBaseConfig;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.RiskDataBaseConfig
{
    /// <summary>
    /// �� �������������嵥˵����
    /// </summary>
    public interface WorkfileIService
    {
        #region ��ȡ����
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<WorkfileEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        WorkfileEntity GetEntity(string keyValue);
          /// <summary>
        /// ���ݻ���Code��ѯ�������Ƿ��Ѿ����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        bool GetIsExist(string orgCode);
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
        void SaveForm(string keyValue, WorkfileEntity entity);
        #endregion
    }
}
