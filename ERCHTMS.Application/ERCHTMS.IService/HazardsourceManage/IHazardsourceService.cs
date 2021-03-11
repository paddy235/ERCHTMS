using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.HazardsourceManage;

namespace ERCHTMS.IService.HazardsourceManage
{
    /// <summary>
    /// �� ����Σ��Դ��ʶ����
    /// </summary>
    public interface IHazardsourceService
    {
        #region ��ȡ����

        DataTable FindTableBySql(string sql);

        /// <summary>
        /// ִ��sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int ExecuteBySql(string sql);


        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<HazardsourceEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        HazardsourceEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, HazardsourceEntity entity);
        #endregion
    }
}
