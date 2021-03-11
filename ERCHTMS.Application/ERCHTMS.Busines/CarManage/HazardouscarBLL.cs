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
    /// �� ����Σ�����س�����
    /// </summary>
    public class HazardouscarBLL
    {
        private HazardouscarIService service = new HazardouscarService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HazardouscarEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// ��ҳ��ѯ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HazardouscarEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ��Σ�������Ƿ������˼���
        /// </summary>
        /// <param name="HazardousId"></param>
        /// <returns></returns>
        public bool GetHazardous(string HazardousId)
        {
            return service.GetHazardous(HazardousId);
        }

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ������Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public HazardouscarEntity GetCar(string CarNo)
        {
            return service.GetCar(CarNo);
        }

        /// <summary>
        /// ��ȡ����Σ��Ʒ��������
        /// </summary>
        /// <returns></returns>
        public List<HazardouscarEntity> GetHazardousList(string day)
        {
            return service.GetHazardousList(day);
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
        public void SaveForm(string keyValue, HazardouscarEntity entity)
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
        /// �����������Ա������ͼƬ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="userjson"></param>
        public void SaveFaceUserForm(string keyValue, HazardouscarEntity entity, List<CarUserFileImgEntity> userjson)
        {
            try
            {
                service.SaveFaceUserForm(keyValue, entity,userjson);
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
        public void Update(string keyValue, HazardouscarEntity entity)
        {
            try
            {
                service.Update(keyValue, entity);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// �ı�GPS����Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        public void ChangeGps(string keyValue, HazardouscarEntity entity, List<PersongpsEntity> pgpslist)
        {
            try
            {
                service.ChangeGps(keyValue, entity, pgpslist);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// �ı�Σ��Ʒ��������״̬λ�������
        /// </summary>
        /// <param name="id"></param>
        public void ChangeProcess(string id)
        {
            try
            {
                service.ChangeProcess(id);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion
    }
}
