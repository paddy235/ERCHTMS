using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// �� ����1.���ּ�������Ŀ
    /// </summary>
    public class ScaffoldprojectBLL
    {
        private ScaffoldprojectIService service = new ScaffoldprojectService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="scaffoldid">��ѯ����</param>
        /// <returns>�����б�</returns>
        public List<ScaffoldprojectEntity> GetList(string scaffoldid)
        {
            return service.GetList(scaffoldid);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ScaffoldprojectEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<ScaffoldprojectEntity> GetListByCondition(string queryJson)
        {
            return service.GetListByCondition(queryJson);
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
        /// �����������ʽɾ��
        /// </summary>
        /// <param name="condition"></param>
        public void RemoveForm(Expression<Func<ScaffoldprojectEntity, bool>> condition)
        {
            try
            {
                service.RemoveForm(condition);
            }
            catch (Exception ex)
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
        public void SaveForm(string keyValue, ScaffoldprojectEntity entity)
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
