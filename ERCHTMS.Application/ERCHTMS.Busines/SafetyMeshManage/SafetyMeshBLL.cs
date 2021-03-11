using ERCHTMS.Entity.SafetyMeshManage;
using ERCHTMS.IService.SafetyMeshManage;
using ERCHTMS.Service.SafetyMeshManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq.Expressions;

namespace ERCHTMS.Busines.SafetyMeshManage
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public class SafetyMeshBLL
    {
        private SafetyMeshIService service = new SafetyMeshService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafetyMeshEntity> GetList()
        {
            return service.GetList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SafetyMeshEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public DataTable GetTableList(string queryJson)
        {
            return service.GetTableList(queryJson);
        }
        public DataTable GetTable(string queryJson)
        {
            return service.GetTable(queryJson);
        }
        public IEnumerable<SafetyMeshEntity> GetListForCon(Expression<Func<SafetyMeshEntity, bool>> condition)
        {
            return service.GetListForCon(condition);
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
        public void SaveForm(string keyValue, SafetyMeshEntity entity)
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
