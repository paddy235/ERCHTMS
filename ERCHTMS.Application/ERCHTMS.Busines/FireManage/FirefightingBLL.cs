using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using ERCHTMS.Service.FireManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Collections;

namespace ERCHTMS.Busines.FireManage
{
    /// <summary>
    /// �� ����������ʩ
    /// </summary>
    public class FirefightingBLL
    {
        private FirefightingIService service = new FirefightingService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<FirefightingEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public FirefightingEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public DataTable StatisticsData(string queryJson)
        {
            return service.StatisticsData(queryJson);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ����id��������ɾ��
        /// </summary>
        /// <param name="Ids"></param>
        public void Remove(string Ids)
        {
            try
            {
                service.Remove(Ids);
            }
            catch (Exception)
            {

                throw;
            }
        }
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
        public void SaveForm(string keyValue, FirefightingEntity entity)
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

        /// <summary>
        /// ͬһ���ͣ���Ų����ظ�
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Code"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public bool ExistCode(string Type, string Code, string keyValue)
        {
            return service.ExistCode(Type,Code, keyValue);
        }

        /// <summary>
        /// ���������ȡ������µ�������ʩ
        /// </summary>
        /// <param name="areaCodes">�������</param>
        /// <returns></returns>
        public IList GetCountByArea(List<string> areaCodes)
        {
            return service.GetCountByArea(areaCodes);
        }
    }
}
