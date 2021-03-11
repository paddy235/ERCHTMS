using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq.Expressions;
using ERCHTMS.Entity.RiskDatabase;

namespace ERCHTMS.Busines.BaseManage
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public class DistrictBLL
    {
        private IDistrictService service = new DistrictService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ���Źܿ��������Ƽ���
        /// </summary>
        /// <param name="deptId">����Id</param>
        /// <returns>�����б�Json</returns>
        public DataTable GetDeptNames(string deptId)
        {
            return service.GetDeptNames(deptId);
        }
        /// <summary>
        /// ���ݹ�˾id��ѯ������������
        /// </summary>
        /// <param name="orgID"></param>
        /// <returns></returns>
        public IEnumerable<DistrictEntity> GetOrgList(string orgID)
        {
            return service.GetOrgList(orgID);
        }

        public List<DistrictEntity> GetDistricts(string companyid, string districtId)
        {
            return service.GetDistricts(companyid, districtId);
        }

        /// <summary>
        /// ����һ����������ȡ������Ϣ
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<DistrictEntity> GetListByOrgIdAndParentId(string orgId, string parentId)
        {
            try
            {
                return service.GetListByOrgIdAndParentId(orgId, parentId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<DistrictEntity> GetListForCon(Expression<Func<DistrictEntity, bool>> condition)
        {
            return service.GetListForCon(condition);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<DistrictEntity> GetList(string orgID = "")
        {
            return service.GetList(orgID);
        }
        /// <summary>
        /// ��ȡ���ƺ�ID
        /// </summary>
        /// <param name="IDS">id����</param>
        /// <returns>�����б�</returns>
        public DataTable GetNameAndID(string ids)
        {
            return service.GetNameAndID(ids);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public DistrictEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// �����б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public IEnumerable<DistrictEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// ���ݻ���Id���������ƻ�ȡ����
        /// </summary>
        /// <param name="orgId">����Id</param>
        /// <param name="name">��������</param>
        /// <returns></returns>
        public DistrictEntity GetDistrict(string orgId, string name)
        {
            return service.GetDistrict(orgId, name);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, DistrictEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
