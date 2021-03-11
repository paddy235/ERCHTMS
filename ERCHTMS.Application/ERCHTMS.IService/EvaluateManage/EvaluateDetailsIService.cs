using ERCHTMS.Entity.EvaluateManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.StandardSystem;

namespace ERCHTMS.IService.EvaluateManage
{
    /// <summary>
    /// �� �����Ϲ���������ϸ
    /// </summary>
    public interface EvaluateDetailsIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<EvaluateDetailsEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        EvaluateDetailsEntity GetEntity(string keyValue);
        
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
        void SaveForm(string keyValue, EvaluateDetailsEntity entity);
        #endregion
        /// <summary>
        /// �����ļ����ƻ�ȡ�������ƣ�ȡ����ࣩ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        string getStCategory(string str);

    }
}
