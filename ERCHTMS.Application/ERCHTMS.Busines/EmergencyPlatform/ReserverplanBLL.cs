using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using ERCHTMS.Service.EmergencyPlatform;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Text;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.AuthorizeManage;

namespace ERCHTMS.Busines.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ��Ԥ��
    /// </summary>
    public class ReserverplanBLL
    {
        private IReserverplanService service = new ReserverplanService();

        #region ��ȡ����
        public string GetOptionsString()
        {
            var list = GetList("");

            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.AppendFormat("<option value='{0}'>{1}</option>", item.ID, item.NAME);
            }
            return sb.ToString();
        }



        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<ReserverplanEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ReserverplanEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, ReserverplanEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                throw;
            }
        }
        #endregion
    }
}
