using ERCHTMS.Entity.LllegalStandard;
using ERCHTMS.IService.LllegalStandard;
using ERCHTMS.Service.LllegalStandard;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.LllegalStandard
{
    /// <summary>
    /// �� ����Υ�±�׼��
    /// </summary>
    public class LllegalstandardBLL
    {
        private LllegalstandardIService service = new LllegalstandardService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<LllegalstandardEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public DataTable GetLllegalStdInfo(Pagination pagination, string queryJson)
        {
            return service.GetLllegalStdInfo(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public LllegalstandardEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, LllegalstandardEntity entity)
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
