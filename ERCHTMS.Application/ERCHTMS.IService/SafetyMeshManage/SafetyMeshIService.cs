using ERCHTMS.Entity.SafetyMeshManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System;

namespace ERCHTMS.IService.SafetyMeshManage
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public interface SafetyMeshIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SafetyMeshEntity> GetList();
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SafetyMeshEntity GetEntity(string keyValue);
        DataTable GetTableList(string queryJson);
        /// <summary>
        /// ��ȡDataTable
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetTable(string queryJson);
        IEnumerable<SafetyMeshEntity> GetListForCon(Expression<Func<SafetyMeshEntity, bool>> condition);
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
        void SaveForm(string keyValue, SafetyMeshEntity entity);
        #endregion
    }
}
