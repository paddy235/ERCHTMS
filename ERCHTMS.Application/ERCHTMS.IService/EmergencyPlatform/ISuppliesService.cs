using ERCHTMS.Entity.EmergencyPlatform;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System;

namespace ERCHTMS.IService.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ������
    /// </summary>
    public interface ISuppliesService
    {
        #region ��ȡ����

        /// <summary>
        /// ��ȡ����SuppliesCode
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        string GetMaxCode();

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SuppliesEntity> GetListForCon(Expression<Func<SuppliesEntity, bool>> condition);

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
        IEnumerable<SuppliesEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SuppliesEntity GetEntity(string keyValue);

        IEnumerable<SuppliesEntity> GetMutipleDataJson(string Ids);

        /// <summary>
        /// ���������˻�ȡ���������
        /// </summary>
        /// <param name="DutyPerson"></param>
        /// <returns></returns>
        IEnumerable<SuppliesEntity> GetDutySuppliesDataJson(string DutyPerson);

        DataTable CheckRemove(string keyvalue);
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
        void SaveForm(string keyValue, SuppliesEntity entity);
        void SaveForm(List<SuppliesEntity> slist);
        #endregion
    }
}
