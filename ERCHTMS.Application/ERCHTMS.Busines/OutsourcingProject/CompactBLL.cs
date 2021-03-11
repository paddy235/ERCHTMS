using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// �� ������ͬ
    /// </summary>
    public class CompactBLL
    {
        private CompactIService service = new CompactService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public DataTable GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public object GetCompactProtocol(string keyValue)
        {
            return service.GetCompactProtocol(keyValue);
        }

        public object GetLastCompactProtocol(string keyValue)
        {
            return service.GetLastCompactProtocol(keyValue);
        }

        #region ��ȡ�����µĺ�ͬ��Ϣ
        /// <summary>
        /// ��ȡ�����µĺ�ͬ��Ϣ
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<CompactEntity> GetListByProjectId(string projectId)
        {
            return service.GetListByProjectId(projectId);
        }
        /// <summary>
        /// ��������Id��ȡ��ͬ����
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DataTable GetComoactTimeByProjectId(string projectId) {
            return service.GetComoactTimeByProjectId(projectId);
        }
        #endregion
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
        /// ����������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, CompactEntity entity)
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