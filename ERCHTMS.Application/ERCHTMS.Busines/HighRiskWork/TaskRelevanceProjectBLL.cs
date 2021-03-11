using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// �� �����Ѽ��ļ����Ŀ
    /// </summary>
    public class TaskRelevanceProjectBLL
    {
        private TaskRelevanceProjectIService service = new TaskRelevanceProjectService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TaskRelevanceProjectEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TaskRelevanceProjectEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ���ݼල�����ȡ�Ѽ����Ŀ
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public IEnumerable<TaskRelevanceProjectEntity> GetEndCheckInfo(string superviseid)
        {
            return service.GetEndCheckInfo(superviseid);
        }

        /// <summary>
        /// ���ݼ����Ŀid�ͼල����id��ȡ��Ϣ
        /// </summary>
        /// <returns></returns>
        public TaskRelevanceProjectEntity GetCheckResultInfo(string checkprojectid, string superviseid)
        {
            return service.GetCheckResultInfo(checkprojectid, superviseid);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageDataTable(Pagination pagination)
        {
            return service.GetPageDataTable(pagination);
        }

        /// <summary>
        /// ���ݼලid��ȡ������Ϣ
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetTaskHiddenInfo(string superviseid)
        {
            return service.GetTaskHiddenInfo(superviseid);
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
        public void SaveForm(string keyValue, TaskRelevanceProjectEntity entity)
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
