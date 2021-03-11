using ERCHTMS.Entity.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace ERCHTMS.IService.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������ȫ���ƻ�
    /// </summary>
    public interface MatrixsafecheckIService
    {
        #region ��ȡ����
        string GetActionNum();
        /// <summary>
        /// ������ȡ����
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetCanlendarListJson(string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetPageListJson(Pagination pagination, string queryJson);
        DataTable GetInfoBySql(string sql);

        int ExecuteBySql(string sql);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<MatrixsafecheckEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        MatrixsafecheckEntity GetEntity(string keyValue);

        MatrixsafecheckEntity SetFormJson(string keyValue, string recid);

        DataTable GetContentPageJson(string queryJson);

        DataTable GetDeptPageJson(string queryJson);
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
        void SaveForm(string keyValue, MatrixsafecheckEntity entity);
        #endregion
    }
}
