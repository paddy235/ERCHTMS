using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using ERCHTMS.Service.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Linq;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.CarManage;
using Newtonsoft.Json;

namespace ERCHTMS.Busines.KbsDeviceManage
{
    /// <summary>
    /// �� ��������ʲ�Ž�����
    /// </summary>
    public class KbsdeviceBLL
    {
        private KbsdeviceIService service = new KbsdeviceService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡȫ���б�����
        /// </summary>
        /// <returns>���ط�ҳ�б�</returns>
        public List<KbsdeviceEntity> GetPageList(string queryJson)
        {
            var list = service.GetPageList();
            if (!string.IsNullOrEmpty(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                //�Ƿ�����
                if (!queryParam["selectStatus"].IsEmpty())
                {
                    string selectStatus = queryParam["selectStatus"].ToString();
                    list = list.Where(it => it.State == selectStatus).ToList();
                }

                if (!queryParam["AreaCode"].IsEmpty())
                {
                    string AreaCode = queryParam["AreaCode"].ToString();
                    list = list.Where(it => it.AreaCode.Contains(AreaCode)).ToList();
                }

                if (!queryParam["Search"].IsEmpty())
                {
                    string Search = queryParam["Search"].ToString();
                    list = list.Where(it => it.DeviceId.Contains(Search) || it.DeviceName.Contains(Search) || it.DeviceModel.Contains(Search) || it.AreaName.Contains(Search) || it.FloorNo.Contains(Search) || it.OperUserName.Contains(Search)).ToList();
                }
            }

            return list;
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<KbsdeviceEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public KbsdeviceEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ����״̬��ȡ����ͷ����
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public int GetDeviceNum(string status)
        {
            return service.GetDeviceNum(status);
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
                RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                SendData sd = new SendData();
                sd.DataName = "RemoveDevice";
                sd.EntityString = keyValue;
                rh.SendMessage(JsonConvert.SerializeObject(sd));
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
        public void SaveForm(string keyValue, KbsdeviceEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
                RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                SendData sd = new SendData();
                if (keyValue == "")
                {
                    sd.DataName = "AddDevice";
                }
                else
                {
                    sd.DataName = "UpdateDevice";
                }
                sd.EntityString = JsonConvert.SerializeObject(entity);
                rh.SendMessage(JsonConvert.SerializeObject(sd));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// �ӿ��޸�״̬�÷���
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateState(KbsdeviceEntity entity)
        {
            try
            {
                service.UpdateState(entity);
            }
            catch (Exception e)
            {
                throw;
                throw;
            }
        }

        #endregion
    }
}
