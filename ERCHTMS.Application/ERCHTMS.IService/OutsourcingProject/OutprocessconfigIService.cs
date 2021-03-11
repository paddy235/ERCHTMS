using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// �� ��������������ñ�
    /// </summary>
    public interface OutprocessconfigIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<OutprocessconfigEntity> GetList();
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        OutprocessconfigEntity GetEntity(string keyValue);
        /// <summary>
        /// ��ȡ��ҳ����
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetPageListJson(Pagination pagination, string queryJson);
        /// <summary>
        /// �жϸõ糧�Ƿ���ڸ�ģ�������
        /// </summary>
        /// <param name="deptid">�糧ID</param>
        /// <param name="moduleCode">ģ��Code</param>
        /// <returns>0:������ >0 ����</returns>
        int IsExistByModuleCode(string deptid, string moduleCode);

        OutprocessconfigEntity GetEntityByModuleCode(string deptid, string moduleCode);
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
        void SaveForm(string keyValue, OutprocessconfigEntity entity);

        /// <summary>
        /// ɾ����������
        /// </summary>
        /// <param name="recid"></param>
        void DeleteLinkData(string recid);
        #endregion
    }
}
