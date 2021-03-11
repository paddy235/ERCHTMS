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
    /// �� ����Υ����Ϣ��
    /// </summary>
    public class CarviolationBLL
    {
        private CarviolationIService service = new CarviolationService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<CarviolationEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ѯ�б�
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
        public CarviolationEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, CarviolationEntity entity)
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
        /// ����һ��Υ�½ӿ�
        /// </summary>
        public void AddViolation(string id, int type, int ViolationType, string ViolationMsg)
        {

            service.AddViolation(id, type, ViolationType, ViolationMsg);

        }

        /// <summary>
        /// ���복��������Ϣ
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(CarviolationEntity entity)
        {
            service.Insert(entity);
        }

        /// <summary>
        /// Ԥ����������
        /// </summary>
        /// <returns></returns>
        public List<CarviolationEntity> GetIndexWaring()
        {
            return service.GetIndexWaring();
        }

        public object GetIndexWaringCount()
        {
            return service.GetIndexWaringCount();
        }

        /// <summary>
        /// ��ȡ���е�δ�����Ԥ����Ϣ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public List<CarviolationEntity> GetUntreatedWaringList(Pagination pagination, string queryJson)
        {
            return service.GetUntreatedWaringList(pagination, queryJson);
        }

        #endregion
    }
}
