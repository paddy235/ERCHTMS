using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using ERCHTMS.Service.EmergencyPlatform;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ��������¼�����
    /// </summary>
    public class DrillplanrecordstepBLL
    {
        private DrillplanrecordstepIService service = new DrillplanrecordstepService();

        #region ��ȡ����


        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<DrillplanrecordstepEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public DrillplanrecordstepEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }


        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// ����recid��ȡ�����б�
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        public IList<DrillplanrecordstepEntity> GetListByRecid(string recid)
        {
            return service.GetListByRecid(recid);
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
        public void SaveForm(string keyValue, DrillplanrecordstepEntity entity)
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

        /// <summary>
        /// ���ݹ���IDɾ������
        /// </summary>
        /// <param name="recid"></param>
        public void RemoveFormByRecid(string recid)
        {
            try
            {
                service.RemoveFormByRecid(recid);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
    }
}
