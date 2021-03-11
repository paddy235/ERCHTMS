using ERCHTMS.Entity.BaseManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System;
using ERCHTMS.Entity.RiskDatabase;

namespace ERCHTMS.IService.BaseManage
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public interface IDistrictService
    {
        #region ��ȡ����

        /// <summary>
        /// ��ȡ���Źܿ��������Ƽ���
        /// </summary>
        /// <param name="deptId">����Id</param>
        /// <returns>�����б�Json</returns>
        DataTable GetDeptNames(string deptId);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<DistrictEntity> GetList(string orgID = "");

        IEnumerable<DistrictEntity> GetListForCon(Expression<Func<DistrictEntity, bool>> condition);
        IEnumerable<DistrictEntity> GetOrgList(string orgID);


        /// <summary>
        /// ����һ����������ȡ������Ϣ
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<DistrictEntity> GetListByOrgIdAndParentId(string orgId, string parentId);
        List<DistrictEntity> GetDistricts(string companyid, string districtId);

        /// <summary>
        /// ��ȡ���ƺ�ID
        /// </summary>
        /// <param name="IDS">id����</param>
        /// <returns>�����б�</returns>
        DataTable GetNameAndID(string ids);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        DistrictEntity GetEntity(string keyValue);

        /// <summary>
        /// �����б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        IEnumerable<DistrictEntity> GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ���ݻ���Id���������ƻ�ȡ����
        /// </summary>
        /// <param name="orgId">����Id</param>
        /// <param name="name">��������</param>
        /// <returns></returns>

        DistrictEntity GetDistrict(string orgId, string name);
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
        void SaveForm(string keyValue, DistrictEntity entity);
        #endregion
    }
}
