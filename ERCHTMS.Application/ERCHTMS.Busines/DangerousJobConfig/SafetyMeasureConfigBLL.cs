using ERCHTMS.Entity.DangerousJobConfig;
using ERCHTMS.IService.DangerousJobConfig;
using ERCHTMS.Service.DangerousJobConfig;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.DangerousJobConfig
{
    /// <summary>
    /// �� ����Σ����ҵ��ȫ��ʩ����
    /// </summary>
    public class SafetyMeasureConfigBLL
    {
        private SafetyMeasureConfigIService service = new SafetyMeasureConfigService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson)
        {
            return service.GetPageListJson(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<SafetyMeasureConfigEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public SafetyMeasureConfigEntity GetEntity(string keyValue)
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
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafetyMeasureConfigEntity entity,List<SafetyMeasureDetailEntity> list)
        {
            try
            {
                service.SaveForm(keyValue, entity, list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
