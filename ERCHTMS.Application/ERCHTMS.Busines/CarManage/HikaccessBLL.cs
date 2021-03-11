using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.CarManage
{
    /// <summary>
    /// �� ���������Ž����豸����
    /// </summary>
    public class HikaccessBLL
    {
        private HikaccessIService service = new HikaccessService();

        #region ��ȡ����

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
        public IEnumerable<HikaccessEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HikaccessEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ����hikID��ȡ�豸��Ϣ
        /// </summary>
        /// <param name="HikId"></param>
        /// <returns></returns>
        public HikaccessEntity HikGetEntity(string HikId)
        {
            return service.HikGetEntity(HikId);
        }

        #endregion

        #region �ύ����

        /// <summary>
        /// �Ž�״̬����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="type"></param>
        /// <param name="pitem"></param>
        /// <param name="url"></param>
        public void ChangeControl(string keyValue, int type, string pitem, string url)
        {
            try
            {
                service.ChangeControl(keyValue, type, pitem, url);
            }
            catch (Exception e)
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
        public void SaveForm(string keyValue, HikaccessEntity entity)
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
