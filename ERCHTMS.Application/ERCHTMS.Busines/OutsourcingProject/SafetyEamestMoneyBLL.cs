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
    /// �� ������ȫ��֤��
    /// </summary>
    public class SafetyEamestMoneyBLL
    {
        private SafetyEamestMoneyIService service = new SafetyEamestMoneyService();

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
        public void RemoveExamineForm(string keyValue)
        {
            try
            {
                service.RemoveExamineForm(keyValue);
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
        public void SaveForm(string keyValue, string state, string auditid, SafetyEamestMoneyEntity entity,List<SafetyMoneyExamineEntity> list)
        {
            try
            {
                service.SaveForm(keyValue,state,auditid, entity,list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
