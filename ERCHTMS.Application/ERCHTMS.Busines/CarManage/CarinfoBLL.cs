using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using BSFramework.Util;
using ERCHTMS.Code;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.SystemManage;

namespace ERCHTMS.Busines.CarManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class CarinfoBLL
    {
        private CarinfoIService service = new CarinfoService();
        private IDataItemDetailService dataItemservice = new DataItemDetailService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<CarinfoEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// ��ȡ¼�복����
        /// </summary>
        /// <returns></returns>
        public List<CarinfoEntity> GetGspCar()
        { 
            return service.GetGspCar();
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public CarinfoEntity GetUserCar(string userid)
        {
            return service.GetUserCar(userid);
        }

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ������Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public CarinfoEntity GetBusCar(string CarNo)
        {
            return service.GetBusCar(CarNo);
        }

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ������Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public CarinfoEntity GetCar(string CarNo)
        {
            return service.GetCar(CarNo);
        }

        /// <summary>
        /// ���ƺ��Ƿ����ظ�
        /// </summary>
        /// <param name="CarNo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool GetCarNoIsRepeat(string CarNo, string id)
        {
            return service.GetCarNoIsRepeat(CarNo, id);
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
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public CarinfoEntity GetEntity(string keyValue)
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
                var data = dataItemservice.GetDataItemListByItemCode("'SocketUrl'");
                string IP = "";
                int Port = 0;
                foreach (var item in data)
                {
                    if (item.ItemName == "IP")
                    {
                        IP = item.ItemValue;
                    }
                    else if (item.ItemName == "Port")
                    {
                        Port = Convert.ToInt32(item.ItemValue);
                    }
                }
                service.RemoveForm(keyValue, IP, Port);
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
        public void SaveForm(string keyValue, CarinfoEntity entity, string pitem, string url)
        {
            try
            {
                var data = dataItemservice.GetDataItemListByItemCode("'SocketUrl'");
                string IP = "";
                int Port = 0;
                foreach (var item in data)
                {
                    if (item.ItemName == "IP")
                    {
                        IP = item.ItemValue;
                    }
                    else if (item.ItemName == "Port")
                    {
                        Port = Convert.ToInt32(item.ItemValue);
                    }
                }
                service.SaveForm(keyValue, entity, pitem, url, IP,Port);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveForm(string keyValue, CarinfoEntity entity)
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

        public void CartoExamine(string keyValue, CarinfoEntity entity)
        {
            try
            {
                service.CartoExamine(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// �޸ĺ���������Ϣ
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="OldCar"></param>
        public void UpdateHiaKangCar(CarinfoEntity entity, string OldCar)
        {
            try
            {
                service.UpdateHiaKangCar(entity, OldCar);
            }
            catch (Exception)
            {
                throw;
            }
        }



        #endregion
    }
}
